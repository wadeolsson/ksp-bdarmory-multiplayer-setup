using System;
using UnityEngine;
using DarkMultiPlayer;

namespace BDArmoryDMPSync
{
    /// <summary>
    /// Main integration point with DarkMultiPlayer
    /// Handles message routing and DMP API interaction
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DMPIntegration : MonoBehaviour
    {
        private static DMPIntegration instance;
        public static DMPIntegration Instance => instance;

        private bool isInitialized = false;
        private string localPlayerName = "";

        // Sync handlers
        private BuildingSyncModule buildingSync;
        private DamageSyncHandler damageSync;
        private WeaponFireSyncHandler weaponSync;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
            Debug.Log("[BDArmoryDMPSync] Awake - Initializing");
        }

        void Start()
        {
            try
            {
                // Check if DMP is available
                if (!IsDMPAvailable())
                {
                    Debug.Log("[BDArmoryDMPSync] DarkMultiPlayer not detected, disabling");
                    enabled = false;
                    return;
                }

                // Check if BDArmory is available
                if (!IsBDArmoryAvailable())
                {
                    Debug.Log("[BDArmoryDMPSync] BDArmory not detected, disabling");
                    enabled = false;
                    return;
                }

                Initialize();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error in Start: {ex}");
                enabled = false;
            }
        }

        void Initialize()
        {
            if (isInitialized) return;

            Debug.Log("[BDArmoryDMPSync] Registering DMP message handlers");

            // Register DMP message handlers
            try
            {
                DMPModInterface.RegisterUpdateModHandler("BDABuilding", HandleBuildingMessage);
                DMPModInterface.RegisterFixedUpdateModHandler("BDADamage", HandleDamageMessage);
                DMPModInterface.RegisterUpdateModHandler("BDAWeapon", HandleWeaponMessage);
                DMPModInterface.RegisterUpdateModHandler("BDAExplosion", HandleExplosionMessage);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Failed to register DMP handlers: {ex}");
                enabled = false;
                return;
            }

            // Initialize sync modules
            buildingSync = gameObject.AddComponent<BuildingSyncModule>();
            damageSync = gameObject.AddComponent<DamageSyncHandler>();
            weaponSync = gameObject.AddComponent<WeaponFireSyncHandler>();

            // Get local player name from DMP
            localPlayerName = GetLocalPlayerName();

            isInitialized = true;
            Debug.Log($"[BDArmoryDMPSync] Initialized successfully as player: {localPlayerName}");
        }

        #region DMP Message Handlers

        private void HandleBuildingMessage(string modName, byte[] data)
        {
            try
            {
                var message = BuildingDamageMessage.Deserialize(data);
                if (message == null) return;

                // Don't process our own messages
                if (message.PlayerName == localPlayerName) return;

                buildingSync?.ApplyBuildingDamage(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error handling building message: {ex}");
            }
        }

        private void HandleDamageMessage(string modName, byte[] data)
        {
            try
            {
                var message = VesselDamageMessage.Deserialize(data);
                if (message == null) return;

                // Don't process our own messages
                if (message.PlayerName == localPlayerName) return;

                damageSync?.ApplyVesselDamage(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error handling damage message: {ex}");
            }
        }

        private void HandleWeaponMessage(string modName, byte[] data)
        {
            try
            {
                var message = WeaponFireMessage.Deserialize(data);
                if (message == null) return;

                // Don't process our own messages
                if (message.PlayerName == localPlayerName) return;

                weaponSync?.ApplyWeaponFire(message);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error handling weapon message: {ex}");
            }
        }

        private void HandleExplosionMessage(string modName, byte[] data)
        {
            try
            {
                var message = ExplosionMessage.Deserialize(data);
                if (message == null) return;

                // Don't process our own messages
                if (message.PlayerName == localPlayerName) return;

                // Apply explosion visual effects
                // (BDArmory's explosion system will handle this)
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Error handling explosion message: {ex}");
            }
        }

        #endregion

        #region Send Messages

        public void SendBuildingDamage(BuildingDamageMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            DMPModInterface.SendDMPModMessage("BDABuilding", data, relay: true, highPriority: true);
        }

        public void SendVesselDamage(VesselDamageMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            DMPModInterface.SendDMPModMessage("BDADamage", data, relay: true, highPriority: true);
        }

        public void SendWeaponFire(WeaponFireMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            DMPModInterface.SendDMPModMessage("BDAWeapon", data, relay: true, highPriority: false);
        }

        public void SendExplosion(ExplosionMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            DMPModInterface.SendDMPModMessage("BDAExplosion", data, relay: true, highPriority: true);
        }

        #endregion

        #region Utility Methods

        private bool IsDMPAvailable()
        {
            try
            {
                // Check if DMPModInterface exists
                var type = Type.GetType("DarkMultiPlayer.DMPModInterface, DarkMultiPlayer");
                return type != null;
            }
            catch
            {
                return false;
            }
        }

        private bool IsBDArmoryAvailable()
        {
            try
            {
                // Check if BDArmory assembly is loaded
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    if (assembly.FullName.StartsWith("BahaTurret"))
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private string GetLocalPlayerName()
        {
            try
            {
                // Try to get player name from DMP
                var settingsType = Type.GetType("DarkMultiPlayer.Settings, DarkMultiPlayer");
                if (settingsType != null)
                {
                    var playerNameField = settingsType.GetField("playerName");
                    if (playerNameField != null)
                    {
                        var name = playerNameField.GetValue(null) as string;
                        if (!string.IsNullOrEmpty(name))
                            return name;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[BDArmoryDMPSync] Could not get DMP player name: {ex.Message}");
            }

            return "Unknown";
        }

        #endregion

        void OnDestroy()
        {
            Debug.Log("[BDArmoryDMPSync] Shutting down");
            instance = null;
        }
    }
}
