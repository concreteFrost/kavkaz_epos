using UnityEditor;
using UnityEngine;

public class ItemCreationTool : EditorWindow
{
    private StatModifierItemsCreatorTool statModifierItemsTool;
    private WeaponModifierItemsCreatorTool weaponModifierItems;
    private PointsEmitterItemsCreatorTool pointsEmitterTool;
    private SpellProjectileCreatorTool spellTool;
    private WeaponCreatorTool weaponTool;
    private ShieldCreatorTool shieldTool;

    private int selectedTab;
    private string[] tabs = { "Stat Modifier Items", "Weapon Modifier Items", "Points Emitter Items","Spells","Weapons","Shields" };
    private Vector2 tabScrollPos; // добавляем поле для прокрутки
   

    [MenuItem("Tools/Items Tools/Items Creator")]
    public static void Open() => GetWindow<ItemCreationTool>("Items Creator");

    private void OnEnable()
    {
        statModifierItemsTool = CreateInstance<StatModifierItemsCreatorTool>();
        weaponModifierItems = CreateInstance<WeaponModifierItemsCreatorTool>();
        pointsEmitterTool = CreateInstance<PointsEmitterItemsCreatorTool>();
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
            case 5:
                shieldTool.DrawWindow();
                break;  
        }
    }
}