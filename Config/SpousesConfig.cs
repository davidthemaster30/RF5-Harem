using BepInEx.Configuration;

namespace RF5_Harem.Configuration;

internal static class SpousesConfig
{
	internal const string SectionName = "Spouses";
	private static readonly AcceptableValueRange<int> CandidatesNPCIds = new AcceptableValueRange<int>(0, NpcDataManagerPatch.MaxNPCId);
	private static readonly AcceptableValueRange<int> HomeBGMValues = new AcceptableValueRange<int>(1, 3);
	internal static ConfigEntry<int> HomeBGM;
	private static readonly ConfigDescription HomeBGMDescription = new ConfigDescription("""
	Changing the background music at home after marriage.
	1=Change after marriage, 2=original, 3=after marriage
	""", HomeBGMValues);

	internal static ConfigEntry<int> SaveLogo;
	private static readonly ConfigDescription SaveLogoDescription = new ConfigDescription("""
	Showing a two-person Logo when saving.
	0=none, 1=random, other=NPCID
	""", CandidatesNPCIds);

	internal static ConfigEntry<int> Bedmate;
	private static readonly ConfigDescription BedmateDescription = new ConfigDescription("""
	Choose a partner at bedtime.
	0=none, 1=random, other=NPCID
	""", CandidatesNPCIds);
	internal static ConfigEntry<bool> Cohabitation;
	internal static ConfigEntry<bool> Alternation;
	internal static ConfigEntry<bool> ForceBedmate;
	internal static ConfigEntry<bool> UnrelatedNPCDialogue;
	internal static ConfigEntry<bool> ChildBed;

	internal static void Load(ConfigFile Config)
	{
		HomeBGM = Config.Bind(SectionName, nameof(HomeBGM), 1, HomeBGMDescription);
		SaveLogo = Config.Bind(SectionName, nameof(SaveLogo), 1, SaveLogoDescription);
		Bedmate = Config.Bind(SectionName, nameof(Bedmate), 1, BedmateDescription);
		Cohabitation = Config.Bind(SectionName, nameof(Cohabitation), false, "Everyone sleeps together.");
		Alternation = Config.Bind(SectionName, nameof(Alternation), true, "Use rotational switching instead of random selection.");
		ForceBedmate = Config.Bind(SectionName, nameof(ForceBedmate), false, "Forced simultaneous sleep.");
		UnrelatedNPCDialogue = Config.Bind(SectionName, nameof(UnrelatedNPCDialogue), true, "Allow some dialogue from unrelated NPCs (maybe there is), maybe there are some bugs.");
		ChildBed = Config.Bind(SectionName, nameof(ChildBed), false, "Automatic access to children's beds.");
	}
}
