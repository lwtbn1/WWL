using UnityEngine;
using System.Collections;
using LuaInterface;
public class LuaScriptManager : MonoBehaviour
{
    string mainLuaScriptPath;
    string mainLuaScript;
    LuaScriptMgr mgr;
    LuaFunction awakeFun;
    LuaFunction startFun;
    LuaFunction updateFun;
    LuaFunction lateUpdateFun;

    // Use this for initialization
    void Awake()
    {
        mgr = new LuaScriptMgr();
        mainLuaScriptPath = Define.MainLuaScriptPath;
        mainLuaScript = FileHelper.ReadLuaScriptFile(mainLuaScriptPath);
        Debug.Log(mainLuaScript);
        object[] ret = mgr.DoString(mainLuaScript);
        LuaTable mainTable = ret[0] as LuaTable;
        awakeFun = mainTable.RawGetFunc("awake");
        startFun = mainTable.RawGetFunc("start");
        updateFun = mainTable.RawGetFunc("update");
        lateUpdateFun = mainTable.RawGetFunc("lateUpdate");

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
}
