using UnityEngine;
using UnityEditor;
using System.IO;

public class FileNameFormatterTool
{
    [MenuItem("Tools/Rename Spaces To _")]
    public static void Rename()
    {
        Object selected = Selection.activeObject;

        if (selected == null)
        {
            Debug.LogError("Выбери папку.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(selected);

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogError("Нужно выбрать папку.");
            return;
        }

        string[] assetGuids = AssetDatabase.FindAssets("", new[] { folderPath });

        int renamed = 0;

        foreach (string guid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            if (AssetDatabase.IsValidFolder(assetPath))
                continue;

            string fileName = Path.GetFileName(assetPath);

            if (!fileName.Contains(" "))
                continue;

            string newFileName = fileName.Replace(" ", "_");

            string error = AssetDatabase.RenameAsset(assetPath, newFileName);

            if (string.IsNullOrEmpty(error))
            {
                renamed++;
                Debug.Log($"Renamed: {fileName} -> {newFileName}");
            }
            else
            {
                Debug.LogError($"Ошибка rename {assetPath}: {error}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Готово. Переименовано: {renamed}");
    }
}