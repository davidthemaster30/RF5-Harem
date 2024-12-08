using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class SVPatch
{
	private const string PLAYER_BED = "00_Police_b7";

	[HarmonyPatch(typeof(SV), nameof(SV.AutoSave))]
	internal static void Prefix(AutoSaveType type)
	{
		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)(SpousesConfig.SaveLogo.Value == 1 ? Relation.RandomSpouses() : SpousesConfig.SaveLogo.Value);

		HaremPlugin.Log.LogDebug($"AutoSave npcid:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}");

		if (type != AutoSaveType.PlayerSleep)
		{
			return;
		}

		for (int i = 0; i <= NpcDataManagerPatch.MaxNPCId; i++)
		{
			NpcData data = NpcDataManager.Instance.NpcDatas[i];

			if (!data.IsSpouses)
			{
				continue;
			}

			if (data.Home != Define.Place.Police || data.BedPatrolPointName != PLAYER_BED || data.ChatTalkLv < 4)
			{
				data.Home = Define.Place.Police;
				data.BedPatrolPointName = PLAYER_BED;
				data.BedPatrolPoint = NpcPatrolPointManager.Instance.GetPoint(data.BedPatrolPointName).GetComponent<NpcPatrolPoint>();
				data.SetChatTalkLv(4, true);
				HaremPlugin.Log.LogInfo($"ResetNpcBed npcid:{data.NpcId}({(Define.NPCID)data.NpcId}), Home:{data.Home}, Bed:{data.BedPatrolPointName}, NickName:{data.NickNameFromPlayerId}, PlayerNickName:{data.NickNameToPlayerId}");
			}
		}
	}
}
