using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch]
internal static class TextOverwriteListPatch
{
	[HarmonyPatch(typeof(TextOverwriteList), nameof(TextOverwriteList.TransformText))]
	internal static void Prefix(int speakerId)
	{
		if (speakerId >= NpcDataManagerPatch.MinNPCId)
		{
			Relation.SetNPC(speakerId);
		}

		HaremPlugin.Log.LogDebug($"TextOverwriteList.TransformText npcid:{speakerId}");
	}
}
