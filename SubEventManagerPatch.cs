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
			Main.Log.LogDebug(string.Format("SubEventManager.CheckCanMarriage npcid:{0}", data?.NpcId));
			__result = ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0;

			// 缺少订婚戒指
			if(!__result)
				Main.Log.LogWarning("CheckCanMarriage ring missing");

			// 缺少双人床，由于不知道怎样才能解锁，这里强制解锁
			if (__result && !SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed))
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
				Main.Log.LogWarning("CheckCanMarriage doublebed missing");
			}

			return false;
		}
	}

	// 递戒指
	[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage_ThrowRing))]
	public class SubEventManagerPatch2
	{
		static bool Prefix(NpcData data, ref bool __result)
		{
			Main.Log.LogDebug(string.Format("SubEventManager.CheckCanMarriage_ThrowRing npcid:{0}", data?.NpcId));

			// 缺少双人床，由于不知道怎样才能解锁，这里强制解锁
			if (!SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed))
			{
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
				Main.Log.LogWarning("CheckCanMarriage_ThrowRing doublebed missing");
			}

			__result = true;
			return false;
		}
	}
}
