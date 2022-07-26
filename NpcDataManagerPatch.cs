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
		static public bool hideLover = false;
		static public bool internalState = false;
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcId))]
	public class NpcDataManagerGetSpouseId
	{
		static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if(NpcDataManagerPatch.hideLover)
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
			if (NpcDataManagerPatch.hideLover)
				__result = false;
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
			if (NpcDataManagerPatch.hideLover)
			{
				__result = null;
				return false;
			}

			if(NpcDataManagerPatch.forceNPCID < 2)
				return true;

			__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID);
			if (__result != null && !__result.IsSpouses)
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

			return true;
		}
	}

	[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetLoverNum))]
	public class NpcDataManagerGetLoverNum
	{
		static bool Prefix(ref int __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = 0;
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
			if (NpcDataManagerPatch.hideLover)
			{
				__result = false;
				return false;
			}

			__result = __instance.GetNpcData(npcid)?.IsSpouses == true;
			return false;
		}
	}
}
