using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 角色昵称
	[HarmonyPatch(typeof(TextOverwriteList), nameof(TextOverwriteList.TransformText))]
	public class TextOverwriteListPatch
	{
		static void Prefix(int speakerId)
		{
			if(speakerId >= 2)
				Relation.SetNPC(speakerId);

			Main.Log.LogDebug(string.Format("TextOverwriteList.TransformText npcid:{0}", speakerId));
		}
	}
}
