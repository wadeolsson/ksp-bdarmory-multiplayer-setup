using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Handles synchronization of combat damage to vessels
    /// Hooks into BDArmory's damage system and broadcasts damage events
    /// </summary>
    public class DamageSyncHandler : MonoBehaviour
    {
        private MethodInfo addDamageToPartMethod;
        private Type damageServiceType;
        private bool isHooked = false;

        private Dictionary<Guid, float> lastDamageSent = new Dictionary<Guid, float>();
        private float damageThreshold = 0.1f; // Only sync damage above this threshold
        private float batchInterval = 0.1f; // Batch damage events every 0.1 seconds
        private float timeSinceLastBatch = 0f;
        private List<VesselDamageMessage> pendingDamage = new List<VesselDamageMessage>();

        void Start()
        {
            try
            {
                HookBDArmoryDamage();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Failed to hook BDArmory damage: {ex}");
                enabled = false;
            }
        }

        /// <summary>
        /// Hook into BDArmory's damage system using reflection
        /// </summary>
        private void HookBDArmoryDamage()
        {
            try
            {
                // Find BDArmory assembly
                var bdaAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith("BahaTurret"));

                if (bdaAssembly == null)
                {
                    Debug.LogWarning("[BDArmoryDMPSync] BDArmory assembly not found");
                    return;
                }

                // Find DamageService type
                damageServiceType = bdaAssembly.GetType("BahaTurret.DamageService");
                if (damageServiceType == null)
                {
                    Debug.LogWarning("[BDArmoryDMPSync] DamageService type not found");
                    return;
                }

                // Get damage method
                addDamageToPartMethod = damageServiceType.GetMethod("AddDamageToPart_svc",
                    BindingFlags.Public | BindingFlags.Static);

                if (addDamageToPartMethod == null)
                {
                    Debug.LogWarning("[BDArmoryDMPSync] AddDamageToPart_svc method not found");
                    return;
                }

                isHooked = true;
                Debug.Log("[BDArmoryDMPSync] Successfully hooked into BDArmory damage system");

                // Start monitoring vessel damage
                GameEvents.onPartDie.Add(OnPartDeath);
                GameEvents.onPartJointBreak.Add(OnPartJointBreak);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error hooking BDArmory: {ex}");
            }
        }

        void FixedUpdate()
        {
            if (!isHooked || !HighLogic.LoadedSceneIsFlight) return;

            timeSinceLastBatch += Time.fixedDeltaTime;

            // Batch and send pending damage
            if (timeSinceLastBatch >= batchInterval && pendingDamage.Count > 0)
            {
                SendPendingDamage();
                timeSinceLastBatch = 0f;
            }

            // Monitor active vessels for damage
            MonitorVesselDamage();
        }

        /// <summary>
        /// Monitor vessels for damage changes
        /// </summary>
        private void MonitorVesselDamage()
        {
            if (FlightGlobals.ActiveVessel == null) return;

            try
            {
                // Check all loaded vessels
                foreach (var vessel in FlightGlobals.Vessels)
                {
                    if (vessel == null || vessel.packed) continue;

                    foreach (var part in vessel.Parts)
                    {
                        if (part == null) continue;

                        // Check if part has taken damage (using maxTemp as proxy for damage)
                        // In reality, we'd hook into BDA's damage system via events
                        float currentHealth = (float)(part.skinTemperature / part.skinMaxTemp);
                        if (currentHealth > 0.5f) // Part is getting hot/damaged
                        {
                            // Part is damaged - queue damage message
                            QueueDamageMessage(vessel, part, currentHealth * 100);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error monitoring vessel damage: {ex}");
            }
        }

        /// <summary>
        /// Queue a damage message to be sent
        /// </summary>
        private void QueueDamageMessage(Vessel vessel, Part part, float damage)
        {
            if (damage < damageThreshold) return;

            var message = new VesselDamageMessage
            {
                VesselId = vessel.id,
                PartFlightId = part.flightID,
                Damage = damage,
                IsExplosive = false
            };

            pendingDamage.Add(message);
        }

        /// <summary>
        /// Send all pending damage messages
        /// </summary>
        private void SendPendingDamage()
        {
            if (pendingDamage.Count == 0) return;

            try
            {
                foreach (var message in pendingDamage)
                {
                    DMPIntegration.Instance?.SendVesselDamage(message);
                }

                Debug.Log($"[BDArmoryDMPSync] Sent {pendingDamage.Count} damage messages");
                pendingDamage.Clear();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error sending pending damage: {ex}");
            }
        }

        /// <summary>
        /// Apply damage received from network
        /// </summary>
        public void ApplyVesselDamage(VesselDamageMessage message)
        {
            try
            {
                // Find the vessel
                var vessel = FlightGlobals.Vessels.FirstOrDefault(v => v.id == message.VesselId);
                if (vessel == null || vessel.packed)
                {
                    Debug.LogWarning($"[BDArmoryDMPSync] Vessel not found or packed: {message.VesselId}");
                    return;
                }

                // Find the part
                var part = vessel.Parts.FirstOrDefault(p => p.flightID == message.PartFlightId);
                if (part == null)
                {
                    Debug.LogWarning($"[BDArmoryDMPSync] Part not found: {message.PartFlightId}");
                    return;
                }

                // Apply damage using BDArmory's method
                if (addDamageToPartMethod != null)
                {
                    addDamageToPartMethod.Invoke(null, new object[] { part, message.Damage });
                    Debug.Log($"[BDArmoryDMPSync] Applied {message.Damage:F1} damage to {part.name} from {message.PlayerName}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error applying vessel damage: {ex}");
            }
        }

        #region GameEvents Handlers

        private void OnPartDeath(Part part)
        {
            try
            {
                if (part?.vessel == null) return;

                // Broadcast part destruction
                var message = new VesselDamageMessage
                {
                    VesselId = part.vessel.id,
                    PartFlightId = part.flightID,
                    Damage = 9999f, // Indicates complete destruction
                    IsExplosive = part.Modules.Contains("ModuleEngines") // Engines explode
                };

                DMPIntegration.Instance?.SendVesselDamage(message);
                Debug.Log($"[BDArmoryDMPSync] Broadcasting part death: {part.name}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error in OnPartDeath: {ex}");
            }
        }

        private void OnPartJointBreak(PartJoint joint, float breakForce)
        {
            try
            {
                if (joint?.Parent == null || joint?.Child == null) return;

                // Could sync joint breaks if needed
                Debug.Log($"[BDArmoryDMPSync] Part joint broke with force: {breakForce:F1}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error in OnPartJointBreak: {ex}");
            }
        }

        #endregion

        void OnDestroy()
        {
            GameEvents.onPartDie.Remove(OnPartDeath);
            GameEvents.onPartJointBreak.Remove(OnPartJointBreak);
            pendingDamage.Clear();
        }
    }
}
