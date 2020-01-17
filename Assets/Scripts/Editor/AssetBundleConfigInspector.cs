#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AssetBundleConfig))]
public class AssetBundleConfigInspector : Editor
{
    private ReorderableList m_List;

    void OnEnable()
    {
        m_List = new ReorderableList(serializedObject, serializedObject.FindProperty("bundles"), true, true, true, true);
        m_List.drawElementCallback += DrawElementCallback;
        m_List.drawElementBackgroundCallback += DrawElementBackgroundCallback;
        m_List.drawHeaderCallback += DrawHeaderCallback;
        m_List.elementHeightCallback += ElementHeightCallback;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        m_List.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }

    public void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
		SerializedProperty element = m_List.serializedProperty.GetArrayElementAtIndex(index);
		Rect position = new Rect(rect.x + 10f, rect.y + 2f, rect.width - 10f, rect.height - 2f);
		EditorGUI.PropertyField(position, element, true);
    }

    public void DrawElementBackgroundCallback(Rect rect, int index, bool isActive, bool isFocused)
    {

    }

    public void DrawHeaderCallback(Rect rect)
    {
		Rect posType = new Rect(rect.x + 15f, rect.y, rect.width, rect.height);
		EditorGUI.LabelField(rect, "Asset Bundle Config");

    }

    private float ElementHeightCallback(int index)
	{
		SerializedProperty element = m_List.serializedProperty.GetArrayElementAtIndex(index);
		return element.isExpanded ? element.CountInProperty() * 18f : 20f;
	}
}
#endif
