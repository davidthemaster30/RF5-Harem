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

			Main.Log.LogDebug(string.Format("NPCActionBehaviorController.GetDataList npcid:{0}", __instance.NPCOwner.NPCData.NpcId));
		}
	}

	/*
	static void Postfix()
	{
		Main.SetNotLover();
	}
	*/
}