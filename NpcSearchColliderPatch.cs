using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(NpcSearchCollider), nameof(NpcSearchCollider.OnTriggerEnter))]
public class NpcSearchColliderTrigger
{
	static void Prefix(NpcSearchCollider __instance)
	{
		if (__instance.owner.NpcId >= 2)
		{
			NpcData data = NpcDataManager.Instance.GetNpcData(__instance.owner.NpcId);
			if (data != null && data.Initialized && !data.IsShortPlay && !EventControllerBase.Instance.IsTalking)
			{
				Relation.SetNPC(__instance.owner.NpcId);
				Main.Log.LogDebug($"NpcSearchCollider.OnTriggerEnter npcid:{__instance.owner.NpcId}");
			}
		}
	}
}