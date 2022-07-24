﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 角色行为
	[HarmonyPatch(typeof(NPCActionBehaviorController), nameof(NPCActionBehaviorController.GetDataList))]
	public class NPCActionBehaviorControllerPatch
	{
		static void Prefix(NPCActionBehaviorController __instance)
		{
			if (__instance.NPCOwner != null && __instance.NPCOwner.NPCData != null)
				NpcDataManagerPatch.forceNPCID = (Define.NPCID)__instance.NPCOwner.NPCData.NpcId;
		}

		static void Postfix()
		{
			NpcDataManagerPatch.forceNPCID = Define.NPCID.None;
		}
	}
}
