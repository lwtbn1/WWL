using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
public class AssetBundleUtil
{

    [MenuItem("Custom Editor/Create AssetBunldes")]
    public static void CreateAssetBundles()
    {
        string streamingAssetsFolder = Application.dataPath + "/StreamingAssets/" + Define.PlatformStr + "/";

        if (!Directory.Exists(streamingAssetsFolder))
        {
            Debug.Log(streamingAssetsFolder + " 目录创建成功.....");
            Directory.CreateDirectory(streamingAssetsFolder);
        }

        List<string> prefabFullPath = new List<string>();
        FileHelper.GetAllFileFullPath(Define.PrefabsRootPath, ref prefabFullPath, ".prefab");
        for (int i = 0; i < prefabFullPath.Count; i++)
        {
            string fullPath = prefabFullPath[i];
            Object asset = AssetDatabase.LoadMainAssetAtPath(fullPath.Substring(fullPath.IndexOf("Assets"), fullPath.Length - fullPath.IndexOf("Assets")));

            string pathName = streamingAssetsFolder + fullPath.Substring(fullPath.IndexOf("_prefabs"), fullPath.LastIndexOf(".") - fullPath.IndexOf("_prefabs")) + ".unity3d";
            Debug.Log(pathName);
            string dir = pathName.Substring(0, pathName.LastIndexOf("\\"));
            Debug.Log(dir);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            BuildPipeline.BuildAssetBundle(asset, null, pathName, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, Define.buildTarget);
        }
        AssetDatabase.Refresh();
    }




}



