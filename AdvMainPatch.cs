using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch]
internal static class AdvMainPatch
{
	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.AdvStart))]
	internal static void Prefix(NpcController npc)
	{
		if (npc is not null && npc.NpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(npc.NpcId);
		}

		Main.Log.LogDebug($"AdvMain.AdvStart npcid:{npc?.NpcId}");
	}


	private static AdvMain.WorkList LastResult = AdvMain.WorkList.WORK_NONE;

	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.ReadCommand))]
	internal static void Prefix(AdvMain __instance, NpcController npc)
	{
		if (npc is not null && npc.NpcId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(npc.NpcId);
		}

		if (__instance.Cmd is not null && __instance.Cmd.Arg is not null && __instance.Cmd.ArgText is not null)
		{
			Main.Log.LogDebug($"AdvMain.ReadCommand npcid:{npc?.NpcId} cmdid:{(CommandList)__instance.Cmd.CmdID} arg:{string.Join(",", __instance.Cmd.Arg)} args:{string.Join(",", __instance.Cmd.ArgText)}");
		}
	}

	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.ReadCommand))]
	internal static void Postfix(AdvMain.WorkList __result)
	{
		LastResult = __result;
		Main.Log.LogDebug($"AdvMain.ReadCommand result:{__result}");
	}

	[HarmonyPatch(typeof(AdvMain), nameof(AdvMain.ReadCommand))]
	internal static Exception Finalizer(Exception __exception, ref AdvMain.WorkList __result)
	{
		__result = LastResult;
		Main.Log.LogDebug($"AdvMain.ReadCommand Exception:{__exception.Message}, LastResult:{__result}, Stack:{__exception.StackTrace}");
		return null;
	}

}