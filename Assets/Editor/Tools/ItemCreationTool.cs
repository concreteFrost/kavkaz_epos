using UnityEditor;
using UnityEngine;

public class ItemCreationTool : EditorWindow
{
    private int selectedTab;
    private string[] tabs = { "Stat Items", "Weapon Items", "Points Emitter Items" };

    private StatModifierItemsCreatorTool statTool;
    private WeaponModifierItemsCreatorTool weaponTool;
    private PointsEmitterItemsCreatorTool pointsEmitterTool;

    [MenuItem("Tools/Items Viewer/Items Creator")]
    public static void Open() => GetWindow<ItemCreationTool>("Items Creator");

    private void OnEnable()
    {
        statTool = CreateInstance<StatModifierItemsCreatorTool>();
        weaponTool = CreateInstance<WeaponModifierItemsCreatorTool>();
        pointsEmitterTool = CreateInstance<PointsEmitterItemsCreatorTool>();
    }

    private void OnGUI()
    {
        selectedTab = GUILayout.Toolbar(selectedTab, tabs);
        GUILayout.Space(5);

        switch (selectedTab)
        {
            case 0:
                statTool.DrawWindow();
                break;
            case 1:
                weaponTool.DrawWindow();
                break;
            case 2:
                pointsEmitterTool.DrawWindow();
                break;
        }
    }
}