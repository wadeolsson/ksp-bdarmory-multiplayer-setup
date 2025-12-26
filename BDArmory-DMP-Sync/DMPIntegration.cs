using System;
using System.Reflection;
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
        private object dmpModInterface = null;

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

            // Get DMP interface using reflection
            try
            {
                var dmpType = Type.GetType("DarkMultiPlayer.DMPModInterface, DarkMultiPlayer");
                if (dmpType == null)
                {
                    Debug.LogError("[BDArmoryDMPSync] DMPModInterface type not found");
                    enabled = false;
                    return;
                }

                // Try to get fetch property or instance
                var fetchProp = dmpType.GetProperty("fetch", BindingFlags.Public | BindingFlags.Static);
                if (fetchProp != null)
                {
                    dmpModInterface = fetchProp.GetValue(null);
                }

                if (dmpModInterface == null)
                {
                    Debug.LogError("[BDArmoryDMPSync] Could not get DMPModInterface instance");
                    enabled = false;
                    return;
                }

                // Register handlers using reflection
                RegisterHandler("BDABuilding", HandleBuildingMessage, "RegisterUpdateModHandler");
                RegisterHandler("BDADamage", HandleDamageMessage, "RegisterFixedUpdateModHandler");
                RegisterHandler("BDAWeapon", HandleWeaponMessage, "RegisterUpdateModHandler");
                RegisterHandler("BDAExplosion", HandleExplosionMessage, "RegisterUpdateModHandler");
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

        private void HandleBuildingMessage(byte[] data)
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

        private void HandleDamageMessage(byte[] data)
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

        private void HandleWeaponMessage(byte[] data)
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

        private void HandleExplosionMessage(byte[] data)
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
            SendDMPMessage("BDABuilding", data, true, true);
        }

        public void SendVesselDamage(VesselDamageMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            SendDMPMessage("BDADamage", data, true, true);
        }

        public void SendWeaponFire(WeaponFireMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            SendDMPMessage("BDAWeapon", data, true, false);
        }

        public void SendExplosion(ExplosionMessage message)
        {
            if (!isInitialized) return;

            message.PlayerName = localPlayerName;
            byte[] data = message.Serialize();
            SendDMPMessage("BDAExplosion", data, true, true);
        }

        #endregion

        #region Utility Methods

        private void RegisterHandler(string modName, Action<byte[]> handler, string methodName)
        {
            try
            {
                var method = dmpModInterface.GetType().GetMethod(methodName);
                if (method == null)
                {
                    Debug.LogWarning($"[BDArmoryDMPSync] Method {methodName} not found");
                    return;
                }

                // Create delegate for the handler
                var delegateType = method.GetParameters()[1].ParameterType;
                var del = Delegate.CreateDelegate(delegateType, handler.Target, handler.Method);

                method.Invoke(dmpModInterface, new object[] { modName, del });
                Debug.Log($"[BDArmoryDMPSync] Registered {modName} via {methodName}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Failed to register {modName}: {ex}");
            }
        }

        private void SendDMPMessage(string modName, byte[] data, bool relay, bool highPriority)
        {
            try
            {
                if (dmpModInterface == null) return;

                var method = dmpModInterface.GetType().GetMethod("SendDMPModMessage");
                if (method == null)
                {
                    Debug.LogWarning("[BDArmoryDMPSync] SendDMPModMessage method not found");
                    return;
                }

                method.Invoke(dmpModInterface, new object[] { modName, data, relay, highPriority });
            }
            catch (Exception ex)
            {
                Debug.LogError($"[BDArmoryDMPSync] Failed to send message: {ex}");
            }
        }

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
                    // BDArmory Plus uses "BDArmory", old versions used "BahaTurret"
                    if (assembly.FullName.StartsWith("BDArmory") || assembly.FullName.StartsWith("BahaTurret"))
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
