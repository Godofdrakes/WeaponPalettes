using BepInEx;

namespace WeaponPalettes.Plugin
{
	public record PluginSettings(bool MatchType, bool MatchMainHand, bool MatchOffHand, bool FakeEmptyHand)
	{
		public static PluginSettings Load(BaseUnityPlugin plugin)
		{
			var _matchType = plugin.Config.Bind("General", nameof(MatchType), true, MATCH_TYPE_DESCRIPTION);
			var _matchMainHand = plugin.Config.Bind("General", nameof(MatchMainHand), true, MATCH_MAIN_DESCRIPTION);
			var _matchOffHand = plugin.Config.Bind("General", nameof(MatchOffHand), false, MATCH_OFF_DESCRIPTION);
			var _fakeEmptyHand = plugin.Config.Bind("General", nameof(FakeEmptyHand), true, FAKE_EMPTY_DESCRIPTION);
			return new PluginSettings(_matchType.Value, _matchMainHand.Value, _matchOffHand.Value, _fakeEmptyHand.Value);
		}

		private const string MATCH_TYPE_DESCRIPTION = @"Should items be matched by type (sword, axe, staff, ect.) or by name? Equipment (such as lanterns) are always matched by name.";
		private const string MATCH_MAIN_DESCRIPTION = @"Should weapons sets consider the item equiped in your main hand?";
		private const string MATCH_OFF_DESCRIPTION = @"Should weapons sets consider the item equiped in your off hand?";
		private const string FAKE_EMPTY_DESCRIPTION = @"Should sheathing your weapon act like you have no items equipped in either hand?";
	}
}