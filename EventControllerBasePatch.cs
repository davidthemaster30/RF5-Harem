using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class EventControllerBasePatch
{
	[HarmonyPatch]
	internal static class EventControllerBaseScript
	{
		[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.RunScript))]
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
	[HarmonyPatch]
	internal static class EventControllerBaseTalkType
	{
		[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.GetNpcTalkType))]
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
	[HarmonyPatch]

	internal static class EventControllerBaseSelectedMenu
	{
		[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.SelectedMenuGroup))]
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
	[HarmonyPatch]
	internal static class EventControllerBaseMarriageInit
	{
		[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.MarriageInit))]
		internal static bool Prefix()
		{
			return false;
		}
	}
}
