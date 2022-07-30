using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using HarmonyLib;

namespace RF5_Harem
{
	// ADV开始，也许不需要
	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.AdvStart))]
	public class AdvMainStart
	{
		static void Prefix(NpcController npc)
		{
			if(npc != null && npc.NpcId >= 2)
				Relation.SetNPC(npc.NpcId);

			Main.Log.LogDebug(string.Format("AdvMain.AdvStart npcid:{0}", npc?.NpcId));
		}
	}

	// ADV文本，这里可以修改显示的文本
	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.ReadCommand))]
	public class AdvMainReadCommand
	{
		static AdvMain.WorkList LastResult = AdvMain.WorkList.WORK_NONE;

		static void Prefix(AdvMain __instance, NpcController npc)
		{
			if (npc != null && npc.NpcId >= 2)
				Relation.SetNPC(npc.NpcId);

			if (__instance.Cmd != null && __instance.Cmd.Arg != null && __instance.Cmd.ArgText != null)
				Main.Log.LogDebug(string.Format("AdvMain.ReadCommand npcid:{0} cmdid:{1} arg:{2} args:{3}",
					npc?.NpcId, (CommandList)__instance.Cmd.CmdID, string.Join(",", __instance.Cmd.Arg), string.Join(",", __instance.Cmd.ArgText)));
		}

		static void Postfix(AdvMain.WorkList __result)
		{
			LastResult = __result;
			Main.Log.LogDebug(string.Format("AdvMain.ReadCommand result:{0}", __result));
		}

		// 尝试修复异常，异常原因未知
		static Exception Finalizer(Exception __exception, ref AdvMain.WorkList __result)
		{
			__result = LastResult;
			Main.Log.LogDebug(string.Format("AdvMain.ReadCommand Exception:{0}, LastResult:{2}, Stack:{1}", __exception.Message, __exception.StackTrace, __result));
			return null;
		}
	}
}
