using UnityEditor;

public class QuestItemsCreatorTool : BaseItemCreatorTool<QuestItemSO>
{
    protected override string ItemFolder => $"{basePath}/QuestItems/";


    // Полный контент при раскрытии
    protected override void DrawItem(QuestItemSO item)
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