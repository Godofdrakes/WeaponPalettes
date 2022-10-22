using System.Collections.Generic;
using System.Linq;

namespace WeaponPalettes.Model
{
	public class WeaponPalette
	{
		private Dictionary<int, string> QuickSlots { get; } = new();

		public void Apply(CharacterQuickSlotManager quickSlotManager)
		{
			for (var index = 0; index < quickSlotManager.QuickSlotCount; ++index)
			{
				quickSlotManager.ClearQuickSlot(index);

				if (!QuickSlots.TryGetValue(index, out var item))
					continue;

				var itemDef = ItemManager.Instance.GetItem(item);
				if (item != null)
					quickSlotManager.SetQuickSlot(index, itemDef);
			}

			quickSlotManager.RefreshQuickSlots();
		}

		public bool TryGetItem(int index, out string item)
		{
			if (!QuickSlots.TryGetValue(index, out item))
			{
				item = string.Empty;
				return false;
			}

			return true;
		}

		public void SetItem(int index, string? item)
		{
			if (string.IsNullOrEmpty(item))
			{
				RemoveItem(index);
				return;
			}

			QuickSlots[index] = item!;
		}

		public void RemoveItem(int index)
		{
			QuickSlots.Remove(index);
		}

		public void ResetItems()
		{
			foreach (var pair in QuickSlots)
			{
				QuickSlots.Remove(pair.Key);
			}
		}

		public IEnumerable<(int index, string item)> Export()
		{
			return QuickSlots.Select(pair => (pair.Key, pair.Value));
		}
	}
}