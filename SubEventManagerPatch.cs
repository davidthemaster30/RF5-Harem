using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 求婚
	[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage))]
	public class SubEventManagerPatch
	{
		static bool Prefix(NpcData data, ref bool __result)
		{
			__result = ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0;
			if (__result && !SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed))
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
			}

			Main.Log.LogDebug(string.Format("SubEventManager.CheckCanMarriage npcid:{0}", data?.NpcId));
			return false;
		}
	}

	// 递戒指
	[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage_ThrowRing))]
	public class SubEventManagerPatch2
	{
		static bool Prefix(NpcData data, ref bool __result)
		{
			if (!SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed))
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
			}

			Main.Log.LogDebug(string.Format("SubEventManager.CheckCanMarriage npcid:{0}", data?.NpcId));
			__result = true;
			return false;
		}
	}
}
