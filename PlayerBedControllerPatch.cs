using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.DoInteraction), typeof(HumanController))]
public class PlayerBedControllerTrySleep
{
	static void Prefix()
	{
		long spouses = MathRF.Clamp(Main.Config.GetInt("Spouses", "Bedmate", 1), 0, 14);
		if (spouses == 1)
		{
			spouses = Relation.RandomSpouses();
		}

		Relation.SetNPC((int)spouses);
		Main.Log.LogDebug(string.Format("PlayerBedController.DoInteraction npcid:{0}", spouses));
	}
}

[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.PlayerSleep))]
public class PlayerBedControllerSleep
{
	static void Prefix()
	{
		if (!Main.Config.GetBool("Spouses", "Cohabitation", true))
		{
			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
			{
				if (data.IsSpouses && data.NpcId != NpcDataManagerPatch.forceNPCID)
				{
					data.Home = data.statusData.Home;
					data.BedPatrolPointName = data.statusData.BedPatrolPointName;
					data.BedPatrolPoint = NpcPatrolPointManager.Instance.GetPoint(data.BedPatrolPointName).GetComponent<NpcPatrolPoint>();
					Main.Log.LogInfo(string.Format("GoHomeSleep npcid:{0}({1}), Home:{2}, Bed:{3}", data.NpcId, (Define.NPCID)data.NpcId, data.Home, data.BedPatrolPointName));
				}
			}
		}

		Main.Log.LogDebug(string.Format("PlayerBedController.PlayerSleep npcid:{0}", NpcDataManagerPatch.forceNPCID));
	}
}

[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.BedJudgment))]
public class PlayerBedControllerJudgment
{
	static bool Prefix(ref bool __result)
	{
		if (NpcDataManagerPatch.forceNPCID < 2 || !Main.Config.GetBool("Spouses", "ForceBedmate", true))
		{
			return true;
		}

		__result = true;
		return false;
	}
}

