using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Syncs building damage states across multiplayer
    /// Monitors DestructibleBuilding damage and broadcasts changes
    /// </summary>
    public class BuildingSyncModule : MonoBehaviour
    {
        private Dictionary<string, float> lastKnownDamage = new Dictionary<string, float>();
        private float syncInterval = 1.0f; // Check for changes every second
        private float timeSinceLastSync = 0f;

        void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;

            timeSinceLastSync += Time.fixedDeltaTime;

            if (timeSinceLastSync >= syncInterval)
            {
                timeSinceLastSync = 0f;
                CheckForBuildingDamage();
            }
        }

        /// <summary>
        /// Scan all destructible buildings for damage changes
        /// </summary>
        private void CheckForBuildingDamage()
        {
            try
            {
                var buildings = FindObjectsOfType<DestructibleBuilding>();

                foreach (var building in buildings)
                {
                    if (building == null) continue;

                    string buildingId = GetBuildingId(building);
                    if (string.IsNullOrEmpty(buildingId)) continue;

                    float currentDamage = building.FacilityDamageFraction;

                    // Check if damage has changed
                    if (!lastKnownDamage.ContainsKey(buildingId))
                    {
                        lastKnownDamage[buildingId] = currentDamage;
                    }
                    else if (Math.Abs(lastKnownDamage[buildingId] - currentDamage) > 0.01f)
                    {
                        // Damage changed - broadcast it
                        BroadcastBuildingDamage(buildingId, currentDamage);
                        lastKnownDamage[buildingId] = currentDamage;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error checking building damage: {ex}");
            }
        }

        /// <summary>
        /// Send building damage to other players
        /// </summary>
        private void BroadcastBuildingDamage(string buildingId, float damageFraction)
        {
            try
            {
                var message = new BuildingDamageMessage
                {
                    BuildingId = buildingId,
                    DamageFraction = damageFraction
                };

                DMPIntegration.Instance?.SendBuildingDamage(message);
                Debug.Log($"[BDArmoryDMPSync] Broadcasting building damage: {buildingId} = {damageFraction:F2}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error broadcasting building damage: {ex}");
            }
        }

        /// <summary>
        /// Apply building damage received from network
        /// </summary>
        public void ApplyBuildingDamage(BuildingDamageMessage message)
        {
            try
            {
                var buildings = FindObjectsOfType<DestructibleBuilding>();
                var building = buildings.FirstOrDefault(b => GetBuildingId(b) == message.BuildingId);

                if (building != null)
                {
                    building.FacilityDamageFraction = message.DamageFraction;
                    lastKnownDamage[message.BuildingId] = message.DamageFraction;

                    Debug.Log($"[BDArmoryDMPSync] Applied building damage from {message.PlayerName}: {message.BuildingId} = {message.DamageFraction:F2}");

                    // If building is destroyed, collapse it
                    if (!building.IsIntact)
                    {
                        building.Demolish();
                    }
                }
                else
                {
                    Debug.LogWarning($"[BDArmoryDMPSync] Could not find building: {message.BuildingId}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error applying building damage: {ex}");
            }
        }

        /// <summary>
        /// Get unique identifier for a building
        /// </summary>
        private string GetBuildingId(DestructibleBuilding building)
        {
            try
            {
                // Use the building's object name and position as unique ID
                // This should be consistent across all clients
                var pos = building.transform.position;
                return $"{building.id}_{building.name}_{pos.x:F0}_{pos.y:F0}_{pos.z:F0}";
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Reset all tracked building damage (for reverting to launch or scene changes)
        /// </summary>
        public void ResetBuildingTracking()
        {
            lastKnownDamage.Clear();
            Debug.Log("[BDArmoryDMPSync] Reset building damage tracking");
        }

        void OnDestroy()
        {
            ResetBuildingTracking();
        }
    }
}
