using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
internal static class CampMenuMainPatch
{
	internal static void Prefix()
	{
		NpcDataManagerPatch.hideLover = false;
		NpcDataManagerPatch.hideSpouse = false;
		NpcDataManagerPatch.forceNPCID = 0;

		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)(SpousesConfig.SaveLogo.Value == 1 ? Relation.RandomSpouses() : SpousesConfig.SaveLogo.Value);
		Main.Log.LogDebug($"StartCamp npcid:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}");
	}
}
