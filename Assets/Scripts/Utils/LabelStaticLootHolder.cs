using UnityEngine;

public class LabelStaticLootHolder : LabelHolder
{
    [SerializeField] StaticLootHolder lootHolder;
    [SerializeField] float gizmoMeshSize = .5f;
    [SerializeField] Color gizmoMeshColor = Color.white;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    protected override void DrawGizmo()
    {
        if(lootHolder == null) return;

        if (lootHolder.guaranteedItems.Count == 0) return;

        var gizmoText = "";

        foreach(var i in lootHolder.guaranteedItems)
        {
            gizmoText += $"{i.itemSO.itemName} : {i.quantity}\n";
        }

        GizmoDrawer.DrawWithSphere(
     gizmoMeshColor,
     transform,
     gizmoText,
     gizmoMeshSize,
     fontSize);

    }

}
