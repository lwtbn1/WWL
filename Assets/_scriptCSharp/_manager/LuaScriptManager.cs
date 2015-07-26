using UnityEngine;
using System.Collections;
using LuaInterface;
using System.Collections.Generic;
public class LuaScriptManager : MonoBehaviour
{
    public Dictionary<string, LuaTable> luaCache;
    string luaScript;
    LuaScriptMgr mgr;
    LuaFunction awakeFun;
    LuaFunction startFun;
    LuaFunction updateFun;
    LuaFunction lateUpdateFun;

    static LuaScriptManager instance;
    public static LuaScriptManager _instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<LuaScriptManager>();
            return instance;
        }
    } 
    // Use this for initialization
    void Awake()
    {
        mgr = new LuaScriptMgr();
        luaCache = new Dictionary<string, LuaTable>();
        luaScript = FileHelper.ReadLuaScriptFile(Define.LuaScriptPathRoot + "/" + "main.lua");
        Debug.Log(luaScript);
        object[] ret = mgr.DoString(luaScript);
        LuaTable mainTable = ret[0] as LuaTable;
        awakeFun = mainTable.RawGetFunc("awake");
        startFun = mainTable.RawGetFunc("start");
        updateFun = mainTable.RawGetFunc("update");
        lateUpdateFun = mainTable.RawGetFunc("lateUpdate");
        LuaTable conf = mainTable.rawget("conf") as LuaTable;
        string[] confStr = conf.ToArray<string>();
        for (int i = 0; i < confStr.Length; i++)
        {
            Debug.Log(Define.LuaScriptPathRoot + "/" + confStr[i] + ".lua");
            luaScript = FileHelper.ReadLuaScriptFile(Define.LuaScriptPathRoot + "/" + confStr[i] + ".lua");
            LuaTable retTable = mgr.DoString(luaScript)[0] as LuaTable;
            luaCache.Add(confStr[i], retTable);
        }
            if (awakeFun != null)
                awakeFun.Call();
    }
    void Start()
    {
        if (startFun != null)
            startFun.Call();

    }

    // Update is called once per frame
    void Update()
    {
        if (updateFun != null)
            updateFun.Call();
    }
    public object CallLuaFunction(string className, string funName, object param = null)
    {
        LuaTable table = luaCache[className];
        LuaFunction fun = table.RawGetFunc(funName);
        return fun.Call(param);
    }
}
