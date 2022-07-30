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

			// 个人线进度以及关系等级
			__result = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveStoryProgress", 8), 0, 8) &&
				NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveLevel", 10), 0, 10000));

			// 订婚戒指
			bool ring = (!Main.Config.GetBool("Marriage", "NeedRing", true) || ItemStorageManager.GetStorage(Define.StorageType.Rucksack).GetItemAmoutId(ItemID.Item_Konyakuyubiwa) > 0);
			if(!ring)
				Main.Log.LogWarning("CheckCanMarriage ring missing");

			// 双人床
			bool doublebed = (!Main.Config.GetBool("Marriage", "NeedDoubleBed", true) || SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
			if(!doublebed)
				Main.Log.LogWarning("CheckCanMarriage double bed missing");

			if(__result = __result && ring && doublebed)
			{
				// 强制解锁双人床(如果还没有)
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
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

			// 个人线进度以及关系等级
			__result = (EventControllerBase.Instance.GetNpcLoveStoryProgress(data.NpcId) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveStoryProgress", 8), 0, 8) &&
				NpcDataManager.Instance.LovePointManager.GetLoveLvByNpcData(data) >= MathRF.Clamp(Main.Config.GetInt("Marriage", "MinLoveLevel", 10), 0, 10000));

			// 双人床
			bool doublebed = (!Main.Config.GetBool("Marriage", "NeedDoubleBed", true) || SaveData.SaveDataManager.BuildData.CheckBuilder(RF5SHOP.BuilderId.Build_Police_doublebed));
			if (!doublebed)
				Main.Log.LogWarning("CheckCanMarriage double bed missing");

			if (__result = __result && doublebed)
			{
				// 强制解锁双人床(如果还没有)
				SaveData.SaveDataManager.GameSaveData.EventData.SaveFlag.SetFlag((int)Define.GameFlagData.FLAG_Extend_DoubleBed, true);
				SaveData.SaveDataManager.BuildData.SetBuilder(true, RF5SHOP.BuilderId.Build_Police_doublebed);
			}

			return false;
		}
	}
}
