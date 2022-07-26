using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 修复存档图片
	[HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
	public class CampMenuMainPatch
	{
		static void Prefix()
		{
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)Relation.RandomSpouses();
		}
	}
}
