using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 防止个人线被禁用
	[HarmonyPatch(typeof(SubEventMasterDataBase), nameof(SubEventMasterDataBase.CheckOccurrenceLoveProgress))]
	public class SubEventMasterDataBasePatch
	{
		static bool Prefix(ref bool __result)
		{
			__result = true;
			return false;
		}
	}
}
