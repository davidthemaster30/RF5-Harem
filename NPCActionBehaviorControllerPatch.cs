using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(NPCActionBehaviorController), nameof(NPCActionBehaviorController.GetDataList))]
public class NPCActionBehaviorControllerPatch
{
	static void Prefix(NPCActionBehaviorController __instance)
	{
		if (__instance.NPCOwner != null && __instance.NPCOwner.NPCData != null)
		{
			if (__instance.NPCOwner.NPCData.NpcId >= 2)
			{
				Relation.SetNPC(__instance.NPCOwner.NPCData.NpcId);
			}

			Main.Log.LogDebug($"NPCActionBehaviorController.GetDataList npcid:{__instance.NPCOwner.NPCData.NpcId}");
		}
	}
}