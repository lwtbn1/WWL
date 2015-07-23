using UnityEngine;
using System.Collections;
using UnityEditor;

public class Define
{
    public static readonly string PlatformStr =
#if UNITY_ANDROID
 "android";
#elif UNITY_IPHONE
		"ios";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "win32";
#else
        string.Empty;
#endif

    public static readonly string PathURL =
#if UNITY_ANDROID
 "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    public static readonly string PrefabsRootPath = Application.dataPath + "/_prefabs/";

    public static BuildTarget buildTarget =
#if UNITY_ANDROID
 BuildTarget.Android;
#elif UNITY_IPHONE
		BuildTarget.iPhone;
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        BuildTarget.StandaloneWindows;
#else
        BuildTarget.StandaloneWindows;
#endif

    public static string StreamingAssetsRootByPlatForm = Application.dataPath + "/StreamingAssets/" + Define.PlatformStr + "/";
    public static string StreamingAssetsRoot = Application.dataPath + "/StreamingAssets/";
    public static string VersionFileFullPath = Application.persistentDataPath + "/" + PlatformStr + "/" + "version.dat";
    public static string VersionFileName = PlatformStr + "/" + "version.dat";

    public struct VersionInfo
    {
        public string fileName;
        public string md5;

    }
}

