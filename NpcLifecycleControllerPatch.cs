using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	[HarmonyPatch(typeof(NpcLifecycleController), nameof(NpcLifecycleController.WakeUpUpdate))]
	public class NpcLifecycleControllerPatch
	{
		static void Postfix()
		{
			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
			{
				if (data.IsSpouses)
				{
					data.Home = Define.Place.Police;
					data.BedPatrolPointName = "00_Police_b7";
					data.BedPatrolPoint = NpcPatrolPointManager.Instance.GetPoint(data.BedPatrolPointName).GetComponent<NpcPatrolPoint>();
				}
			}
		}
	}
}
