using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 表白可行性
	[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckCanbeLoverNPCID))]
	public class ConditionsForLoverJudgmentPatch
	{
		static bool Prefix(ref bool __result)
		{
			__result = true;
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
			__result = (EventControllerBase.Instance.GetNpcLoveStoryProgress(npcid) >= 4);
			return false;
		}
	}

	// 恋人约会
	[HarmonyPatch(typeof(ConditionsForLoverJudgment), nameof(ConditionsForLoverJudgment.CheckLoveEventDateJudgment))]
	public class ConditionsForLoverJudgmentPatch3
	{
		static bool Prefix(int npcid, ref int __result)
		{
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
					__result = 0;
					break;
				}
			}

			return false;
		}
	}
}
