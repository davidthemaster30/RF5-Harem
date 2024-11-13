using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(TextOverwriteList), nameof(TextOverwriteList.TransformText))]
internal static class TextOverwriteListPatch
{
	internal static void Prefix(int speakerId)
	{
		if (speakerId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(speakerId);
		}

		Main.Log.LogDebug($"TextOverwriteList.TransformText npcid:{speakerId}");
	}
}
