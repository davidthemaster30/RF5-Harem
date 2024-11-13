using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch]
internal static class NpcDataManagerPatch
{
	internal const int MinNPCId = 2;
	internal const int MaxNPCId = 14;
	internal static int forceNPCID = 0;
	internal static bool ForcedNPCisNotPlayer => forceNPCID >= MinNPCId;
	internal static bool hideSpouse = false;
	internal static bool hideLover = false;

	[HarmonyPatch]
	internal static class NpcDataManagerGetSpouseId
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcId))]
		internal static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
			{
				__result = 0;
				return false;
			}

			if (!NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				return true;
			}

			__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID)?.IsSpouses == true ? NpcDataManagerPatch.forceNPCID : 0;

			return false;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerIsSpouse
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsSpouseNpc))]
		internal static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
			{
				__result = false;
			}
			else if (NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsSpouses == true;
			}
			else
			{
				__result = __instance.GetNpcData(npcid)?.IsSpouses == true;
			}

			return false;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerSpouseData
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetSpouseNpcData))]
		internal static bool Prefix(NpcDataManager __instance, ref NpcData __result)
		{
			if (NpcDataManagerPatch.hideSpouse)
			{
				__result = null;
				return false;
			}

			if (!NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				return true;
			}

			__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID);
			if (__result?.IsSpouses != true)
			{
				__result = null;
			}

			return false;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerIsLover
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsLover))]
		internal static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = false;
			}
			else if (NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsLover == true;
			}
			else
			{
				__result = __instance.GetNpcData(npcid)?.IsLover == true;
			}

			return false;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerIsExistLover
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.IsExistLover))]
		internal static bool Prefix(NpcDataManager __instance, int npcid, ref bool __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = false;
				return false;
			}

			if (NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				__result = NpcDataManagerPatch.forceNPCID == npcid && __instance.GetNpcData(npcid)?.IsLover == true;
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerGetLoverNum
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.GetLoverNum))]
		internal static bool Prefix(NpcDataManager __instance, ref int __result)
		{
			if (NpcDataManagerPatch.hideLover)
			{
				__result = 0;
				return false;
			}

			if (NpcDataManagerPatch.ForcedNPCisNotPlayer)
			{
				__result = __instance.GetNpcData(NpcDataManagerPatch.forceNPCID)?.IsLover == true ? 1 : 0;
				return false;
			}

			return true;
		}
	}

	[HarmonyPatch]
	internal static class NpcDataManagerMarriage
	{
		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.DoMarriage))]
		internal static void Prefix(NpcDataManager __instance, int npcId, ref bool[] __state)
		{
			__state = new bool[__instance.NpcDatas.Count];
			for (int i = 0; i < __instance.NpcDatas.Count; ++i)
			{
				__state[i] = __instance.NpcDatas[i].IsSpouses;
			}

			Relation.SetNPC(npcId);
		}

		[HarmonyPatch(typeof(NpcDataManager), nameof(NpcDataManager.DoMarriage))]
		internal static void Postfix(NpcDataManager __instance, int npcId, bool[] __state)
		{
			for (int i = 0; i < __state.Length; ++i)
			{
				if (__instance.NpcDatas[i].NpcId != npcId)
				{
					__instance.NpcDatas[i].IsSpouses = __state[i];
				}
			}
		}
	}
}
