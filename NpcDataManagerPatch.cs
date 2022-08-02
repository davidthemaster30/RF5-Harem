using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	public class NpcDataManagerPatch
	{
		static public int forceNPCID = 0;
		static public bool hideSpouse = false;
		static public bool hideLover = false;
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcId))]
	public class NpcDataManagerGetSpouseId
	{
		static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if(NpcDataManagerPatch.hideSpouse)
			{
				__result = 0;
				return false;
			}
			
			if(NpcDataManagerPatch.forceNPCID < 2)
				return true;

			if (__instance.GetNpcData(NpcDataManagerPatch.forceNPCID)?.IsSpouses == true)
				__result = NpcDataManagerPatch.forceNPCID;
			else
				__result = 0;

			return false;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsSpouseNpc))]
	public class NpcDataManagerIsSpouse
	{
		static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
				__result = false;
			else if(NpcDataManagerPatch.forceNPCID >= 2)
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsSpouses == true;
			else
				__result = __instance.GetNpcData(npcid)?.IsSpouses == true;

			return false;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcData))]
	public class NpcDataManagerSpouseData
	{
		static bool Prefix(NpcDataManager __instance, ref NpcData __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
			{
				__result = null;
				return false;
			}

			if(NpcDataManagerPatch.forceNPCID < 2)
				return true;

			__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID);
			if (__result?.IsSpouses != true)
				__result = null;

			return false;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsLover))]
	public class NpcDataManagerIsLover
	{
		static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideLover)
				__result = false;
			else if(NpcDataManagerPatch.forceNPCID >= 2)
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsLover == true;
			else
				__result = __instance.GetNpcData(npcid)?.IsLover == true;

			return false;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsExistLover))]
	public class NpcDataManagerIsExistLover
	{
		static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = false;
				return false;
			}

			if (NpcDataManagerPatch.forceNPCID >= 2)
			{
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsLover == true;
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetLoverNum))]
	public class NpcDataManagerGetLoverNum
	{
		static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = 0;
				return false;
			}

			if (NpcDataManagerPatch.forceNPCID >= 2)
			{
				__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID)?.IsLover == true ? 1 : 0;
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsMoreThanLover))]
	public class NpcDataManagerIsMoreThanLover
	{
		static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
				__result = false;
			else if (NpcDataManagerPatch.forceNPCID >= 2)
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsSpouses == true;
			else
				__result = __instance.GetNpcData(npcid)?.IsSpouses == true;

			return false;
		}
	}

	// 修复其他NPC的关系
	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.DoMarriage))]
	public class NpcDataManagerMarriage
	{
		static void Prefix(NpcDataManager __instance, int npcId, ref bool[] __state)
		{
			__state = new bool[__instance.NpcDatas.Count];
			for(int i = 0; i < __instance.NpcDatas.Count; ++i)
				__state[i] = __instance.NpcDatas[i].IsSpouses;

			Relation.SetNPC(npcId);
		}

		static void Postfix(NpcDataManager __instance, int npcId, bool[] __state)
		{
			for (int i = 0; i < __state.Length; ++i)
				if(__instance.NpcDatas[i].NpcId != npcId)
					__instance.NpcDatas[i].IsSpouses = __state[i];
		}
	}
}
