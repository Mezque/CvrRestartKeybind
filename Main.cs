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
    [BepInPlugin("com.mezque.cvr.plugin.CvrRestartKeybind", $"{AsInfo.Title}", $"{AsInfo.Version}")]
    [BepInProcess("ChilloutVR.exe")]

    internal class MainBiE : BaseUnityPlugin
    {
        internal void Awake()

        {
            Logger.LogMessage($"{AsInfo.Title}: V{AsInfo.Version} Started");
            Logger.LogMessage("Checking For Updates Now.");
            UpdateNotice.UpdateCheck();
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
            UpdateNotice.UpdateCheck();

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
internal class UpdateNotice
{
    internal static void UpdateCheck()
    {
        using var sha = SHA256.Create();
#if MELONLOADER
        var gitURL = "https://github.com/Mezque/CvrRestartKeybind/releases/latest/download/MlCvrRestartKeybind.dll";
        var ModDLL = "Mods//MLMicDotRecolour.dll";
#endif

#if BEPINEX
        var gitURL = "https://github.com/Mezque/CvrRestartKeybind/releases/latest/download/BiECvrRestartKeybind.dll";
        var ModDLL = "BepInEx//plugins//MLMicDotRecolour.dll";
#endif
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
#if MELONLOADER
            MlLogger.Msg(ConsoleColor.DarkMagenta,$"Unable to check for mod update. \n{ex}");
#endif
#if BEPINEX
            Console.WriteLine($"Unable to check for mod update. \n{ex}");
#endif
        }
        try
        {
            string CurModHash = ComputeHash(sha, DllCur);
            string UpdateModHash = ComputeHash(sha, DLLupdate);

            if (CurModHash != UpdateModHash)
            {
#if MELONLOADER
                MlLogger.Msg(ConsoleColor.DarkMagenta, $"There Is A Mod Update Available At:\n {gitURL}\n Certan Features May NOT Work Until You Update!");
#endif
#if BEPINEX
                Console.Write($"There Is A Mod Update Available At:\n {gitURL}\n Certan Features May NOT Work Until You Update!");
#endif
            }
            else
            {
#if MELONLOADER
                MlLogger.Msg(ConsoleColor.DarkMagenta, "[INFO] No Updates Available. :)");
#endif
#if BEPINEX
                Console.Write("[INFO] No Updates Available. :)");
#endif
            }
        }
        catch (Exception ex)
        {
#if MELONLOADER
            MlLogger.Msg(ConsoleColor.DarkMagenta, $"Failed To Check For Updates:\n{ex}");
#endif
#if BEPINEX
            Console.Write($"Failed To Check For Updates:\n{ex}");
#endif
        }
    }
    private static string ComputeHash(HashAlgorithm sha256, byte[] data)
    {
        byte[] array = sha256.ComputeHash(data);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in array)
        {
            stringBuilder.Append(b.ToString("x2"));
        }
        return stringBuilder.ToString();
    }
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
    internal const string Version = "1.0.0.0";
    internal const string DevBuild = "1";
    internal const string Dev = "Mezque";
    internal const string Title = "Cvr Restart Keybinds";
}