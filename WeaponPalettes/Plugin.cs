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
	[BepInDependency("com.sinai.SideLoader", "3.5.5")]
	public class Plugin : BaseUnityPlugin
	{
		private static Plugin _instance;
		private static CharacterMap _characterMap;

		public static Plugin Instance
		{
			get => _instance ? _instance : throw Util.InvalidOperation();
			private set => _instance = value;
		}
		
		public static PluginSettings Settings { get; private set; }

		public static CharacterMap CharacterMap
		{
			get => _characterMap ?? throw Util.InvalidOperation();
			private set => _characterMap = value;
		}

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