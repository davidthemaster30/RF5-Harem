using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class GiveBirthControllerPatch
{
	[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.WakeUpUpdate))]
	internal static bool Prefix(GiveBirthController __instance, ref bool __result)
	{
		if (Relation.RandomSpouses() < NpcDataManagerPatch.MinNPCId)
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

		if (SpousesConfig.ChildBed.Value && __instance.IsGiveBirthTalkOn && SaveData.SaveDataManager.NpcData.GiveBirthParams.CanChildBedBuy())
		{
			FlagDataStorage.SetScriptFlag(true, (int)Define.GameFlagData.FLAG_Having_ChildBed);
		}

		HaremPlugin.Log.LogDebug($"GiveBirthController.WakeUpUpdate nowtype:{nowType}, targetdays:{SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays}, curdays:{TimeManager.Instance.CurrentDate().LowTime}, diff:{SaveData.SaveDataManager.NpcData.GiveBirthParams.Targetdays - TimeManager.Instance.CurrentDate().LowTime}");
		return false;
	}

	[HarmonyPatch(typeof(GiveBirthController), nameof(GiveBirthController.DoMarriage))]
	internal static bool Prefix(GiveBirthController __instance)
	{
		NpcDataManager.Instance.GetSpouseNpcData()?.SetTalkedTime(Define.NpcTalkedType.BirthChild, TimeManager.Instance.CurrentTimeInt);
		return SaveData.SaveDataManager.NpcData.GiveBirthParams.NowType <= 0;
	}
}
