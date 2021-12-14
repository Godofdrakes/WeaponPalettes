using System;
using System.Collections.Generic;

namespace WeaponPalettes
{
	public class WeaponPalette
	{
		public readonly Dictionary<int, string> ItemUids = new();

		public void ApplyToCharacter(Character character)
		{
			var quickSlotManager = character.QuickSlotMngr;

			for (var index = 0; index < quickSlotManager.QuickSlotCount; ++index)
			{
				quickSlotManager.ClearQuickSlot(index);
			}

			for (var index = 0; index < quickSlotManager.QuickSlotCount; ++index)
			{
				if (!ItemUids.TryGetValue(index, out var itemUid))
					continue;

				var item = ItemManager.Instance.GetItem(itemUid);
				if (item != null)
					quickSlotManager.SetQuickSlot(index, item);
			}
			
			quickSlotManager.RefreshQuickSlots();
		}
	}
}