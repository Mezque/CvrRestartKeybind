using System;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
#if MELONLOADER
using MelonLoader;
[assembly: MelonInfo(typeof(CvrRestartKeybind.MainML), $"{AsInfo.Title}", $"{AsInfo.Version}", $"{AsInfo.Dev}")]
[assembly: MelonColor(System.ConsoleColor.DarkMagenta)]
[assembly: MelonAuthorColor(System.ConsoleColor.Magenta)]
#endif

namespace CvrRestartKeybind
{
#if BEPINEX
    using BepInEx;
    [BepInPlugin("com.mezque.cvr.plugin.CvrRestartKeybind", $"{AsInfo.Title}", $"{AsInfo.Version}")]
    [BepInProcess("ChilloutVR.exe")]

    internal class MainBiE : BaseUnityPlugin
    {
        internal void Awake()
        {
            Logger.LogMessage($"{AsInfo.Title}: V{AsInfo.Version} Started");
        }
        internal void Update()
        {
            keybinds.Keys();
        }
    }
}
#endif

#if MELONLOADER
    internal class MainML : MelonMod
    {
        public override void OnApplicationStart()
        {
            LoggerInstance.Msg($"{AsInfo.Title}: V{AsInfo.Version} Started");

        }
        public override void OnUpdate()
        {
            keybinds.Keys();
        }

        internal class MLprefs
        {
            internal static readonly MelonPreferences_Category CvrRB = MelonPreferences.CreateCategory("CvrRestartBinds");
            internal static MelonPreferences_Entry<string> PreviousInstance = (MelonPreferences_Entry<string>)CvrRB.CreateEntry("PreviousInstance", "null", "PreviousInstance", true);
        }
    }
}
#endif

internal class keybinds
{
    internal static void Keys()
    {
        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetKeyDown(KeyCode.Insert)))
        {
            Restarts.RestartGame();
        }
    }
}
internal class Restarts
{
    internal static void RestartGame()
    {
#if MELONLOADER

#endif

#if BEPINEX
#endif
        Process.Start("steam://rungameid/661130"); //way better lol thanks kirai, though need to figure out if there is a flag to launch VR mode. there is need to add it after.
        Process.GetCurrentProcess().Kill();
    }
}
internal struct AsInfo
{
    internal const string Version = "1.0.0.0";
    internal const string DevBuild = "1";
    internal const string Dev = "Mezque";
    internal const string Title = "Cvr Restart Keybinds";
}