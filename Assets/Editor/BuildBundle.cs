
#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class BuildBundle
{
    private class BuildBundleCollection
    {
        public HashSet<string> originalPaths;
        public List<string> mainPaths;
        public List<string> bundleNames;
        public HashSet<string> deps;
        public AssetBundleConfigItem config;

    }

    public static string outputPath = $"{Application.dataPath}/../FarmGameAssetBundle";
    public static AssetBundleConfig buildConfig;
    public static AssetBundleConfig BuildConfig
    {
        get
        {
            return buildConfig ?? AssetDatabase.LoadAssetAtPath<AssetBundleConfig>("Assets/Editor/BuildBundle.asset");
        }
    }
    private static List<BuildBundleCollection> collections;
    private static Dictionary<string, string> depBundleCaches;

    [MenuItem("Tools/BuildBundle/Build")]
    public static void Build()
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        RemoveAllBundleNames();
        SetAllBundleNames();

        // AssetBundleBuild[] results = CollectBundleBuilds();
        AssetBundleManifest manifast = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.DeterministicAssetBundle,
            BuildTarget.StandaloneOSX);

    }

    private static AssetBundleBuild[] CollectBundleBuilds()
    {
        return null;
    }

    private static void SetBundleName(string path, string bundleName)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (!string.IsNullOrEmpty(importer.assetBundleName) && importer.assetBundleName != bundleName)
        {
            // error
        }
        else
        {
            importer.assetBundleName = bundleName;
        }

    }

    private static void SetDepBundleName(List<string> deps, string bundleName)
    {

    }

    private static string TryParseBundleName(string main, BuildBundleCollection collection)
    {
        string bundleName = "";
        if (collection.config.buildType == ENBuildType.enAllInOne)
        {
            bundleName = collection.config.bundleName;
        }
        else if (collection.config.buildType == ENBuildType.enByFolder)
        {
            string folderName = Path.GetDirectoryName(main);
            if (collection.originalPaths.Contains(folderName) || collection.originalPaths.Contains(main))
            {
                folderName = Path.ChangeExtension(main, null);
            }
            bundleName = folderName.Replace("Assets/", "");
        }
        else if (collection.config.buildType == ENBuildType.enByGroups)
        {

        }
        else if (collection.config.buildType == ENBuildType.enOneByOne)
        {
            bundleName = Path.ChangeExtension(main, null).Replace("Assets/", null);
        }

        collection.bundleNames.Add(bundleName);

        return bundleName;
    }

    private static List<string> CollectDependenciesPaths(string main)
    {
        string[] dependencies = AssetDatabase.GetDependencies(main, true);
        if(dependencies.Length < 1)
        { // 没依赖项
            return new List<string>();
        }
        
        List<string> assets = new List<string>();
        foreach (string asset in dependencies)
        {
            if(main == asset || Path.GetExtension(asset) == ".cs" || Path.GetExtension(asset) == ".dll")
            {
                continue;
            }

            
        }

        return assets;
    }

    private static void SetAllBundleNames()
    {
        collections = new List<BuildBundleCollection>();
        foreach (AssetBundleConfigItem config in BuildConfig.bundles)
        {
            BuildBundleCollection c = new BuildBundleCollection();
            c.config = config;
            collections.Add(c);
        }

        // original path // 外层资源的路径
        foreach (BuildBundleCollection c in collections)
        {
            c.originalPaths = c.config.GetOriginalPaths();
        }

        // main path // 具体资源路径
        foreach (BuildBundleCollection c in collections)
        {
            c.mainPaths = c.config.GetMainPaths();
        }

        // 通过具体资源路径，先设置包名，后处理依赖项相关的
        foreach (BuildBundleCollection c in collections)
        {
            foreach (string main in c.mainPaths)
            {
                SetBundleName(main, TryParseBundleName(main, c));
            }
        }

        // 处理依赖项
        depBundleCaches = new Dictionary<string, string>();//<资源路径，包名>
        foreach (BuildBundleCollection c in collections)
        {
            foreach (string main in c.mainPaths)
            {
                List<string> deps = CollectDependenciesPaths(main);
                foreach (string dep in deps)
                {
                    c.deps.Add(dep); // ?? 有什么用 
                }
                SetDepBundleName(deps, TryParseBundleName(main, c));
            }
        }
    }

    private static void RemoveAllBundleNames()
    {
        string[] names = AssetDatabase.GetAllAssetBundleNames();
        for (int index = 0; index < names.Length; index++)
        {
            AssetDatabase.RemoveAssetBundleName(names[index], true);
        }
    }
}
#endif
