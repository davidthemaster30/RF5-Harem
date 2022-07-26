using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 打开菜单界面
	[HarmonyPatch(typeof(CampMenuMain), nameof(CampMenuMain.StartCamp))]
	public class CampMenuMainPatch
	{
		static void Prefix()
		{
			NpcDataManagerPatch.hideLover = false;
			NpcDataManagerPatch.hideSpouse = false;
			NpcDataManagerPatch.forceNPCID = 0;

			// 修复存档时没有双人图片
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)Relation.RandomSpouses();
		}
	}
}
