using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch]
internal class NpcSearchColliderPatch
{
	[HarmonyPatch(typeof(NpcSearchCollider), nameof(NpcSearchCollider.OnTriggerEnter))]
	internal static void Prefix(NpcSearchCollider __instance)
	{
		if (__instance.owner.NpcId >= 2)
		{
			NpcData data = NpcDataManager.Instance.GetNpcData(__instance.owner.NpcId);
			if (data is not null && data.Initialized && !data.IsShortPlay && !EventControllerBase.Instance.IsTalking)
			{
				Relation.SetNPC(__instance.owner.NpcId);
				HaremPlugin.Log.LogDebug($"NpcSearchCollider.OnTriggerEnter npcid:{__instance.owner.NpcId}");
			}
		}
	}
}