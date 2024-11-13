using System.Collections.ObjectModel;
using Define;
using RF5_Harem.Configuration;

namespace RF5_Harem;

internal static class Relation
{
	private static int LastPoll = 0;
	internal static readonly ReadOnlyCollection<NPCID> ChildNPCIDs = new ReadOnlyCollection<NPCID>([
			NPCID.Baby,
			NPCID.ChildBoy,
			NPCID.ChildBoy1,
			NPCID.ChildBoy2,
			NPCID.ChildBoy3,
			NPCID.ChildGirl,
			NPCID.ChildGirl1,
			NPCID.ChildGirl2,
			NPCID.ChildGirl3
		]);

	internal static void HideSpouse()
	{
		NpcDataManagerPatch.hideSpouse = true;
		NpcDataManagerPatch.forceNPCID = 0;
		SaveData.SaveDataManager.PlayerData.MarriedNPCID = NPCID.Ares;

		Main.Log.LogDebug($"*** hideSpouse:{NpcDataManagerPatch.hideSpouse}, hideLover:{NpcDataManagerPatch.hideLover}, forceNPCID:{NpcDataManagerPatch.forceNPCID}, MarriedNPCID:{SaveData.SaveDataManager.PlayerData.MarriedNPCID}, PLAYER_MARRIED:{FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)}");
	}

	internal static void SetNPC(int npcid)
	{
		int oldNpcId = NpcDataManagerPatch.forceNPCID;

		NpcDataManagerPatch.hideSpouse = false;
		NpcDataManagerPatch.hideLover = false;
		NpcDataManagerPatch.forceNPCID = 0;

		if (npcid < NpcDataManagerPatch.MinNPCId)
		{
			HideSpouse();
			NpcDataManagerPatch.hideLover = true;
			return;
		}

		if (SpousesConfig.UnrelatedNPCDialogue.Value && (ChildNPCIDs.Contains((NPCID)npcid) ||
			!EventControllerBase.Instance.MarriageCandidateList.Contains((NPCID)npcid)))
		{
			npcid = oldNpcId < NpcDataManagerPatch.MinNPCId ? RandomSpouses() : oldNpcId;
			if (npcid < NpcDataManagerPatch.MinNPCId)
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

	internal static int RandomSpouses()
	{
		List<int> spouses = new List<int>();

		for (int i = 0; i <= NpcDataManagerPatch.MaxNPCId; i++)
		{
			if (NpcDataManager.Instance.NpcDatas[i].IsSpouses)
			{
				spouses.Add(NpcDataManager.Instance.NpcDatas[i].NpcId);
			}
		}

		if (spouses.Count == 0)
		{
			return 0;
		}

		int choose = SpousesConfig.Alternation.Value
			? LastPoll++ % spouses.Count
			: new Random(TimeManager.Instance.ElapsedTime).Next(0, spouses.Count);

		return spouses[choose];
	}
}
