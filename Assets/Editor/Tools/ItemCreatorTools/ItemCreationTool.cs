using UnityEditor;
using UnityEngine;

public class ItemCreationTool : EditorWindow
{
    private ConsumableItemsCreatorTool consumableItemsTool;
    private SpellProjectileCreatorTool spellTool;
    private WeaponCreatorTool weaponTool;
    private ShieldCreatorTool shieldTool;

    private int selectedTab;
    private readonly string[] tabs = { "Consumables", "Spells", "Weapons", "Shields" };
    private Vector2 tabScrollPos;

    [MenuItem("Tools/Items Tools/Items Creator")]
    public static void Open() => GetWindow<ItemCreationTool>("Items Creator");

    private void OnEnable()
    {
        consumableItemsTool = CreateInstance<ConsumableItemsCreatorTool>();
        spellTool = CreateInstance<SpellProjectileCreatorTool>();
        weaponTool = CreateInstance<WeaponCreatorTool>();
        shieldTool = CreateInstance<ShieldCreatorTool>();
    }

    private void OnGUI()
    {
        tabScrollPos = EditorGUILayout.BeginScrollView(tabScrollPos, GUILayout.Height(50), GUILayout.ExpandWidth(true));
        selectedTab = GUILayout.Toolbar(selectedTab, tabs, GUILayout.Height(25));
        EditorGUILayout.EndScrollView();
        GUILayout.Space(5);

        switch (selectedTab)
        {
            case 0:
                consumableItemsTool.DrawWindow();
                break;
            case 1:
                spellTool.DrawWindow();
                break;
            case 2:
                weaponTool.DrawWindow();
                break;
            case 3:
                shieldTool.DrawWindow();
                break;
        }
    }
}