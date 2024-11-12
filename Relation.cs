
namespace RF5_Harem;
public class Relation
{
	static int LastPoll = 0;
	static HashSet<Define.NPCID> ChildNPCIDs = new HashSet<Define.NPCID> {
			Define.NPCID.Baby,
			Define.NPCID.ChildBoy,
			Define.NPCID.ChildBoy1,
			Define.NPCID.ChildBoy2,
			Define.NPCID.ChildBoy3,
			Define.NPCID.ChildGirl,
			Define.NPCID.ChildGirl1,
			Define.NPCID.ChildGirl2,
			Define.NPCID.ChildGirl3,
		};

	public static void HideSpouse()
	{
		NpcDataManagerPatch.hideSpouse = true;
		NpcDataManagerPatch.forceNPCID = 0;
		SaveData.SaveDataManager.PlayerData.MarriedNPCID = Define.NPCID.Ares;

		/*
		FlagDataStorage.SetScriptFlag(false, (int)Define.GameFlagData.PLAYER_MARRIED);
		SaveData.SaveDataManager.GameSaveData.StampData.GetStampRecord(StampEnum.Marriage).StampLevel = StampLevel.None;
		*/

		Main.Log.LogDebug(string.Format("*** hideSpouse:{0}, hideLover:{1}, forceNPCID:{2}, MarriedNPCID:{3}, PLAYER_MARRIED:{4}",
			NpcDataManagerPatch.hideSpouse, NpcDataManagerPatch.hideLover,
			NpcDataManagerPatch.forceNPCID, SaveData.SaveDataManager.PlayerData.MarriedNPCID,
			FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
		));
	}

	public static void SetNPC(int npcid)
	{
		int oldNpcId = NpcDataManagerPatch.forceNPCID;

		NpcDataManagerPatch.hideSpouse = false;
		NpcDataManagerPatch.hideLover = false;
		NpcDataManagerPatch.forceNPCID = 0;

		if (npcid < 2)
		{
			HideSpouse();
			NpcDataManagerPatch.hideLover = true;
			return;
		}

		if (Main.Config.GetBool("Spouses", "UnrelatedNPCDialogue", true) && (ChildNPCIDs.Contains((Define.NPCID)npcid) ||
			!EventControllerBase.Instance.MarriageCandidateList.Contains((Define.NPCID)npcid)))
		{
			npcid = oldNpcId < 2 ? RandomSpouses() : oldNpcId;
			if (npcid < 2)
			{
				HideSpouse();
				NpcDataManagerPatch.hideLover = true;
				return;
			}
		}

		if (!NpcDataManager.Instance.IsSpouseNpc(npcid))
		{
			HideSpouse();


			if (NpcDataManager.Instance.IsLover(npcid))
			{
				NpcDataManagerPatch.forceNPCID = npcid;
			}
			else
			{
				NpcDataManagerPatch.hideLover = true;
			}

			Main.Log.LogDebug(string.Format("*** hideSpouse:{0}, hideLover:{1}, forceNPCID:{2}, MarriedNPCID:{3}, PLAYER_MARRIED:{4}",
				NpcDataManagerPatch.hideSpouse, NpcDataManagerPatch.hideLover,
				NpcDataManagerPatch.forceNPCID, SaveData.SaveDataManager.PlayerData.MarriedNPCID,
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
			));
			return;
		}

		NpcDataManagerPatch.forceNPCID = npcid;
		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)npcid;

		Main.Log.LogDebug(string.Format("*** hideSpouse:{0}, hideLover:{1}, forceNPCID:{2}, MarriedNPCID:{3}, PLAYER_MARRIED:{4}",
			NpcDataManagerPatch.hideSpouse, NpcDataManagerPatch.hideLover,
			NpcDataManagerPatch.forceNPCID, SaveData.SaveDataManager.PlayerData.MarriedNPCID,
			FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
		));
	}

	public static int RandomSpouses()
	{
		int top = -1;
		NpcData[] npcs = new NpcData[(int)Define.NPCID.Max];
		foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
		{
			if (data.IsSpouses)
			{
				npcs[++top] = data;
			}
		}

		if (top < 0)
		{
			return 0;
		}

		int choose;
		if (Main.Config.GetBool("Spouses", "Alternation", true))
		{
			choose = Math.Abs(LastPoll++ % (top + 1));
		}
		else
		{
			choose = new Random(TimeManager.Instance.ElapsedTime).Next(0, top);
		}

		// Main.Log.LogInfo(string.Format("Spouses Hit:{0}, npcid:{1}({2})", choose, npcs[choose].NpcId, (Define.NPCID)npcs[choose].NpcId));
		return npcs[choose].NpcId;
	}
}
