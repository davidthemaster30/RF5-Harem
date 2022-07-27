using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 相遇时的打招呼
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
					Main.Log.LogDebug(string.Format("NpcSearchCollider.OnTriggerEnter npcid:{0}", __instance.owner.NpcId));
				}
			}
		}
	}
}
