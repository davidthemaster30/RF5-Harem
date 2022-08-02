using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 生崽对话相关检查
	[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.WakeUpUpdate))]
	public class GiveBirthControllerUpdate
	{
		static bool Prefix(GiveBirthController __instance, ref bool __result)
		{
			if(Relation.RandomSpouses() < 2)
			{
				__result = false;
				return false;
			}

			// 这个是什么不清楚
			int nowType = SaveData.SaveDataManager.NpcData.GiveBirthParams.NowType;
			if (nowType == 9)
				__result = true;
			else if(nowType == 11 || nowType == 0)
				__result = false;
			else if(SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays > TimeManager.Instance.CurrentDate().LowTime)
				__result = false;
			else
				__result = true;

			if(__result)
				__instance.IsGiveBirthTalkOn = true;

			Main.Log.LogDebug(string.Format("GiveBirthController.WakeUpUpdate nowtype:{0}, targetdays:{1}, curdays:{2}, diff:{3}",
				nowType,
				SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays,
				TimeManager.Instance.CurrentDate().LowTime,
				SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays - TimeManager.Instance.CurrentDate().LowTime
			));
			return false;
		}
	}

	// 防止覆盖数据
	[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.DoMarriage))]
	class GiveBirthControllerDoMarriage
	{
		static bool Prefix(GiveBirthController __instance)
		{
			NpcDataManager.Instance.GetSpouseNpcData()?.SetTalkedTime(Define.NpcTalkedType.BirthChild, TimeManager.Instance.CurrentTimeInt);
			return SaveData.SaveDataManager.NpcData.GiveBirthParams.NowType <= 0;
		}
	}
}
