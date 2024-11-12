using HarmonyLib;

namespace RF5_Harem;

[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.GetHomeBGMId))]
public class TeleportAreaManagerGetHomeBGMId
{
	static bool Prefix(ref BGMID __result)
	{
		switch (MathRF.Clamp(Main.SpousesConfig.HomeBGM.Value, 1, 3))
		{
			case 1:
				{
					if (Relation.RandomSpouses() >= 2)
					{
						__result = BGMID.BGM_HeroHouse_002;
					}
					else
					{
						__result = BGMID.BGM_HeroHouse_001;
					}
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

		Main.Log.LogDebug($"TeleportAreaManager.GetHomeBGMId bgm:{__result}");
		return false;
	}
}

[HarmonyPatch(typeof(TeleportAreaManager), nameof(TeleportAreaManager.PlayHomeBGM))]
public class TeleportAreaManagerPlayHomeBGM
{
	static bool Prefix()
	{
		SoundManager.Instance.PlayBGM(TeleportAreaManager.Instance.GetHomeBGMId());
		Main.Log.LogDebug($"TeleportAreaManager.PlayHomeBGM bgm:{TeleportAreaManager.Instance.GetHomeBGMId()}");
		return false;
	}
}
