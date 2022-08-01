using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace RF5_Harem
{
	// 背景音乐相关
	[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.GetHomeBGMId))]
	public class TeleportAreaManagerGetHomeBGMId
	{
		static bool Prefix(ref BGMID __result)
		{
			switch(MathRF.Clamp(Main.Config.GetInt("Spouses", "HomeBGM", 1), 1, 3))
			{
				case 1:
				{
					if (Relation.RandomSpouses() >= 2)
						__result = BGMID.BGM_HeroHouse_002;
					else
						__result = BGMID.BGM_HeroHouse_001;
					break;
				}
				case 2:
				{
					__result = BGMID.BGM_HeroHouse_001;
					break;
				}
				case 3:
				{
					__result = BGMID.BGM_HeroHouse_002;
					break;
				}
			}

			Main.Log.LogDebug(string.Format("TeleportAreaManager.GetHomeBGMId bgm:{0}", __result));
			return false;
		}
	}

	// 背景音乐相关
	[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.PlayHomeBGM))]
	public class TeleportAreaManagerPlayHomeBGM
	{
		static bool Prefix()
		{
			SoundManager.Instance.PlayBGM(TeleportAreaManager.Instance.GetHomeBGMId());
			Main.Log.LogDebug(string.Format("TeleportAreaManager.PlayHomeBGM bgm:{0}", TeleportAreaManager.Instance.GetHomeBGMId()));
			return false;
		}
	}

	/*
	// 这个不确定会不会出问题，所以就不开启了
	[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.SetSceneBgm))]
	public class TeleportAreaManagerSetSceneBgm
	{
		static void Prefix()
		{
			if (NpcDataManagerPatch.hideSpouse)
				Relation.SetNPC(Relation.RandomSpouses());
		}
	}
	*/
}
