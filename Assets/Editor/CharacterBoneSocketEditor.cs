using UnityEditor;
using UnityEngine;

/// <summary>
/// Показывает обзор по направлению корпуса и позволяет настроить точку глаз.
/// </summary>
[CustomEditor(typeof(CharacterBoneSocket))]
public class CharacterBoneSocketEditor : Editor
{
    private const float MinimumLineLength = 0.1f;
    private const float LineWidth = 3f;
    private float lineLength = 5f;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Предпросмотр обзора", EditorStyles.boldLabel);
        lineLength = Mathf.Max(MinimumLineLength,
            EditorGUILayout.FloatField("Длина синей линии", lineLength));

        if (!TryGetHead(out _, out _))
        {
            EditorGUILayout.HelpBox(
                "Для предпросмотра нужны назначенный сокет глаз и корректный Humanoid Animator в сервис-локаторе персонажа.",
                MessageType.Warning);
            return;
        }

        EditorGUILayout.HelpBox(
            "Обзор начинается от глаз и направлен вперёд корпуса. Поворот головы и Eyes Socket → Local Rotation не меняют направление FOV. Перемещайте манипулятор в Scene для настройки Eyes Socket → Local Position.",
            MessageType.Info);

        if (GUI.changed) SceneView.RepaintAll();
    }

    private void OnSceneGUI()
    {
        if (!TryGetHead(out Transform bone, out Transform character)) return;

        serializedObject.Update();
        SerializedProperty positionProperty = serializedObject.FindProperty("eyesSocket.localPosition");
        Transform socket = ((CharacterBoneSocket)target).GetEyesSocket;
        Vector3 origin = Application.isPlaying
            ? socket.position
            : bone.TransformPoint(positionProperty.vector3Value);

        var previousDepthTest = Handles.zTest;
        try
        {
            Handles.zTest = UnityEngine.Rendering.CompareFunction.Always;
            using (new Handles.DrawingScope(Color.blue))
            {
                Handles.DrawLine(origin, origin + character.forward * lineLength, LineWidth);
                Handles.Label(origin, "FOV: от глаз вперёд корпуса");
            }

            if (Application.isPlaying) return;

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.PositionHandle(origin, character.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                positionProperty.vector3Value = bone.InverseTransformPoint(newPosition);
                serializedObject.ApplyModifiedProperties();
            }
        }
        finally
        {
            Handles.zTest = previousDepthTest;
        }
    }

    private bool TryGetHead(out Transform bone, out Transform character)
    {
        bone = null;
        character = null;
        var sockets = (CharacterBoneSocket)target;
        var locator = sockets.GetComponentInParent<BaseHumanoidAiServiceLocator>();
        if (locator == null) return false;

        var locatorData = new SerializedObject(locator);
        var animator = locatorData.FindProperty("animator").objectReferenceValue as Animator;
        if (animator == null || animator.avatar == null || !animator.avatar.isValid || !animator.isHuman)
            return false;

        serializedObject.Update();
        if (serializedObject.FindProperty("eyesSocket.socketTransform").objectReferenceValue == null)
            return false;

        int boneIndex = serializedObject.FindProperty("eyesSocket.bone").intValue;
        if (boneIndex < 0 || boneIndex >= (int)HumanBodyBones.LastBone) return false;

        bone = animator.GetBoneTransform((HumanBodyBones)boneIndex);
        var motor = locatorData.FindProperty("motor").objectReferenceValue as BaseHumanoidMotor;
        character = motor != null ? motor.transform : null;
        return bone != null && character != null;
    }
}
