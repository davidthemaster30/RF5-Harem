using HarmonyLib;
using RF5_Harem.Configuration;

namespace RF5_Harem;

[HarmonyPatch]
internal static class TeleportAreaManagerPatch
{
	[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.GetHomeBGMId))]
	internal static bool Prefix(ref BGMID __result)
	{
		switch (SpousesConfig.HomeBGM.Value)
		{
			case 1:
				{
					__result = Relation.RandomSpouses() >= NpcDataManagerPatch.MinNPCId ? BGMID.BGM_HeroHouse_002 : BGMID.BGM_HeroHouse_001;
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

		HaremPlugin.Log.LogDebug($"TeleportAreaManager.GetHomeBGMId bgm:{__result}");
		return false;
	}

	[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.PlayHomeBGM))]
	internal static bool Prefix()
	{
		SoundManager.Instance.PlayBGM(TeleportAreaManager.Instance.GetHomeBGMId());
		HaremPlugin.Log.LogDebug($"TeleportAreaManager.PlayHomeBGM bgm:{TeleportAreaManager.Instance.GetHomeBGMId()}");
		return false;
	}
}
