using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	/*
	// 绕过某些检查
	[HarmonyPatch(typeof(FlagDataStorage), nameof(FlagDataStorage.CheckScriptFlag))]
	public class FlagDataStoragePatch
	{
		static bool Prefix(int eventScriptFlagID, ref bool __result)
		{
			switch((Define.GameFlagData)eventScriptFlagID)
			{
				// SubEventManager.CheckCanMarriage
				case Define.GameFlagData.CHAPTER1_D2_8:
				{
					__result = true;
					return false;
				}

				// ConditionsForLoverJudgment.CheckConditionsForLoverJudgment
				case Define.GameFlagData.LASTEPISODE:

				// ConditionsForLoverJudgment.CheckLoveEventDateJudgment
				case Define.GameFlagData.FLAG_DATE_RESERVATION_NG:

				// ConditionsForLoverJudgment.CheckConditionsForLoverJudgment & ConditionsForLoverJudgment.CheckLoveEventDateJudgment
				case Define.GameFlagData.SERIOUS_EVENT:
				case Define.GameFlagData.CHARA_ACTOR_MARRIAGE:
				case Define.GameFlagData.TOWN_EVENT:

				// SubEventManager.CheckCanMarriage_ThrowRing
				case Define.GameFlagData.FLAG_WARP_NG:
				case Define.GameFlagData.LOVESTORY_REINHARD10_2:
				{
					__result = false;
					return false;
				}
			}

			return true;
		}
	}
	*/
}
