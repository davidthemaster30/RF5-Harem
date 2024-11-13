using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(NPCActionBehaviorController), nameof(NPCActionBehaviorController.GetDataList))]
internal static class NPCActionBehaviorControllerPatch
{
	internal static void Prefix(NPCActionBehaviorController __instance)
	{
		if (__instance.NPCOwner is not null && __instance.NPCOwner.NPCData is not null)
		{
			if (__instance.NPCOwner.NPCData.NpcId >= NpcDataManagerPatch.MinNPCId)
			{
				Relation.SetNPC(__instance.NPCOwner.NPCData.NpcId);
			}

			Main.Log.LogDebug($"NPCActionBehaviorController.GetDataList npcid:{__instance.NPCOwner.NPCData.NpcId}");
		}
	}
}