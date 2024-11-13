using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckCanbeLoverNPCID))]
internal static class ConditionsForLoverJudgmentPatch
{
	internal static bool Prefix(Define.NPCID npcid, ref bool __result)
	{
		NpcData data = NpcDataManager.Instance.GetNpcData(npcid);
		if (data is null)
		{
			__result = false;
		}
		else if (NpcDataManager.Instance.GetLoverNum() > 0)
		{
			__result = data.IsLover;
		}
		else
		{
			__result = data.statusData.MarriageCandidate;
		}

		return false;
	}
}

[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckConditionsForLoverJudgment))]
internal static class ConditionsForLoverJudgmentPatch2
{
	internal static bool Prefix(int npcid, ref bool __result)
	{
		bool relation = EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid) >= LoverConfig.MinLoveStoryProgress.Value &&
			NpcDataManager.Instance.LovePointManager.GetLoveLv(npcid) >= LoverConfig.MinLoveLevel.Value;

		bool eventFlag = !LoverConfig.EventCheck.Value ||
			(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.TOWN_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.SERIOUS_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.LASTEPISODE));
		if (!eventFlag)
		{
			Main.Log.LogWarning("Events 20, 21, 22, 1250 in progress");
		}

		__result = relation && eventFlag;
		return false;
	}
}

[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckLoveEventDateJudgment))]
internal static class ConditionsForLoverJudgmentPatch3
{
	internal static bool Prefix(int npcid, ref int __result)
	{
		bool eventFlag = !LoverConfig.DateEventCheck.Value ||
			(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.TOWN_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.FLAG_DATE_RESERVATION_NG));
		if (!eventFlag)
		{
			Main.Log.LogWarning("Events 22, 23 in progress");
			__result = 0;
			return false;
		}

		switch (EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid))
		{
			case 5:
				{
					__result = 1;
					break;
				}
			case 6:
				{
					__result = 2;
					break;
				}
			case 7:
				{
					__result = 3;
					break;
				}
			default:
				{
					__result = 0;
					break;
				}
		}

		return false;
	}
}