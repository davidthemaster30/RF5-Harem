using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnhollowerBaseLib;

namespace RF5_Harem
{
	// 防止已婚告白被拒
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.RunScript))]
	public class EventControllerBaseScript
	{
		static void Prefix(EventControllerBase __instance)
		{
			Relation.SetNPC(__instance.TargetNpcId);
			Main.Log.LogDebug(string.Format("EventControllerBase.RunScript npcid:{0}", __instance.TargetNpcId));
		}
	}
}
