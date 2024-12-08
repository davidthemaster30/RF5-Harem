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

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess(GAME_PROCESS)]
public class Main : BasePlugin
{
	private const string GAME_PROCESS = "Rune Factory 5.exe";

	internal static readonly ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource("RF5Harem");

	internal void LoadConfig()
	{
		LoverConfig.Load(Config);
		MarriageConfig.Load(Config);
		SpousesConfig.Load(Config);
	}

	public override void Load()
	{
		Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loading!");

		LoadConfig();
		var assembly = Assembly.GetExecutingAssembly();
		Harmony.CreateAndPatchAll(assembly);

		Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} {MyPluginInfo.PLUGIN_VERSION} is loaded!");
	}
}
