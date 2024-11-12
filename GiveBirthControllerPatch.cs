using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.WakeUpUpdate))]
public class GiveBirthControllerUpdate
{
	static bool Prefix(GiveBirthController __instance, ref bool __result)
	{
		if (Relation.RandomSpouses() < 2)
		{
			__result = false;
			return false;
		}

		int nowType = SaveData.SaveDataManager.NpcData.GiveBirthParams.NowType;
		if (nowType == 9)
		{
			__result = true;
		}
		else if (nowType == 11 || nowType == 0)
		{
			__result = false;
		}
		else if (SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays > TimeManager.Instance.CurrentDate().LowTime)
		{
			__result = false;
		}
		else
		{
			__result = true;
		}

		if (__result)
		{
			__instance.IsGiveBirthTalkOn = true;
		}

		if (Main.Config.GetBool("Spouses", "ChildBed", false) && __instance.IsGiveBirthTalkOn && SaveData.SaveDataManager.NpcData.GiveBirthParams.CanChildBedBuy())
		{
			FlagDataStorage.SetScriptFlag(true, (int)Define.GameFlagData.FLAG_Having_ChildBed);
		}

		Main.Log.LogDebug(string.Format("GiveBirthController.WakeUpUpdate nowtype:{0}, targetdays:{1}, curdays:{2}, diff:{3}",
			nowType,
			SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays,
			TimeManager.Instance.CurrentDate().LowTime,
			SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays - TimeManager.Instance.CurrentDate().LowTime
		));
		return false;
	}
}

[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.DoMarriage))]
class GiveBirthControllerDoMarriage
{
	static bool Prefix(GiveBirthController __instance)
	{
		NpcDataManager.Instance.GetSpouseNpcData()?.SetTalkedTime(Define.NpcTalkedType.BirthChild, TimeManager.Instance.CurrentTimeInt);
		return SaveData.SaveDataManager.NpcData.GiveBirthParams.NowType <= 0;
	}
}
