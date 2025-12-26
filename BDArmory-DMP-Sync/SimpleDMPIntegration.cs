using System;
using UnityEngine;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Simplified version - just logs that sync would happen
    /// Future versions can integrate with DMP's actual API
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class SimpleDMPIntegration : MonoBehaviour
    {
        private static SimpleDMPIntegration instance;
        public static SimpleDMPIntegration Instance => instance;

        private BuildingSyncModule buildingSync;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            Debug.Log("[BDArmoryDMPSync] Simple version loaded - logging sync events only");
        }

        void Start()
        {
            buildingSync = gameObject.AddComponent<BuildingSyncModule>();
            Debug.Log("[BDArmoryDMPSync] Initialized - ready to track building damage");
        }

        // Stub methods for now - log what would be sent
        public void SendBuildingDamage(BuildingDamageMessage message)
        {
            Debug.Log($"[BDArmoryDMPSync] Would send building damage: {message.BuildingId} = {message.DamageFraction:F2}");
        }

        public void SendVesselDamage(VesselDamageMessage message)
        {
            Debug.Log($"[BDArmoryDMPSync] Would send vessel damage: {message.VesselId} part {message.PartFlightId} damage {message.Damage:F1}");
        }

        public void SendWeaponFire(WeaponFireMessage message)
        {
            Debug.Log($"[BDArmoryDMPSync] Would send weapon fire: {message.WeaponName} firing={message.IsFiring}");
        }

        public void SendExplosion(ExplosionMessage message)
        {
            Debug.Log($"[BDArmoryDMPSync] Would send explosion: power={message.ExplosionPower:F1}");
        }

        void OnDestroy()
        {
            Debug.Log("[BDArmoryDMPSync] Shutting down");
            instance = null;
        }
    }
}
