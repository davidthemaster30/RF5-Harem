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
		Main.Log.LogDebug($"PlayerBedController.DoInteraction npcid:{spouses}");
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
					Main.Log.LogInfo($"GoHomeSleep npcid:{data.NpcId}({(Define.NPCID)data.NpcId}), Home:{data.Home}, Bed:{data.BedPatrolPointName}");
				}
			}
		}

		Main.Log.LogDebug($"PlayerBedController.PlayerSleep npcid:{NpcDataManagerPatch.forceNPCID}");
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

