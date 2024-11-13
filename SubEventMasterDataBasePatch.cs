using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class SubEventMasterDataBasePatch
{
	[HarmonyPatch(typeof(SubEventMasterDataBase), nameof(SubEventMasterDataBase.CheckOccurrenceLoveProgress))]
	internal static bool Prefix(ref bool __result)
	{
		__result = LoverConfig.UnlimitedLoveEvent.Value;
		return false;
	}
}
