namespace WeaponPalettes
{
	public static class CharacterEx
	{
		public static bool IsValidLocalPlayer(this Character? character) =>
			character != null && character.IsLocalPlayer;
	}
}