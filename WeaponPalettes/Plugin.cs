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

		public static Plugin Instance
		{
			get => _instance == null ? throw new InvalidOperationException() : _instance;
			private set => _instance = value;
		}
		
		public static PluginSettings Settings { get; private set; }
		
		public static CharacterMap CharacterMap { get; private set; }

		public new ManualLogSource Logger => base.Logger;

		private void Awake()
		{
			Logger.LogInfo($"{PluginInfo.PLUGIN_GUID} loaded");

			Instance = this;

			Settings = new PluginSettings(this);

			Logger.LogDebug($"matchUid: {Settings.MatchUid}");
			Logger.LogDebug($"matchOffhand: {Settings.MatchOffHand}");

			CharacterMap = new CharacterMap();

			Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
		}
	}
}