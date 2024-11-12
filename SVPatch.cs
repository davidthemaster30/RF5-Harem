using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(SV), "AutoSave")]
public class SVAutoSave
{
	private const string PLAYER_BED = "00_Police_b7";

	static void Prefix(AutoSaveType type)
	{
		long spouses = MathRF.Clamp(Main.Config.GetInt("Spouses", "SaveLogo", 1), 0, 14);
		if (spouses == 1)
		{
			spouses = Relation.RandomSpouses();
		}

		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)spouses;
		Main.Log.LogDebug($"AutoSave npcid:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}");

		if (type != AutoSaveType.PlayerSleep)
		{
			return;
		}

		foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
		{
			if (!data.IsSpouses)
			{
				continue;
			}

			if (data.Home != Define.Place.Police || data.BedPatrolPointName != "00_Police_b7" || data.ChatTalkLv < 4)
			{
				data.Home = Define.Place.Police;
				data.BedPatrolPointName = PLAYER_BED;
				data.BedPatrolPoint = NpcPatrolPointManager.Instance.GetPoint(data.BedPatrolPointName).GetComponent<NpcPatrolPoint>();
				data.SetChatTalkLv(4, true);
				Main.Log.LogInfo($"ResetNpcBed npcid:{data.NpcId}({(Define.NPCID)data.NpcId}), Home:{data.Home}, Bed:{data.BedPatrolPointName}, NickName:{data.NickNameFromPlayerId}, PlayerNickName:{data.NickNameToPlayerId}");
			}
		}
	}
}
