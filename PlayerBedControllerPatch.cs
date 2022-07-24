using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 随机选一个陪睡
	[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.DoInteraction), typeof(HumanController))]
	public class PlayerBedControllerPatch
	{
		static void Prefix()
		{
			NpcDataManagerPatch.forceNPCID = NpcDataManagerPatch.RandomSpouses();
		}

		static void Postfix()
		{
			NpcDataManagerPatch.forceNPCID = Define.NPCID.None;
		}
	}
}
