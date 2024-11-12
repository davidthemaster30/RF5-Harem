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

		Main.Log.LogDebug($"*** hideSpouse:{NpcDataManagerPatch.hideSpouse}, hideLover:{NpcDataManagerPatch.hideLover}, forceNPCID:{NpcDataManagerPatch.forceNPCID}, MarriedNPCID:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}, PLAYER_MARRIED:{FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)}");
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

		if (Main.SpousesConfig.UnrelatedNPCDialogue.Value && (ChildNPCIDs.Contains((Define.NPCID)npcid) ||
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

			Main.Log.LogDebug($"*** hideSpouse:{NpcDataManagerPatch.hideSpouse}, hideLover:{NpcDataManagerPatch.hideLover}, forceNPCID:{NpcDataManagerPatch.forceNPCID}, MarriedNPCID:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}, PLAYER_MARRIED:{FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)}");
			return;
		}

		NpcDataManagerPatch.forceNPCID = npcid;
		SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)npcid;

		Main.Log.LogDebug($"*** hideSpouse:{NpcDataManagerPatch.hideSpouse}, hideLover:{NpcDataManagerPatch.hideLover}, forceNPCID:{NpcDataManagerPatch.forceNPCID}, MarriedNPCID:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}, PLAYER_MARRIED:{FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)}");
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
		if (Main.SpousesConfig.Alternation.Value)
		{
			choose = Math.Abs(LastPoll++ % (top + 1));
		}
		else
		{
			choose = new Random(TimeManager.Instance.ElapsedTime).Next(0, top);
		}

		return npcs[choose].NpcId;
	}
}
