using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class CampMenuMainPatch
{
	[HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
	internal static void Prefix()
	{
		NpcDataManagerPatch.hideLover = false;
		NpcDataManagerPatch.hideSpouse = false;
		NpcDataManagerPatch.forceNPCID = 0;

		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)(SpousesConfig.SaveLogo.Value == 1 ? Relation.RandomSpouses() : SpousesConfig.SaveLogo.Value);
		Main.Log.LogDebug($"StartCamp npcid:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}");
	}
}
