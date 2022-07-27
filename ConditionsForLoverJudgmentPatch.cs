using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 朋友界面显示红心
	[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckCanbeLoverNPCID))]
	public class ConditionsForLoverJudgmentPatch
	{
		static bool Prefix(Define.NPCID npcid, ref bool __result)
		{
			NpcData data = NpcDataManager.Instance.GetNpcData(npcid);
			if (data == null)
				__result = false;
			else if (NpcDataManager.Instance.GetLoverNum() > 0)
				__result = data.IsLover;
			else
				__result = data.statusData.MarriageCandidate;

			return false;
		}
	}

	// 表白成功判定
	[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckConditionsForLoverJudgment))]
	public class ConditionsForLoverJudgmentPatch2
	{
		static bool Prefix(int npcid, ref bool __result)
		{
			// 先完成个人线的部分剧情才可以表白
			__result = (EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid) >= MathRF.Clamp(Main.Config.GetInt("Lover", "MinLoveStoryProgress", 4), 0, 4) &&
				NpcDataManager.Instance.LovePointManager.GetLoveLv(npcid) >= MathRF.Clamp(Main.Config.GetInt("Lover", "MinLoveLevel", 4), 0, 10000));
			return false;
		}
	}

	// 恋人约会
	[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckLoveEventDateJudgment))]
	public class ConditionsForLoverJudgmentPatch3
	{
		static bool Prefix(int npcid, ref int __result)
		{
			// 前三次是恋人特殊约会
			switch(EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid))
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
					// 之后只有普通约会
					__result = 0;
					break;
				}
			}

			return false;
		}
	}
}
