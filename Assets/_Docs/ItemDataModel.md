# Item data and typed inventories

ItemData stores the shared serialized fields itemSO, quantity, instanceId, isEquiped, durability and weaponLevel. WeaponData, ShieldData, CombatItemData, SpellData and ConsumableData remain its specialized runtime types. Weapon damage/upgrades and item statistics remain on the appropriate subclasses. BaseInventory<TItem> retains its generic constraint and typed items, quick slots and methods.

Static loot and reward definitions use List<ItemData>. LootItemEntry is removed. ItemData.CreateInstance creates the appropriate runtime subtype from itemSO and copies authored state, while assigning a fresh instance ID and clearing equipped state. Existing typed instances retain their subtype when copied. Loot, rewards and dynamic drops share this conversion, so typed inventories receive the correct data without modifying definitions.

StaticLootHolder retains the serialized name lootEntries. The old guaranteedItems lists were explicitly migrated in the three loot prefabs. Thirty-six obsolete guaranteedItems.Array.size=0 scene overrides were removed; the FormerlySerializedAs alias was removed because it allowed these old empty overrides to conflict with populated lootEntries. Existing item references, quantities and level/durability overrides were preserved. Copies before that migration are in the session temporary EposLootRestore/before directory.

Static loot initialization is idempotent. Loading collected/uncollected state hides or restores the holder respectively. Dynamic loot saves retain instance ID, durability and weapon level; new fields are optional for older BinaryFormatter saves. InventoryItemSaveData and WeaponSaveData keep their original types and field layouts.

Validation after restoring the typed architecture:
- Complete runtime and editor source sets compile against project Unity/plugin assemblies without errors.
- Compared hashes for all 992 scene/prefab/asset files before and after this correction: unchanged.
- Isolated checks use the actual ItemData and QuestSO sources with Unity API stubs to verify typed creation, inheritance, quantity/level/durability preservation, independent copies, fresh IDs, upgrades and invalid reward rejection.
- Unity scene deserialization and Play Mode remain to be verified in the editor.
