using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using BepInEx.IL2CPP;
using BepInEx.Logging;

namespace RF5_Harem
{
	[BepInPlugin(GUID, NAME, VERSION)]
	[BepInProcess(GAME_PROCESS)]
	public class Main : BasePlugin
	{
		#region PluginInfo
		private const string GUID = "C9A56862-DFB2-8BA7-D503-A8DE4D24068B";
		private const string NAME = "RF5_Harem";
		private const string VERSION = "1.0";
		private const string GAME_PROCESS = "Rune Factory 5.exe";
		#endregion

		static public new ManualLogSource Log;

		public override void Load()
		{
			Log = base.Log;
			new Harmony(GUID).PatchAll();
		}
	}
}
