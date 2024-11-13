using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.DoInteraction), typeof(HumanController))]
internal static class PlayerBedControllerTrySleep
{
	internal static void Prefix()
	{
		int spouse = SpousesConfig.Bedmate.Value == 1 ? Relation.RandomSpouses() : SpousesConfig.Bedmate.Value;

		Relation.SetNPC(spouse);
		Main.Log.LogDebug($"PlayerBedController.DoInteraction npcid:{spouse}");
	}
}

[HarmonyPatch(typeof(PlayerBedController), nameof(PlayerBedController.PlayerSleep))]
internal static class PlayerBedControllerSleep
{
	internal static void Prefix()
	{
		if (!SpousesConfig.Cohabitation.Value)
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
internal static class PlayerBedControllerJudgment
{
	internal static bool Prefix(ref bool __result)
	{
		if (!NpcDataManagerPatch.ForcedNPCisNotPlayer || !SpousesConfig.ForceBedmate.Value)
		{
			return true;
		}

		__result = true;
		return false;
	}
}
