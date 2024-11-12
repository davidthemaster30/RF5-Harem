using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage))]
public class SubEventManagerPatch
{
	static bool Prefix(NpcData data, ref bool __result)
	{
		Main.Log.LogDebug($"SubEventManager.CheckCanMarriage npcid:{data?.NpcId}");

		bool relation = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.MarriageConfig.MinLoveStoryProgress.Value, 0, 9) &&
			NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.MarriageConfig.MinLoveLevel.Value, 0, 10000));

		bool ring = (!Main.MarriageConfig.NeedRing.Value || ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0);
		if (!ring)
		{
			Main.Log.LogWarning("CheckCanMarriage ring missing");
		}

		bool doublebed = (!Main.MarriageConfig.NeedDoubleBed.Value ||
			SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
		if (!doublebed)
		{
			Main.Log.LogWarning("CheckCanMarriage double bed missing");
		}

		bool eventFlag = (!Main.MarriageConfig.EventCheck.Value ||
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

		bool relation = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.MarriageConfig.MinLoveStoryProgress.Value, 0, 8) &&
			NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.MarriageConfig.MinLoveLevel.Value, 0, 10000));

		bool doublebed = (!Main.MarriageConfig.NeedDoubleBed.Value || SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
		if (!doublebed)
		{
			Main.Log.LogWarning("CheckCanMarriage double bed missing");
		}

		bool eventFlag = (!Main.MarriageConfig.EventCheck.Value ||
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
