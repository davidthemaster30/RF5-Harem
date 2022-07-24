using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 对话求婚
	[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage))]
	public class SubEventManagerPatch
	{
		static bool Prefix(ref bool __result)
		{
			__result = (SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed) &&
				ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0);
			return false;
		}
	}

	// 递戒指求婚
	[HarmonyPatch(typeof(SubEventManager), nameof(SubEventManager.CheckCanMarriage_ThrowRing))]
	public class SubEventManagerPatch2
	{
		static bool Prefix(ref bool __result)
		{
			__result = SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed);
			return false;
		}
	}
}
