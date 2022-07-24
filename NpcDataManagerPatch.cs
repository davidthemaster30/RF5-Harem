using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcId))]
	public class NpcDataManagerPatch
	{
		// 修复各种事件和对话
		static public Define.NPCID forceNPCID = Define.NPCID.None;

		static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if(forceNPCID <= Define.NPCID.Ares)
				return true;

			if (forceNPCID != Define.NPCID.Max)
			{
				// 固定某个
				if (__instance.GetNpcData(forceNPCID)?.IsSpouses == true)
					__result = (int)forceNPCID;
				else
					__result = 0;
			}

			return false;
		}

		public static Define.NPCID RandomSpouses()
		{
			int top = -1;
			int[] npcs = new int[(int)Define.NPCID.Max];
			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
				if (data.IsSpouses)
					npcs[++top] = data.NpcId;

			// 没有返回0
			if (top <= -1)
				return Define.NPCID.Ares;

			return (Define.NPCID)npcs[UnityEngine.Random.Range(0, top)];
		}
	}

	// 解决只能返回一个人的问题
	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsSpouseNpc))]
	public class NpcDataManagerPatch2
	{
		static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			// 原版是直接比较 npcid == NpcDataManager.GetSpouseNpcId()
			__result = (__instance.GetNpcData(npcid)?.IsSpouses == true);
			return false;
		}
	}
}
