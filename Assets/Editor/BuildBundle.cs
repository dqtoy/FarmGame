#if UNITY_EDITOR
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;

public static class BuildBundle
{
    private class BuildBundleCollection
    {
        public HashSet<string> originalPaths = new HashSet<string>();
        public List<string> mainPaths = new List<string>();
        public List<string> bundleNames = new List<string>();
        public HashSet<string> deps = new HashSet<string>();
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

        AssetBundleManifest manifast = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.DeterministicAssetBundle,
            BuildTarget.StandaloneOSX);

        RemoveAllBundleNames();

        // 为了解包
        CreateAssetBundleSummary();

    }

    private static void CreateAssetBundleSummary()
    {

    }

    private static void SetBundleName(string path, string bundleName)
    {
        // 跳过 Resourcesa 文件夹里的资源
        if (path.Contains("/Resources/"))
        {
            return;
        }

        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (!string.IsNullOrEmpty(importer.assetBundleName) && importer.assetBundleName != bundleName)
        {
            throw new System.Exception(string.Format("[AssetBundle] Set [{0}] asset bundle name: [{1}]. But it already have: [{2}]",
                            path, bundleName, importer.assetBundleName));
        }
        else
        {
            importer.assetBundleName = bundleName;
        }

        // unity_builtin_extra
    }

    private static void SetDepBundleName(List<string> deps, string bundleName, BuildBundleCollection collection)
    {
        Dictionary<string, int> originals = new Dictionary<string, int>();
        for (int i = 0; i < deps.Count; i++)
        {
            string depBundleName = "";
            if (!depBundleCaches.TryGetValue(deps[i], out depBundleName))
            {
                depBundleCaches.Add(deps[i], bundleName);
                continue;
            }
            else if (bundleName == depBundleName)
            {
                continue;
            }

            int index;
            if (!originals.TryGetValue(deps[i], out index))
            {
                index = originals.Count;
                originals.Add(deps[i], index);
            }

            string sharedName = bundleName + "_shared_" + index; // 相同索引的存到一个包里
            depBundleCaches[deps[i]] = sharedName;
            collection.bundleNames.Add(sharedName.ToLower());
        }
    }

    private static string TryParseBundleName(string main, BuildBundleCollection collection)
    {
        string bundleName = "";
        if (collection.config.buildType == ENBuildType.AllInOne)
        { // 直接使用指定包名
            bundleName = collection.config.bundleName;
        }
        else if (collection.config.buildType == ENBuildType.ByFolder)
        { // 获取文件夹名
            string folderName = Path.GetDirectoryName(main);
            if (collection.originalPaths.Contains(folderName) || collection.originalPaths.Contains(main))
            {
                folderName = Path.ChangeExtension(main, null);
            }
            bundleName = folderName.Replace("Assets/", "");
        }
        else if (collection.config.buildType == ENBuildType.ByGroups)
        {

        }
        else if (collection.config.buildType == ENBuildType.OneByOne)
        {
            bundleName = Path.ChangeExtension(main, null).Replace("Assets/", null);
        }

        collection.bundleNames.Add(bundleName);

        return bundleName;
    }

    private static List<string> CollectDependenciesPaths(string main, BuildBundleCollection collection)
    {
        string[] dependencies = AssetDatabase.GetDependencies(main, true);
        if (dependencies.Length < 1)
        { // 没依赖项
            return new List<string>();
        }

        List<string> assets = new List<string>();
        foreach (string asset in dependencies)
        {
            if (main == asset || Path.GetExtension(asset) == ".cs" || Path.GetExtension(asset) == ".dll")
            {
                continue;
            }

            BuildBundleCollection result = collections.FindLast((c) =>
            {
                return c.mainPaths.Contains(asset) || c.deps.Contains(asset);
            });

            if (null == result || result == collection)
            {
                assets.Add(asset);
                continue;
            }

            if (result.config.order < collection.config.order)
            {
                continue;
            }

            if (result.config.order >= collection.config.order)
            {
                throw new System.Exception($"{collection.config.bundleName}不能依赖{result.config.bundleName}!");
            }
        }

        // assets.Sort((left, right));

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
        depBundleCaches = new Dictionary<string, string>(); // <资源路径，包名>
        foreach (BuildBundleCollection c in collections)
        {
            foreach (string main in c.mainPaths)
            {
                List<string> deps = CollectDependenciesPaths(main, c);
                foreach (string dep in deps)
                {
                    c.deps.Add(dep);
                }
                SetDepBundleName(deps, TryParseBundleName(main, c), c);
            }
        }

        // 设置依赖项的包名
        foreach (KeyValuePair<string, string> e in depBundleCaches)
        {
            SetBundleName(e.Key, e.Value);
        }

        // 清除没用的包名
        AssetDatabase.RemoveUnusedAssetBundleNames();
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
