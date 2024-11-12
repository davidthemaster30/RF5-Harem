using HarmonyLib;

namespace RF5_Harem;
[HarmonyPatch(typeof(SubEventMasterDataBase), nameof(SubEventMasterDataBase.CheckOccurrenceLoveProgress))]
public class SubEventMasterDataBasePatch
{
	static bool Prefix(ref bool __result)
	{
		__result = Main.LoverConfig.UnlimitedLoveEvent.Value;
		return false;
	}
}
