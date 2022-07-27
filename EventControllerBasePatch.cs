using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnhollowerBaseLib;

namespace RF5_Harem
{
	// 防止已婚告白被拒
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.RunScript))]
	public class EventControllerBaseScript
	{
		static void Prefix(EventControllerBase __instance)
		{
			if(__instance.TargetNpcId >= 2)
				Relation.SetNPC(__instance.TargetNpcId);
			Main.Log.LogDebug(string.Format("EventControllerBase.RunScript npcid:{0}", __instance.TargetNpcId));
		}
	}

	// 对话前置检查
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.GetNpcTalkType))]
	public class EventControllerBaseTalkType
	{
		static void Prefix(EventControllerBase __instance)
		{
			if(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= 2)
				Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
			else if (__instance.TargetNpcId >= 2)
				Relation.SetNPC(__instance.TargetNpcId);

			Main.Log.LogDebug(string.Format("EventControllerBase.GetNpcTalkType npcid:{0} evnpcid:{1}",
				__instance.TargetNpcId,
				SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId
			));
		}
	}

	// 对话时的选择菜单
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.SelectedMenuGroup))]
	public class EventControllerBaseSelectedMenu
	{
		static void Prefix(EventControllerBase __instance)
		{
			if (SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId >= 2)
				Relation.SetNPC(SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId);
			else if (__instance.TargetNpcId >= 2)
				Relation.SetNPC(__instance.TargetNpcId);

			Main.Log.LogDebug(string.Format("EventControllerBase.SelectedMenuGroup npcid:{0} evnpcid:{1}",
				__instance.TargetNpcId,
				SaveData.SaveDataManager.GameSaveData.EventData.EventSaveParameter.TargetNpcId
			));
		}
	}

	// 阻止重置其他NPC的数据
	[HarmonyPatch(typeof(EventControllerBase), nameof(EventControllerBase.MarriageInit))]
	public class EventControllerBaseMarriageInit
	{
		static bool Prefix()
		{
			return false;
		}
	}
}
