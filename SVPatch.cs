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
		Main.Log.LogDebug(string.Format("AutoSave npcid:{0}", SaveData.SaveDataManager.PlayerData.MarriedNPCID));

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
				Main.Log.LogInfo(string.Format("ResetNpcBed npcid:{0}({1}), Home:{2}, Bed:{3}, NickName:{4}, PlayerNickName:{5}",
					data.NpcId, (Define.NPCID)data.NpcId, data.Home, data.BedPatrolPointName,
					data.NickNameFromPlayerId, data.NickNameToPlayerId
				));
			}
		}
	}
}
