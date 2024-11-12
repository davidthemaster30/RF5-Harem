using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage))]
public class SubEventManagerPatch
{
	static bool Prefix(NpcData data, ref bool __result)
	{
		Main.Log.LogDebug($"SubEventManager.CheckCanMarriage npcid:{data?.NpcId}");

		bool relation = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveStoryProgress", 9), 0, 9) &&
			NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveLevel", 10), 0, 10000));

		bool ring = (!Main.Config.GetBool("Marriage", "NeedRing", true) || ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0);
		if (!ring)
		{
			Main.Log.LogWarning("CheckCanMarriage ring missing");
		}

		bool doublebed = (!Main.Config.GetBool("Marriage", "NeedDoubleBed", true) ||
			SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
		if (!doublebed)
		{
			Main.Log.LogWarning("CheckCanMarriage double bed missing");
		}

		bool eventFlag = (!Main.Config.GetBool("Marriage", "EventCheck", true) ||
			FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.CHAPTER1_D2_8));
		if (!eventFlag)
		{
			Main.Log.LogWarning("Event 1342 not completed");
		}

		if (__result = relation && ring && doublebed && eventFlag)
		{
			SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
			SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
		}

		return false;
	}
}

[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage_ThrowRing))]
public class SubEventManagerPatch2
{
	static bool Prefix(NpcData data, ref bool __result)
	{
		Main.Log.LogDebug($"SubEventManager.CheckCanMarriage_ThrowRing npcid:{data?.NpcId}");

		bool relation = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveStoryProgress", 8), 0, 8) &&
			NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveLevel", 10), 0, 10000));

		bool doublebed = (!Main.Config.GetBool("Marriage", "NeedDoubleBed", true) || SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
		if (!doublebed)
		{
			Main.Log.LogWarning("CheckCanMarriage double bed missing");
		}

		bool eventFlag = (!Main.Config.GetBool("Marriage", "EventCheck", true) ||
			(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.FLAG_WARP_NG) &&
			FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.CHAPTER1_D2_8)));
		if (!eventFlag)
		{
			Main.Log.LogWarning("Event 1342 not completed");
		}

		if (__result = relation && eventFlag && doublebed)
		{
			SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
			SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
		}

		return false;
	}
}
