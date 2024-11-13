using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.RunScript))]
internal static class EventControllerBaseScript
{
	internal static void Prefix(EventControllerBase __instance)
	{
		if (__instance.TargetNpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		if (LoverConfig.CantRefuse.Value && __instance.TargetNpc?.NpcData?.IsRefused == true)
		{
			__instance.TargetNpc.NpcData.IsRefused = false;
		}

		Main.Log.LogDebug($"EventControllerBase.RunScript npcid:{__instance.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.GetNpcTalkType))]
internal static class EventControllerBaseTalkType
{
	internal static void Prefix(EventControllerBase __instance)
	{
		if (SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
		}
		else if (__instance.TargetNpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		Main.Log.LogDebug($"EventControllerBase.GetNpcTalkType npcid:{__instance.TargetNpcId} evnpcid:{SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.SelectedMenuGroup))]
internal static class EventControllerBaseSelectedMenu
{
	internal static void Prefix(EventControllerBase __instance)
	{
		if (SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
		}
		else if (__instance.TargetNpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		Main.Log.LogDebug($"EventControllerBase.SelectedMenuGroup npcid:{__instance.TargetNpcId} evnpcid:{SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.MarriageInit))]
internal static class EventControllerBaseMarriageInit
{
	internal static bool Prefix()
	{
		return false;
	}
}
