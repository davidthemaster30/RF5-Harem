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
		static int LastPoll = 0;
		
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

			if(npcid < 2)
			{
				// 全部隐藏
				HideSpouse();
				NpcDataManagerPatch.hideLover = true;

				/*
				Main.Log.LogWarning(string.Format("------ invalid NpcId:{0} ------", npcid));
				foreach (StackFrame frame in new StackTrace().GetFrames())
					Main.Log.LogWarning(string.Format("file:{0}|line:{1}|function:{2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetMethod()));
				Main.Log.LogWarning("------ end ------");
				*/
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
			NpcData[] npcs = new NpcData[(int)Define.NPCID.Max];
			foreach (NpcData data in NpcDataManager.Instance.NpcDatas)
				if (data.IsSpouses)
					npcs[++top] = data;

			if (top < 0)
				return 0;

			/*
			for (int i = 0; i <= top; ++i)
				Main.Log.LogInfo(string.Format("Spouses[{0}/{1}] npcid:{2}({3}), Home:{4}, BedPatrol:{5}, NickName:{6}, PlayerNickName:{7}",
					i, top, npcs[i].NpcId, (Define.NPCID)npcs[i].NpcId,
					npcs[i].Home,
					npcs[i].BedPatrolPointName,
					npcs[i].NickNameFromPlayerId,
					npcs[i].NickNameToPlayerId
				));
			*/

			int choose;
			if (Main.Config.GetBool("Spouses", "Alternation", true))
				choose = Math.Abs(LastPoll++ % (top + 1));
			else
				choose = new Random(TimeManager.Instance.ElapsedTime).Next(0, top);

			// Main.Log.LogInfo(string.Format("Spouses Hit:{0}, npcid:{1}({2})", choose, npcs[choose].NpcId, (Define.NPCID)npcs[choose].NpcId));
			return npcs[choose].NpcId;
		}
	}
}
