using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RF5_Harem
{
	public class Relation
	{
		public static void SetNotLover()
		{
			NpcDataManagerPatch.hideLover = true;
			NpcDataManagerPatch.forceNPCID = 0;
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = Define.NPCID.Ares;

			/*
			int[] flags = new int[1];
			flags[0] = (int)Define.GameFlagData.PLAYER_MARRIED;
			FlagDataStorage.SetScriptFlag(false, flags);

			SaveData.SaveDataManager.GameSaveData.StampData.GetStampRecord(StampEnum.Marriage).StampLevel = StampLevel.None;
			*/

			Main.Log.LogDebug(string.Format("hideLover:{0}, forceNPCID:{1}, MarriedNPCID:{2}, PLAYER_MARRIED:{3}",
				NpcDataManagerPatch.hideLover, NpcDataManagerPatch.forceNPCID,
				SaveData.SaveDataManager.PlayerData.MarriedNPCID,
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
			));
		}

		public static void SetNPC(int npcid)
		{
			// 下面的 IsSpouseNpc 会读取，所以需要放在这里
			NpcDataManagerPatch.hideLover = false;
			NpcDataManagerPatch.forceNPCID = 0;

			if (npcid < 2 || !NpcDataManager.Instance.IsSpouseNpc(npcid))
			{
				SetNotLover();
				return;
			}

			NpcDataManagerPatch.hideLover = false;
			NpcDataManagerPatch.forceNPCID = npcid;

			SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)npcid;

			/*
			SaveData.SaveDataManager.GameSaveData.StampData.GetStampRecord(StampEnum.Marriage).StampLevel = StampLevel.Bronze;

			int[] flags = new int[1];
			flags[0] = (int)Define.GameFlagData.PLAYER_MARRIED;
			FlagDataStorage.SetScriptFlag(true, flags);
			*/

			Main.Log.LogDebug(string.Format("hideLover:{0}, forceNPCID:{1}, MarriedNPCID:{2}, PLAYER_MARRIED:{3}",
				NpcDataManagerPatch.hideLover, NpcDataManagerPatch.forceNPCID,
				SaveData.SaveDataManager.PlayerData.MarriedNPCID,
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
			));
		}

		public static int RandomSpouses()
		{
			int top = -1;
			int[] npcs = new int[(int)Define.NPCID.Max];

			NpcDataManagerPatch.internalState = true;
			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
				if (data.IsSpouses)
					npcs[++top] = data.NpcId;
			NpcDataManagerPatch.internalState = false;

			if (top <= -1)
				return 0;

			return npcs[UnityEngine.Random.Range(0, top)];
		}
	}
}
