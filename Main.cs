using BepInEx;
using BepInEx.IL2CPP;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

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

	internal static class LoverConfig
	{
		internal const string SectionName = "Lover";
		internal static ConfigEntry<int> MinLoveStoryProgress;
		internal static ConfigEntry<int> MinLoveLevel;
		internal static ConfigEntry<bool> UnlimitedLoveEvent;
		internal static ConfigEntry<bool> EventCheck;
		internal static ConfigEntry<bool> DateEventCheck;
		internal static ConfigEntry<bool> DontRefused;
	}

	internal static class MarriageConfig
	{
		internal const string SectionName = "Marriage";
		internal static ConfigEntry<int> MinLoveStoryProgress;
		internal static ConfigEntry<int> MinLoveLevel;
		internal static ConfigEntry<bool> NeedRing;
		internal static ConfigEntry<bool> NeedDoubleBed;
		internal static ConfigEntry<bool> EventCheck;
	}

	internal static class SpousesConfig
	{
		internal const string SectionName = "Spouses";
		internal static ConfigEntry<int> HomeBGM;
		internal static ConfigEntry<int> SaveLogo;
		internal static ConfigEntry<int> Bedmate;
		internal static ConfigEntry<bool> Cohabitation;
		internal static ConfigEntry<bool> Alternation;
		internal static ConfigEntry<bool> ForceBedmate;
		internal static ConfigEntry<bool> UnrelatedNPCDialogue;
		internal static ConfigEntry<bool> ChildBed;
	}

	static public new ManualLogSource Log;

	public override void Load()
	{
		Log = base.Log;

		LoverConfig.MinLoveStoryProgress = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.MinLoveStoryProgress), 4, "The progress of the romantic story required for confession.");
		LoverConfig.MinLoveLevel = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.MinLoveLevel), 4, "The relationship required for confession.");
		LoverConfig.UnlimitedLoveEvent = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.UnlimitedLoveEvent), true, "Developing romantic relationships with other NPCs is still allowed after marriage.");
		LoverConfig.EventCheck = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.EventCheck), true, "Confessions need to be made when no event has occurred.");
		LoverConfig.DateEventCheck = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.DateEventCheck), true, "Dating needs to happen when there is no event.");
		LoverConfig.DontRefused = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.DontRefused), true, "Failure to confess will not be refused.");

		MarriageConfig.MinLoveStoryProgress = Config.Bind(MarriageConfig.SectionName, nameof(MarriageConfig.MinLoveStoryProgress), 9, "Progress of the romantic story required for the proposal.");
		MarriageConfig.MinLoveLevel = Config.Bind(MarriageConfig.SectionName, nameof(MarriageConfig.MinLoveLevel), 10, "The relationship required for the proposal.");
		MarriageConfig.NeedRing = Config.Bind(MarriageConfig.SectionName, nameof(MarriageConfig.NeedRing), true, "Need a ring to propose.");
		MarriageConfig.NeedDoubleBed = Config.Bind(MarriageConfig.SectionName, nameof(MarriageConfig.NeedDoubleBed), true, "Need to have a double bed to propose.");
		MarriageConfig.EventCheck = Config.Bind(MarriageConfig.SectionName, nameof(MarriageConfig.EventCheck), true, "The proposal requires a certain level of main mission.");

		SpousesConfig.HomeBGM = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.HomeBGM), 1, $"Changing the background music at home after marriage.{Environment.NewLine}#1=Change after marriage.2=original.3=after marriage");
		SpousesConfig.SaveLogo = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.SaveLogo), 1, $"Progress of the romantic story required for the proposal.{Environment.NewLine}#0=none.1=random.other=NPCID");
		SpousesConfig.Bedmate = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.Bedmate), 1, $"Progress of the romantic story required for the proposal.{Environment.NewLine}#0=none.1=random.other=NPCID");
		SpousesConfig.Cohabitation = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.Cohabitation), false, "Everyone sleeps together.");
		SpousesConfig.Alternation = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.Alternation), true, "Use rotational switching instead of random selection.");
		SpousesConfig.ForceBedmate = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.ForceBedmate), false, "Forced simultaneous sleep.");
		SpousesConfig.UnrelatedNPCDialogue = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.UnrelatedNPCDialogue), true, "Allow some dialogue from unrelated NPCs (maybe there is), maybe there are some bugs.");
		SpousesConfig.ChildBed = Config.Bind(SpousesConfig.SectionName, nameof(SpousesConfig.ChildBed), false, "Automatic access to children's beds.");

		new Harmony(GUID).PatchAll();
	}
}
