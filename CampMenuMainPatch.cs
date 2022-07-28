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

			long spouses = MathRF.Clamp(Main.Config.GetInt("Spouses", "SaveLogo", 1), 0, 14);
			if (spouses == 1)
				spouses = Relation.RandomSpouses();

			// 修复存档时没有双人图片
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)spouses;
			Main.Log.LogDebug(string.Format("StartCamp npcid:{0}", SaveData.SaveDataManager.PlayerData.MarriedNPCID));
		}
	}
}
