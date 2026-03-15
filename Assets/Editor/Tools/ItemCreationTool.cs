using UnityEditor;
using UnityEngine;

public class ItemCreationTool : EditorWindow
{
    private int selectedTab;
    private string[] tabs = { "Stat Modifier Items", "Weapon Modifier Items", "Points Emitter Items","Spells","Weapons" };

    private StatModifierItemsCreatorTool statModifierItemsTool;
    private WeaponModifierItemsCreatorTool weaponModifierItems;
    private PointsEmitterItemsCreatorTool pointsEmitterTool;
    private SpellProjectileCreatorTool spellTool;
    private WeaponCreatorTool weaponTool;

    [MenuItem("Tools/Items Viewer/Items Creator")]
    public static void Open() => GetWindow<ItemCreationTool>("Items Creator");

    private void OnEnable()
    {
        statModifierItemsTool = CreateInstance<StatModifierItemsCreatorTool>();
        weaponModifierItems = CreateInstance<WeaponModifierItemsCreatorTool>();
        pointsEmitterTool = CreateInstance<PointsEmitterItemsCreatorTool>();
        spellTool = CreateInstance<SpellProjectileCreatorTool>();
        weaponTool = CreateInstance<WeaponCreatorTool>();
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);
        GUILayout.Space(5);

        switch (selectedTab)
        {
            case 0:
                statModifierItemsTool.DrawWindow();
                break;
            case 1:
                weaponModifierItems.DrawWindow();
                break;
            case 2:
                pointsEmitterTool.DrawWindow();
                break;
            case 3:
                spellTool.DrawWindow();
                break;
            case 4:
                weaponTool.DrawWindow();
                break;
        }
    }
}