using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.AdvStart))]
	public class AdvMainStart
	{
		static void Prefix(NpcController npc)
		{
			Relation.SetNPC(npc?.NpcId ?? 0);
			Main.Log.LogDebug(string.Format("AdvMain.AdvStart npcid:{0}", npc?.NpcId));
		}
	}

	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.ReadCommand))]
	public class AdvMainReadCommand
	{
		static void Prefix(AdvMain __instance, NpcController npc)
		{
			Relation.SetNPC(npc?.NpcId ?? 0);
			Main.Log.LogDebug(string.Format("AdvMain.ReadCommand npcid:{0} cmdid:{1} arg:{2} args:{3}",
				npc?.NpcId, (CommandList)__instance.Cmd.CmdID, string.Join(",", __instance.Cmd.Arg), string.Join(",", __instance.Cmd.ArgText)));
		}
	}
}
