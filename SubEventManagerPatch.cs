﻿using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]

internal static class SubEventManagerPatch
{
	[HarmonyPatch]
	internal static class SubEventManagerCheckCanMarriage
	{
		[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage))]
		internal static bool Prefix(NpcData data, ref bool __result)
		{
			if (data is null)
			{
				return false;
			}

			HaremPlugin.Log.LogDebug($"SubEventManager.CheckCanMarriage npcid:{data.NpcId}");

			bool relation = EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MarriageConfig.MinLoveStoryProgress.Value &&
				NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MarriageConfig.MinLoveLevel.Value;

			bool ring = !MarriageConfig.NeedRing.Value || ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0;
			if (!ring)
			{
				HaremPlugin.Log.LogWarning("CheckCanMarriage ring missing");
			}

			bool doublebed = !MarriageConfig.NeedDoubleBed.Value ||
				SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed);
			if (!doublebed)
			{
				HaremPlugin.Log.LogWarning("CheckCanMarriage double bed missing");
			}

			bool eventFlag = !MarriageConfig.EventCheck.Value ||
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.CHAPTER1_D2_8);
			if (!eventFlag)
			{
				HaremPlugin.Log.LogWarning("Event 1342 not completed");
			}

			if (__result == relation && ring && doublebed && eventFlag)
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
			}

			return false;
		}
	}

	[HarmonyPatch]
	internal static class SubEventManagerCheckCanMarriage_ThrowRing
	{
		[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage_ThrowRing))]
		internal static bool Prefix(NpcData data, ref bool __result)
		{
			if (data is null)
			{
				return false;
			}

			HaremPlugin.Log.LogDebug($"SubEventManager.CheckCanMarriage_ThrowRing npcid:{data.NpcId}");

			bool relation = EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MarriageConfig.MinLoveStoryProgress.Value &&
				NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MarriageConfig.MinLoveLevel.Value;

			bool doublebed = !MarriageConfig.NeedDoubleBed.Value || SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed);
			if (!doublebed)
			{
				HaremPlugin.Log.LogWarning("CheckCanMarriage double bed missing");
			}

			bool eventFlag = !MarriageConfig.EventCheck.Value ||
				(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.FLAG_WARP_NG) &&
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.CHAPTER1_D2_8));
			if (!eventFlag)
			{
				HaremPlugin.Log.LogWarning("Event 1342 not completed");
			}

			if (__result == relation && eventFlag && doublebed)
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
			}

			return false;
		}
	}
}