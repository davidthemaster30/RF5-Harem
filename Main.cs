using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Logging;
using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[BepInPlugin(GUID, NAME, VERSION)]
[BepInProcess(GAME_PROCESS)]
public class Main : BasePlugin
{
	#region PluginInfo
	private const string GUID = "C9A56862-DFB2-8BA7-D503-A8DE4D24068B";
	private const string NAME = "RF5_Harem";
	private const string VERSION = "1.2.0";
	private const string GAME_PROCESS = "Rune Factory 5.exe";
	#endregion

	static public new ManualLogSource Log;

	public override void Load()
	{
		Log = base.Log;

		LoverConfig.Load(Config);
		MarriageConfig.Load(Config);
		SpousesConfig.Load(Config);

		new Harmony(GUID).PatchAll();
	}
}
