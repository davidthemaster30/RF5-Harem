using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 随机选一个陪睡
	[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.DoInteraction), typeof(HumanController))]
	public class PlayerBedControllerPatch
	{
		static void Prefix()
		{
			long spouses = MathRF.Clamp(Main.Config.GetInt("Spouses", "Bedmate", 1), 0, 14);
			if (spouses == 1)
				spouses = Relation.RandomSpouses();

			Relation.SetNPC((int)spouses);

			if(!Main.Config.GetBool("Spouses", "Cohabitation", true))
			{
				foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
				{
					if(data.IsSpouses && data.NpcId != spouses)
					{
						data.Home = data.statusData.Home;
						data.BedPatrolPointName = data.statusData.BedPatrolPointName;
						data.BedPatrolPoint = NpcPatrolPointManager.Instance.GetPoint(data.BedPatrolPointName).GetComponent<NpcPatrolPoint>();
					}
				}
			}

			Main.Log.LogInfo(string.Format("PlayerBedController.DoInteraction npcid:{0}", spouses));
		}
	}
}
