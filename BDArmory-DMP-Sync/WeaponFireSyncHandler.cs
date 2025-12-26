using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Syncs weapon firing states (visual effects only, not projectiles)
    /// This provides visual feedback of remote players firing weapons
    /// </summary>
    public class WeaponFireSyncHandler : MonoBehaviour
    {
        private Dictionary<string, bool> lastFiringState = new Dictionary<string, bool>();
        private float syncInterval = 0.2f; // Check weapon state 5 times per second
        private float timeSinceLastSync = 0f;

        void FixedUpdate()
        {
            if (!HighLogic.LoadedSceneIsFlight) return;

            timeSinceLastSync += Time.fixedDeltaTime;

            if (timeSinceLastSync >= syncInterval)
            {
                timeSinceLastSync = 0f;
                CheckWeaponFiring();
            }
        }

        /// <summary>
        /// Check all active weapons for firing state changes
        /// </summary>
        private void CheckWeaponFiring()
        {
            try
            {
                if (FlightGlobals.ActiveVessel == null) return;

                // Check all loaded vessels
                foreach (var vessel in FlightGlobals.Vessels)
                {
                    if (vessel == null || vessel.packed) continue;

                    foreach (var part in vessel.Parts)
                    {
                        if (part == null) continue;

                        // Look for weapon modules
                        CheckPartWeapons(vessel, part);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error checking weapon firing: {ex}");
            }
        }

        /// <summary>
        /// Check a part for weapon modules and their firing state
        /// </summary>
        private void CheckPartWeapons(Vessel vessel, Part part)
        {
            try
            {
                // Check for ModuleWeapon (guns, missiles, etc.)
                foreach (var module in part.Modules)
                {
                    if (module == null) continue;

                    string moduleName = module.GetType().Name;

                    // Check if it's a BDArmory weapon module
                    if (IsWeaponModule(moduleName))
                    {
                        bool isFiring = IsModuleFiring(module);
                        string weaponKey = $"{vessel.id}_{part.flightID}_{moduleName}";

                        // Check if firing state changed
                        if (!lastFiringState.ContainsKey(weaponKey))
                        {
                            lastFiringState[weaponKey] = isFiring;
                        }
                        else if (lastFiringState[weaponKey] != isFiring)
                        {
                            // Firing state changed - broadcast it
                            BroadcastWeaponFire(vessel, part, moduleName, isFiring);
                            lastFiringState[weaponKey] = isFiring;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error checking part weapons: {ex}");
            }
        }

        /// <summary>
        /// Check if a module is a BDArmory weapon
        /// </summary>
        private bool IsWeaponModule(string moduleName)
        {
            return moduleName.Contains("ModuleWeapon") ||
                   moduleName.Contains("MissileLauncher") ||
                   moduleName.Contains("ModuleTurret") ||
                   moduleName.Contains("BDModularGuidance");
        }

        /// <summary>
        /// Check if a weapon module is currently firing
        /// </summary>
        private bool IsModuleFiring(PartModule module)
        {
            try
            {
                // Try to get firing state via reflection
                var firingField = module.GetType().GetField("isFireing", // Note: BDA typo "Fireing"
                    BindingFlags.Public | BindingFlags.Instance);

                if (firingField == null)
                {
                    firingField = module.GetType().GetField("isFiring",
                        BindingFlags.Public | BindingFlags.Instance);
                }

                if (firingField != null)
                {
                    return (bool)firingField.GetValue(module);
                }

                // Alternative: check armed state
                var armedField = module.GetType().GetField("armed",
                    BindingFlags.Public | BindingFlags.Instance);

                if (armedField != null)
                {
                    return (bool)armedField.GetValue(module);
                }
            }
            catch
            {
                // Silent fail - module might not have these fields
            }

            return false;
        }

        /// <summary>
        /// Broadcast weapon firing state to other players
        /// </summary>
        private void BroadcastWeaponFire(Vessel vessel, Part part, string weaponName, bool isFiring)
        {
            try
            {
                var message = new WeaponFireMessage
                {
                    VesselId = vessel.id,
                    WeaponName = $"{part.name}_{weaponName}",
                    IsFiring = isFiring
                };

                DMPIntegration.Instance?.SendWeaponFire(message);

                if (isFiring)
                {
                    Debug.Log($"[BDArmoryDMPSync] Broadcasting weapon fire: {weaponName} on {vessel.vesselName}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error broadcasting weapon fire: {ex}");
            }
        }

        /// <summary>
        /// Apply weapon fire state from network (visual effects only)
        /// </summary>
        public void ApplyWeaponFire(WeaponFireMessage message)
        {
            try
            {
                // Find the vessel
                var vessel = FlightGlobals.Vessels.FirstOrDefault(v => v.id == message.VesselId);
                if (vessel == null || vessel.packed)
                {
                    return;
                }

                // Find matching weapon modules and update their visual state
                foreach (var part in vessel.Parts)
                {
                    if (part == null) continue;

                    foreach (var module in part.Modules)
                    {
                        if (module == null) continue;

                        string moduleKey = $"{part.name}_{module.GetType().Name}";
                        if (moduleKey == message.WeaponName)
                        {
                            ApplyVisualFiring(module, message.IsFiring);
                        }
                    }
                }

                Debug.Log($"[BDArmoryDMPSync] Applied weapon fire from {message.PlayerName}: {message.WeaponName} = {message.IsFiring}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error applying weapon fire: {ex}");
            }
        }

        /// <summary>
        /// Apply visual firing effects to a weapon module
        /// </summary>
        private void ApplyVisualFiring(PartModule module, bool isFiring)
        {
            try
            {
                // Try to call visual update method
                var updateVisualMethod = module.GetType().GetMethod("UpdateVisualFire",
                    BindingFlags.Public | BindingFlags.Instance);

                if (updateVisualMethod != null)
                {
                    updateVisualMethod.Invoke(module, new object[] { isFiring });
                    return;
                }

                // Alternative: Set animation state
                // Note: Animation is in UnityEngine.AnimationModule which we don't reference
                // For now, skip animation - visual updates will rely on UpdateVisualFire method
                Debug.Log($"[BDArmoryDMPSync] Applied visual state (animation skipped): {isFiring}");
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[BDArmoryDMPSync] Could not apply visual firing: {ex.Message}");
            }
        }

        void OnDestroy()
        {
            lastFiringState.Clear();
        }
    }
}
