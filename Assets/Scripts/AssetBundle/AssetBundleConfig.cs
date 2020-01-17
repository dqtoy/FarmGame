#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ENBuildType
{
    enAllInOne,
    enOneByOne,
    enByFolder,
    enByGroups,
}

public class AssetBundleConfig : ScriptableObject
{
    public List<AssetBundleConfigItem> bundles = new List<AssetBundleConfigItem>();
}

[System.Serializable]
public class AssetBundleConfigItem
{
    public string bundleName;
    public string filter;
    public ENBuildType buildType;
    public Object[] assets;

    public HashSet<string> GetOriginalPaths()
    {
        HashSet<string> results = new HashSet<string>();
        for (int index = 0; index < assets.Length; index++)
        {
            results.Add(AssetDatabase.GetAssetPath(assets[index]));
        }
        return results;
    }

    public List<string> GetMainPaths()
    {
        List<string> results = new List<string>();
        List<string> folders = new List<string>();
        for (int index = 0; index < assets.Length; index++)
        {
            string fullPath = AssetDatabase.GetAssetPath(assets[index]);
            if (Directory.Exists(fullPath))
            {
                folders.Add(fullPath);
                continue;
            }

            if (Path.GetExtension(fullPath) == filter)
            {
                results.Add(fullPath);
                continue;
            }
        }

        if (folders.Count <= 0)
        {
            return results;
        }

        string[] guids = AssetDatabase.FindAssets(filter, folders.ToArray());
        for (int index = 0; index < guids.Length; index++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[index]);
            results.Add(path);
        }

        if(buildType != ENBuildType.enByGroups)
        {
            return results;
        }

        // groups

        return results;
    }
}
#endif