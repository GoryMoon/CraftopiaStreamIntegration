using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine.SceneManagement;
// ReSharper disable InconsistentNaming

namespace CraftopiaStreamIntegration
{
    [BepInPlugin("se.gory_moon.plugins.csi_plugin", "Stream Integration", "1.0.0.0")]
    [BepInProcess("Craftopia.exe")]
    public class CSIPlugin: BaseUnityPlugin
    {

        public ManualLogSource Log => Logger;
        private IntegrationManager _integrationManager;
        //private bool _setup;
        
        public ActionManager ActionManager { get; private set; }

        public static CSIPlugin Instance { get; private set; }

        public CSIPlugin()
        {
            Instance = this;
            
            var harmony = new Harmony(Info.Metadata.GUID);
            harmony.PatchAll();
            
            AccessUtils.Init();
        }

        private void Awake()
        {
            Logger.LogDebug("Started plugin");

            _integrationManager = new IntegrationManager(this);
            ActionManager = new ActionManager(Logger);

            SceneManager.sceneLoaded += (scene, mode) =>
            {
                Logger.LogDebug($"Loaded scene: {scene.name}");
            };

            /*var saveDatas = orig(self);
            foreach (var saveData in saveDatas)
            {
                var em = self.getEmFromGuid(saveData.GUID);
                if (em.HasCustomName())
                {
                    
                }
            }*/
        }

        /*private void Update()
        {
            if (!Utils.InGame)
            {
                _setup = false;
                return;
            }

            if (_setup) return;
            _setup = true;
            
            Logger.LogInfo("Setting up hooks!");
            SingletonMonoBehaviour<OcSaveManager>.Inst.SaveEvent += autoSave =>
            {
                Logger.LogDebug("Saving!");
            };
        }*/

        private void Start()
        {
            _integrationManager.Start();
            InvokeRepeating(nameof(UpdateIntegration), 0, 0.1f);
        }

        private void UpdateIntegration()
        {
            if (!Utils.InGame) return;
            _integrationManager.Update();
            ActionManager.Update();
        }

        private void OnDestroy() => _integrationManager.Close();
        private void OnApplicationQuit() => OnDestroy();
    }
}