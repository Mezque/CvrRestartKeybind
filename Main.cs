using System;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Net;
using System.Text;
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
    using BepInEx.Logging;

    [BepInPlugin("com.mezque.cvr.plugin.CvrRestartKeybind", $"{AsInfo.Title}", $"{AsInfo.Version}")]
    [BepInProcess("ChilloutVR.exe")]

    internal class MainBiE : BaseUnityPlugin
    {
        internal void Awake()

        {
            Logger.LogMessage($"{AsInfo.Title}: V{AsInfo.Version} Started");
            Logger.LogMessage("Checking For Updates Now.");
            UpdateNotice.UpdateCheckBiE();
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
            LoggerInstance.Msg(ConsoleColor.DarkMagenta,$"{AsInfo.Title}: V{AsInfo.Version} Started");
            LoggerInstance.Msg(ConsoleColor.DarkMagenta,"Checking For Updates Now.");
            UpdateNotice.UpdateCheckML();

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
        Process.Start("steam://rungameid/661130");
        Process.GetCurrentProcess().Kill();
    }
}
internal class UpdateNotice
{
#if BEPINEX
    internal static void UpdateCheckBiE()
    {
        using var sha = SHA256.Create();

        var gitURL = "https://github.com/Mezque/CvrRestartKeybind/releases/latest/download/BiECvrRestartKeybind.dll";
        var ModDLL = $"{AppDomain.CurrentDomain.BaseDirectory}\\BepInEx\\plugins\\BiEMicDotRecolour.dll";
        byte[] DllCur = null;
        byte[] DLLupdate = null;
        using var wc = new WebClient();

        if (File.Exists(ModDLL))
        {
            DllCur = File.ReadAllBytes(ModDLL);
        }
        try
        {
            DLLupdate = wc.DownloadData($"{gitURL}");
        }
        catch (WebException ex)
        {
            Console.Write($"Unable to check for mod update. \n{ex}");
        }
        try
        {
            byte[] CurModHash = DllCur;
            byte[] UpdateModHash = DLLupdate;

            if (CurModHash != UpdateModHash)
            {
                Console.Write($"There Is A Mod Update Available At:\n {gitURL}\n Certan Features May NOT Work Until You Update!");
            }
            else
            {
                Console.Write("[INFO] No Updates Available. :)");
            }
        }
        catch (Exception ex)
        {
            Console.Write($"Failed To Check For Updates:\n{ex}");
        }
    }
#endif
#if MELONLOADER
    internal static void UpdateCheckML()
    {
        using var sha = SHA256.Create();

        var gitURL = "https://github.com/Mezque/CvrRestartKeybind/releases/latest/download/MlCvrRestartKeybind.dll";
        var ModDLL = $"{AppDomain.CurrentDomain.BaseDirectory}\\Mods\\MLMicDotRecolour.dll";
        byte[] DllCur = null;
        byte[] DLLupdate = null;
        using var wc = new WebClient();

        if (File.Exists(ModDLL))
        {
            DllCur = File.ReadAllBytes(ModDLL);
        }
        try
        {
            DLLupdate = wc.DownloadData($"{gitURL}");
        }
        catch (WebException ex)
        {
            MlLogger.Msg(ConsoleColor.DarkMagenta, $"There Is A Mod Update Available At:\n {gitURL}\n Certan Features May NOT Work Until You Update!");
        }
        try
        {
            byte[] CurModHash = DllCur;
            byte[] UpdateModHash = DLLupdate;

            if (CurModHash != UpdateModHash)
            {
                MlLogger.Msg(ConsoleColor.DarkMagenta, $"There Is A Mod Update Available At:\n {gitURL}\n Certan Features May NOT Work Until You Update!");
            }
            else
            {
                MlLogger.Msg(ConsoleColor.DarkMagenta, "[INFO] No Updates Available. :)");
            }
        }
        catch (Exception ex)
        {
            MlLogger.Msg(ConsoleColor.DarkMagenta, $"Failed To Check For Updates:\n{ex}");
        }
    }
#endif
}
#if MELONLOADER
internal class MlLogger
{
    internal static MelonLogger.Instance Conlog = new($"{AsInfo.Title}", ConsoleColor.White);
    internal static void Msg(ConsoleColor ConColour, string obj) => Conlog.Msg(ConColour, obj);
    internal static void Error(string obj) => Conlog.Error(obj);
    internal static void Warning(string obj) => Conlog.Warning(obj);
}
#endif
internal struct AsInfo
{
    internal const string Version = "1.1.0.1";
    internal const string DevBuild = "0";
    internal const string Dev = "Mezque";
    internal const string Title = "Cvr Restart Keybinds";
}