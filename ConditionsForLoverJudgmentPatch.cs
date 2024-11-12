using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckCanbeLoverNPCID))]
public class ConditionsForLoverJudgmentPatch
{
	static bool Prefix(Define.NPCID npcid, ref bool __result)
	{
		NpcData data = NpcDataManager.Instance.GetNpcData(npcid);
		if (data == null)
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
public class ConditionsForLoverJudgmentPatch2
{
	static bool Prefix(int npcid, ref bool __result)
	{
		// 先完成个人线剧情以及关系检查
		bool relation = (EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid) >= MathRF.Clamp(Main.Config.GetInt("Lover", "MinLoveStoryProgress", 4), 0, 4) &&
			NpcDataManager.Instance.LovePointManager.GetLoveLv(npcid) >= MathRF.Clamp(Main.Config.GetInt("Lover", "MinLoveLevel", 4), 0, 10000));

		bool eventFlag = (!Main.Config.GetBool("Lover", "EventCheck", true) ||
			(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.TOWN_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.SERIOUS_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.LASTEPISODE)));
		if (!eventFlag)
		{
			Main.Log.LogWarning(string.Format("Events 20, 21, 22, 1250 in progress"));
		}

		__result = relation && eventFlag;
		return false;
	}
}

[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckLoveEventDateJudgment))]
public class ConditionsForLoverJudgmentPatch3
{
	static bool Prefix(int npcid, ref int __result)
	{
		bool eventFlag = (!Main.Config.GetBool("Lover", "DateEventCheck", true) ||
			(!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.TOWN_EVENT) &&
			!FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.FLAG_DATE_RESERVATION_NG)));
		if (!eventFlag)
		{
			Main.Log.LogWarning(string.Format("Events 22, 23 in progress"));
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