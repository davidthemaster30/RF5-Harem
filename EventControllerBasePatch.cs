using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.RunScript))]
public class EventControllerBaseScript
{
	static void Prefix(EventControllerBase __instance)
	{
		if (__instance.TargetNpcId >= 2)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		if (Main.Config.GetBool("Lover", "DontRefused", false) && __instance.TargetNpc?.NpcData?.IsRefused == true)
		{
			__instance.TargetNpc.NpcData.IsRefused = false;
		}

		Main.Log.LogDebug($"EventControllerBase.RunScript npcid:{__instance.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.GetNpcTalkType))]
public class EventControllerBaseTalkType
{
	static void Prefix(EventControllerBase __instance)
	{
		if (SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= 2)
		{
			Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
		}
		else if (__instance.TargetNpcId >= 2)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		Main.Log.LogDebug($"EventControllerBase.GetNpcTalkType npcid:{__instance.TargetNpcId} evnpcid:{SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.SelectedMenuGroup))]
public class EventControllerBaseSelectedMenu
{
	static void Prefix(EventControllerBase __instance)
	{
		if (SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= 2)
		{
			Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
		}
		else if (__instance.TargetNpcId >= 2)
		{
			Relation.SetNPC(__instance.TargetNpcId);
		}

		Main.Log.LogDebug($"EventControllerBase.SelectedMenuGroup npcid:{__instance.TargetNpcId} evnpcid:{SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId}");
	}
}

[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.MarriageInit))]
public class EventControllerBaseMarriageInit
{
	static bool Prefix()
	{
		return false;
	}
}
