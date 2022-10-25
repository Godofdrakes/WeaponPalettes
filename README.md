# Weapon Palettes

A mod for Outward to enable per-weapon quickslots. I like the idea of multiple quickslot bars but really don't want to have to cycle between quickslot bars mid-combat so I built something more intuitive. This mod will automatically switch quickslot bars based on your currently equipped weapons.

# Install

Use [Thunderstore](https://outward.thunderstore.io/package/Godofdrakes/WeaponPalettes/)

# Examples

- Weapon Switching
  1. Equip a sword
  2. Assign a bow to your hotbar
  3. Equip a bow
  4. Assign a sword to the quickslot your bow was in
  - This button now acts as a "switch weapon" button, toggling between the two weapon types.
- Magic/Melee Toggle
  1. Enable `matchOffHand` and disable `matchMainHand`.
  2. Equip a sword and a shield.
  3. Assign a lexicon to your quickbar
  4. Assign any other skills/items you want to your quickslots
  5. Equip your lexicon
  6. Assign your shield to the quickslot your lexicon was in
  7. Assign any runes to your quickslots
  - This button now acts as a "toggle magic" button, granting easy access to runes by simply equiping your lexicon

# Config

Thee are a few config settings that let you tweak how the mod tracks your weapon set

- `MatchType`
  - When enabled weapons of the same type (1H Swords, Polearms, Axes, ect.) will use the same set of quickslots
  - Defaults to enabled
- `MatchMainHand`
  - When enabled the item in your main hand will be considered when deciding which set of quickslots to load
  - Defaults to enabled
- `MatchOffHand`
  - When enabled the item in your off hand will be considered when deciding which set of quickslots to load
  - This can be enabled in addition to MatchMainHand
  - Defaults to disabled
- `FakeEmptyHand`
  - When enabled if you sheathe your weapon it will treat your main hand as empty, loading the corresponding set of quickslots
  - Defaults to enabled
