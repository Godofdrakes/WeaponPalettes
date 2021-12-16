# Weapon Palettes

A mod for Outward to enable per-weapon quickslots. I like the idea of multiple quickslot bars but really don't want to have to cucle between quickslot bars mid-combat so I built something more intuitive. This mod will automatically switch quickslot bars based on your currently equipped weapons.

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

- `matchUid`
  - By default the mod considers different weapons of the same category (swords, bows, ect.) as the same weapon set. This makes setting up the quickbar simpler as finding a new sword doesn't then require reassigning your entire quickbar. Enabling this setting will cause weapons in the same category to be treated as different weapon sets.
- `matchMainHand` / `matchOffHand`
  - By default the mod only pays attention to your main hand weapon, ignoring your off hand item. This allows you to cycle between lanterns, torches, shields, and other off hand items without having to set up your quickslot bar for each. If you'd prefer your quickslot bar also (or only) be based on your off hand item you can change these values.
