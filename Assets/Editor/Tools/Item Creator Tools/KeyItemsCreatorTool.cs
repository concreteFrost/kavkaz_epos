using UnityEditor;

public class KeyItemsCreatorTool : BaseItemCreatorTool<KeyItemSO>
{
    protected override string ItemFolder => $"{basePath}/KeyItems/";


    // Полный контент при раскрытии
    protected override void DrawItem(KeyItemSO item)
    {
        if (item == null) return;

        if (!serializedCache.TryGetValue(item, out var so) || so.targetObject == null)
        {
            so = new SerializedObject(item);
            serializedCache[item] = so;
        }

        so.Update();


        so.ApplyModifiedProperties();
    }
}