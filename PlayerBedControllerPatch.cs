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
			int npcid = Relation.RandomSpouses();
			Relation.SetNPC(npcid);
			Main.Log.LogDebug(string.Format("PlayerBedController.DoInteraction npcid:{0}", npcid));
		}
	}
}
