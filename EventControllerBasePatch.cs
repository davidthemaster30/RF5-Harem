using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 不确定是否需要
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.GetNpcTalkTypeBefore))]
	public class EventControllerBasePatch
	{
		static void Prefix(EventControllerBase __instance)
		{
			int npcid = __instance.TargetNpcId;
			if (npcid <= 0 &&
				SaveData.SaveDataManager.GameSaveData != null &&
				SaveData.SaveDataManager.GameSaveData.EventData != null &&
				SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter != null)
				npcid = SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId;

			if (npcid > 0)
				NpcDataManagerPatch.forceNPCID = (Define.NPCID)npcid;
		}

		static void Postfix()
		{
			NpcDataManagerPatch.forceNPCID = Define.NPCID.None;
		}
	}

	// 对话相关
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.Update))]
	public class EventControllerBasePatch2
	{
		static void Prefix(EventControllerBase __instance)
		{
			NpcDataManagerPatch.forceNPCID = (Define.NPCID)__instance.TargetNpcId;
		}

		static void Postfix()
		{
			NpcDataManagerPatch.forceNPCID = Define.NPCID.None;
		}
	}
}
