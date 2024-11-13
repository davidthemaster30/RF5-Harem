using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using RF5_Harem.Configuration;

#if (NETSTANDARD2_1)
using BepInEx.IL2CPP;
#endif

#if (NET6_0)
using BepInEx.Unity.IL2CPP;
#endif

namespace RF5_Harem;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInProcess(GAME_PROCESS)]
public class Main : BasePlugin
{
	#region PluginInfo
	private const string GUID = "RF5_Harem";
	private const string NAME = "RF5_Harem";
	private const string VERSION = "1.2.0";
	private const string GAME_PROCESS = "Rune Factory 5.exe";
	#endregion

	internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("RF5Harem");

	public override void Load()
	{
		// Plugin startup logic
        Log.LogInfo($"Plugin {NAME} is loaded!");

        // Config
		LoverConfig.Load(Config);
		MarriageConfig.Load(Config);
		SpousesConfig.Load(Config);

		var assembly = Assembly.GetExecutingAssembly();
		Harmony.CreateAndPatchAll(assembly);
	}
}
