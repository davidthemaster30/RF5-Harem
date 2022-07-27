using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RF5_Harem
{
	public class Relation
	{
		public static void HideSpouse()
		{
			NpcDataManagerPatch.hideSpouse = true;
			NpcDataManagerPatch.forceNPCID = 0;
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = Define.NPCID.Ares;

			/*
			int[] flags = new int[1];
			flags[0] = (int)Define.GameFlagData.PLAYER_MARRIED;
			FlagDataStorage.SetScriptFlag(false, flags);

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
			// 下面的 IsSpouseNpc 会读取，所以需要放在这里
			NpcDataManagerPatch.hideSpouse = false;
			NpcDataManagerPatch.hideLover = false;
			NpcDataManagerPatch.forceNPCID = 0;

			// 一般来说不建议进入到这里，会导致某些场景出现 bug 的
			if(npcid < 2)
			{
				// 全部隐藏
				HideSpouse();
				NpcDataManagerPatch.hideLover = true;

				Main.Log.LogWarning(string.Format("------ invalid NpcId:{0} ------", npcid));
				foreach (StackFrame frame in new StackTrace().GetFrames())
					Main.Log.LogWarning(string.Format("file:{0}|line:{1}|function:{2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetMethod()));
				Main.Log.LogWarning("------ end ------");
				return;
			}

			if(!NpcDataManager.Instance.IsSpouseNpc(npcid))
			{
				HideSpouse();

				// 伪装成单恋
				if (NpcDataManager.Instance.IsLover(npcid))
					NpcDataManagerPatch.forceNPCID = npcid;
				else // 伪装成单身
					NpcDataManagerPatch.hideLover = true;

				Main.Log.LogDebug(string.Format("*** hideSpouse:{0}, hideLover:{1}, forceNPCID:{2}, MarriedNPCID:{3}, PLAYER_MARRIED:{4}",
					NpcDataManagerPatch.hideSpouse, NpcDataManagerPatch.hideLover,
					NpcDataManagerPatch.forceNPCID, SaveData.SaveDataManager.PlayerData.MarriedNPCID,
					FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
				));
				return;
			}

			NpcDataManagerPatch.forceNPCID = npcid;
			SaveData.SaveDataManager.PlayerData.MarriedNPCID = (Define.NPCID)npcid;

			/*
			SaveData.SaveDataManager.GameSaveData.StampData.GetStampRecord(StampEnum.Marriage).StampLevel = StampLevel.Bronze;

			int[] flags = new int[1];
			flags[0] = (int)Define.GameFlagData.PLAYER_MARRIED;
			FlagDataStorage.SetScriptFlag(true, flags);
			*/

			Main.Log.LogDebug(string.Format("*** hideSpouse:{0}, hideLover:{1}, forceNPCID:{2}, MarriedNPCID:{3}, PLAYER_MARRIED:{4}",
				NpcDataManagerPatch.hideSpouse, NpcDataManagerPatch.hideLover,
				NpcDataManagerPatch.forceNPCID, SaveData.SaveDataManager.PlayerData.MarriedNPCID,
				FlagDataStorage.CheckScriptFlag((int)Define.GameFlagData.PLAYER_MARRIED)
			));
		}

		public static int RandomSpouses()
		{
			int top = -1;
			int[] npcs = new int[(int)Define.NPCID.Max];

			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
				if (data.IsSpouses)
					npcs[++top] = data.NpcId;

			if (top <= -1)
				return 0;

			return npcs[UnityEngine.Random.Range(0, top)];
		}
	}
}
