namespace WeaponPalettes.Model
{
	public struct SavedQuickSlot
	{
		public WeaponSet WeaponSet { get; set; }
		public string ItemUid { get; set; }
		public int ItemId { get; set; }
		public int Index { get; set; }
	}
}