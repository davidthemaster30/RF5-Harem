using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(TextOverwriteList), nameof(TextOverwriteList.TransformText))]
public class TextOverwriteListPatch
{
	static void Prefix(int speakerId)
	{
		if (speakerId >= 2)
		{
			Relation.SetNPC(speakerId);
		}

		Main.Log.LogDebug($"TextOverwriteList.TransformText npcid:{speakerId}");
	}
}
