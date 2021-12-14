using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace WeaponPalettes
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		private static Plugin _instance;

		private ConfigEntry<bool> _matchUid;
		private ConfigEntry<bool> _matchOffhand;

		public static bool MatchUid => Instance._matchUid.Value;
		public static bool MatchOffhand => Instance._matchOffhand.Value;

		public static Plugin Instance
		{
			get => _instance == null ? throw new InvalidOperationException() : _instance;
			private set => _instance = value;
		}

		public new ManualLogSource Logger => base.Logger;

		private void Awake()
		{
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

			Instance = this;

			_matchUid = Config.Bind(
				"General",
				"matchUid",
				false,
				"True: Weapon sets are matched per weapon (more granular). False: Weapon sets are matched per weapon type (less granular).");

			_matchOffhand = Config.Bind(
				"General",
				"matchOffhand",
				false,
				"True: Weapon sets are matched using both secondary and primary weapons (more granular). False: Weapon sets are matched using only the primary weapon (less granular).");

			Logger.LogDebug($"matchUid: {_matchUid.Value}");
			Logger.LogDebug($"matchOffhand: {_matchOffhand.Value}");

			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
		}
	}
}