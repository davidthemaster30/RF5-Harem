using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
public class CampMenuMainPatch
{
	static void Prefix()
	{
		NpcDataManagerPatch.hideLover = false;
		NpcDataManagerPatch.hideSpouse = false;
		NpcDataManagerPatch.forceNPCID = 0;

		long spouses = MathRF.Clamp(Main.Config.GetInt("Spouses", "SaveLogo", 1), 0, 14);
		if (spouses == 1)
		{
			spouses = Relation.RandomSpouses();
		}

		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)spouses;
		Main.Log.LogDebug($"StartCamp npcid:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}");
	}
}
