using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Reflection;
using System.IO;

public class Util {

    /// <summary>
    /// 取得Lua路径
    /// </summary>
    public static string LuaPath(string name) {
        string path = Application.dataPath ;
        string lowerName = name.ToLower();
        if (lowerName.EndsWith(".lua")) {
            int index = name.LastIndexOf('.');
            name = name.Substring(0, index);
        }
        name = name.Replace('.', '/');
        return path + "/lua/" + name + ".lua";
    }

    public static void Log(string str) {
        Debug.Log(str);
    }

    public static void LogWarning(string str) {
        Debug.LogWarning(str);
    }

    public static void LogError(string str) {
        Debug.LogError(str); 
    }
}