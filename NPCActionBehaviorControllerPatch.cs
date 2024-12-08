using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch]
internal static class NPCActionBehaviorControllerPatch
{
	[HarmonyPatch(typeof(NPCActionBehaviorController), nameof(NPCActionBehaviorController.GetDataList))]
	internal static void Prefix(NPCActionBehaviorController __instance)
	{
		if (__instance.NPCOwner is not null && __instance.NPCOwner.NPCData is not null)
		{
			if (__instance.NPCOwner.NPCData.NpcId >= NpcDataManagerPatch.MinNPCId)
			{
				Relation.SetNPC(__instance.NPCOwner.NPCData.NpcId);
			}

			HaremPlugin.Log.LogDebug($"NPCActionBehaviorController.GetDataList npcid:{__instance.NPCOwner.NPCData.NpcId}");
		}
	}
}