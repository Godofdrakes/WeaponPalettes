using System.Collections.Generic;
using System.Linq;

namespace WeaponPalettes.Model
{
	public class WeaponPalette
	{
		public record ItemData(int ItemId, string ItemUid);

		private Dictionary<int, ItemData> QuickSlots { get; } = new();

		public void Apply(CharacterQuickSlotManager quickSlotManager)
		{
			for (var index = 0; index < quickSlotManager.QuickSlotCount; ++index)
			{
				quickSlotManager.ClearQuickSlot(index);

				if (!QuickSlots.TryGetValue(index, out var item))
					continue;

				var itemDef = ItemManager.Instance.GetItem(item.ItemUid);
				if (itemDef == null)
				{
					itemDef = ResourcesPrefabManager.Instance.GetItemPrefab(item.ItemId);
				}

				quickSlotManager.SetQuickSlot(index, itemDef);
			}

			quickSlotManager.RefreshQuickSlots();
		}

		public bool TryGetItem(int index, out string itemUid)
		{
			if (!QuickSlots.TryGetValue(index, out var item))
			{
				itemUid = string.Empty;
				return false;
			}

			itemUid = item.ItemUid;
			return true;
		}

		public void SetItem(int index, Item? item)
		{
			if (item == null)
			{
				RemoveItem(index);
				return;
			}

			QuickSlots[index] = new ItemData(item.ItemID, item.UID);
		}

		public void SetItem(int index, (int itemId, string itemUid) itemData)
		{
			QuickSlots[index] = new ItemData(itemData.itemId, itemData.itemUid);
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

		public IEnumerable<(int index, int itemId, string itemUid)> Export()
		{
			return QuickSlots.Select(pair => (pair.Key, pair.Value.ItemId, pair.Value.ItemUid));
		}
	}
}