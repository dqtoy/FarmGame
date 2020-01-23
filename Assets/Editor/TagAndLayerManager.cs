#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TagAndLayerManager
{
    [MenuItem("Tools/TagsAndLayers/GenerateSortingLayer")]
    public static void AddSortingLayer()
    {
        List<string> layers = new List<string>();
        foreach (int v in Enum.GetValues(typeof(ENScreenPriority)))
        {
            layers.Add(Enum.GetName(typeof(ENScreenPriority), v));
        }

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        if (null == tagManager)
        {
            Debug.LogError("序列化失败");
            return;
        }

        SerializedProperty it = tagManager.GetIterator();
        while (it.NextVisible(true))
        {
            if (it.name != "m_SortingLayers")
            {
                continue;
            }

            while (it.arraySize > 0)
            {
                it.DeleteArrayElementAtIndex(0);
            }

            for (int index = 0; index < layers.Count; index++)
            {
                it.InsertArrayElementAtIndex(index);
                SerializedProperty dataPoint = it.GetArrayElementAtIndex(index);
                while(dataPoint.NextVisible(true))
                {
                    if(dataPoint.name == "name")
                    {
                        dataPoint.stringValue = layers[index];
                    }
                    else if(dataPoint.name == "uniqueID")
                    {
                        dataPoint.intValue = (int)Enum.Parse(typeof(ENScreenPriority),layers[index]);
                    }
                }
            }
        }
        
        tagManager.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }
}
#endif