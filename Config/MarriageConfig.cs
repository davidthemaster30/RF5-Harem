using BepInEx.Configuration;

namespace RF5_Harem.Configuration;


internal static class MarriageConfig
{
	internal const string SectionName = "Marriage";
	private static readonly AcceptableValueRange<int> RelationshopProgressRange = new AcceptableValueRange<int>(0, 10);
	private const int DefaultMinLoveProgress = 9;
	private const int DefaultMinLoveProposal = 10;
	internal static ConfigEntry<int> MinLoveStoryProgress;
	private static readonly ConfigDescription MinLoveStoryProgressDescription = new ConfigDescription("Progress of the romantic story required for the proposal.", RelationshopProgressRange);
	internal static ConfigEntry<int> MinLoveLevel;
	private static readonly ConfigDescription MinLoveLevelDescription = new ConfigDescription("The relationship required for the proposal.", RelationshopProgressRange);
	internal static ConfigEntry<bool> NeedRing;
	internal static ConfigEntry<bool> NeedDoubleBed;
	internal static ConfigEntry<bool> EventCheck;

		internal static void Load(ConfigFile Config)
	{
		MinLoveStoryProgress = Config.Bind(SectionName, nameof(MinLoveStoryProgress), DefaultMinLoveProgress, MinLoveStoryProgressDescription);
		MinLoveLevel = Config.Bind(SectionName, nameof(MinLoveLevel), DefaultMinLoveProposal, MinLoveLevelDescription);
		NeedRing = Config.Bind(SectionName, nameof(NeedRing), true, "Need a ring to propose.");
		NeedDoubleBed = Config.Bind(SectionName, nameof(NeedDoubleBed), true, "Need to have a double bed to propose.");
		EventCheck = Config.Bind(SectionName, nameof(EventCheck), true, "The proposal requires a certain level of main mission.");
	}
}
