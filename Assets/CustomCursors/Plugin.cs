using System.Reflection;
using HarmonyLib;
using TimberApi.ConsoleSystem;
using TimberApi.DependencyContainerSystem;
using TimberApi.ModSystem;
using UnityEngine.UIElements;

namespace CustomCursors
{
    public class Plugin : IModEntrypoint
    {
        public const string PluginGuid = "tobbert.customcursors";
        public const string PluginName = "Custom Cursors";
        public const string PluginVersion = "1.0.0";

        public static string myPath;

        public void Entry(IMod mod, IConsoleWriter consoleWriter)
        {
            myPath = mod.DirectoryPath;

            new Harmony(PluginGuid).PatchAll();
        }
    }


    [HarmonyPatch]
    public class StartGrabbingPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GrabbingCameraTargetPicker"), "StartGrabbing");
        }
        
        static void Postfix()
        {
            DependencyContainer.GetInstance<CustomCursorsService>().StartGrabbing();
        }
    }
    
    [HarmonyPatch]
    public class StopGrabbingPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GrabbingCameraTargetPicker"), "StopGrabbing");
        }
        
        static void Postfix()
        {
            DependencyContainer.GetInstance<CustomCursorsService>().StopGrabbing();
        }
    }
    
    [HarmonyPatch]
    public class SettingsPatch
    {
        static MethodInfo TargetMethod()
        {
            return AccessTools.Method(AccessTools.TypeByName("GameSavingSettingsController"), "InitializeAutoSavingOnToggle", new []
            {
                typeof(VisualElement)
            });
        }
        
        static void Postfix(ref VisualElement root)
        {
            DependencyContainer.GetInstance<CustomCursorsService>().InitializeSelectorSettings(ref root);
        }
    }
}