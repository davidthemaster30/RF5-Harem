using BepInEx.Configuration;

namespace RF5_Harem.Configuration;

internal static class LoverConfig
{
	internal const string SectionName = "Lover";
	private const int DefaultMinLove = 4;
	private static readonly AcceptableValueRange<int> RelationshopProgressRange = new AcceptableValueRange<int>(0, 99);
	internal static ConfigEntry<int> MinLoveStoryProgress;
	private static readonly ConfigDescription MinLoveStoryProgressDescription = new ConfigDescription("The progress of the romantic story required for confession.", RelationshopProgressRange);
	internal static ConfigEntry<int> MinLoveLevel;
	private static readonly ConfigDescription MinLoveLevelDescription = new ConfigDescription("The relationship required for confession.", RelationshopProgressRange);
	internal static ConfigEntry<bool> UnlimitedLoveEvent;
	internal static ConfigEntry<bool> EventCheck;
	internal static ConfigEntry<bool> DateEventCheck;
	internal static ConfigEntry<bool> CantRefuse;

	internal static void Load(ConfigFile Config)
	{
		MinLoveStoryProgress = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.MinLoveStoryProgress), DefaultMinLove, MinLoveStoryProgressDescription);
		MinLoveLevel = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.MinLoveLevel), DefaultMinLove, MinLoveLevelDescription);
		UnlimitedLoveEvent = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.UnlimitedLoveEvent), true, "Developing romantic relationships with other NPCs is still allowed after marriage.");
		EventCheck = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.EventCheck), true, "Confessions need to be made when no event has occurred.");
		DateEventCheck = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.DateEventCheck), true, "Dating needs to happen when there is no event.");
		CantRefuse = Config.Bind(LoverConfig.SectionName, nameof(LoverConfig.CantRefuse), true, "Failure to confess will not be refused.");
	}
}
