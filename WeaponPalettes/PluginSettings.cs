using BepInEx;
using BepInEx.Configuration;

namespace WeaponPalettes
{
	public readonly struct PluginSettings
	{
		private readonly ConfigEntry<bool> _matchUid;
		private readonly ConfigEntry<bool> _matchMainHand;
		private readonly ConfigEntry<bool> _matchOffHand;

		public PluginSettings(BaseUnityPlugin plugin)
		{
			_matchUid = plugin.Config.Bind(
				"General",
				"matchUid",
				false,
				"True: Weapon sets are matched per weapon (more granular). False: Weapon sets are matched per weapon type (less granular).");

			_matchMainHand = plugin.Config.Bind(
				"General",
				"matchMainHand",
				true,
				"True: Weapon sets are matched based on your mainhand weapon or equipment. False: Weapon sets ignore whatever is in your mainhand.");

			_matchOffHand = plugin.Config.Bind(
				"General",
				"matchOffHand",
				false,
				"True: Weapon sets are matched based on your offhand weapon or equipment. False: Weapon sets ignore whatever is in your offhand.");
		}

		public bool MatchUid => _matchUid.Value;

		public bool MatchMainHand => _matchMainHand.Value;

		public bool MatchOffHand => _matchOffHand.Value;
	}
}