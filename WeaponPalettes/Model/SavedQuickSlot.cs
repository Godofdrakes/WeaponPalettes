namespace WeaponPalettes.Model
{
	public struct SavedQuickSlot
	{
		public WeaponSet WeaponSet { get; }
		public string Item { get; }
		public int Index { get; }

		public SavedQuickSlot(WeaponSet weaponSet, string item, int index)
		{
			this.WeaponSet = weaponSet;
			this.Item = item;
			this.Index = index;
		}
	}
}