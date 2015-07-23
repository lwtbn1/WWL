using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class ResourceManager : MonoBehaviour
{
    public delegate void OnResourcesLoadedCallback(object param);
    public delegate void BeforeResourcesLoadedCallback();
    public delegate void AfterResourcesLoadedCallback();

    public Dictionary<string, Object> AllLoadedResources = new Dictionary<string, Object>();

    public void StartLoadResources(List<string> allResName, OnResourcesLoadedCallback callBackOn, BeforeResourcesLoadedCallback callBackBefore, AfterResourcesLoadedCallback callBackAfter)
    {
        StartCoroutine(LoadResourceAll(allResName, callBackOn, callBackBefore, callBackAfter));
    }
    IEnumerator LoadResource(string resName, OnResourcesLoadedCallback callBack)
    {
        string resPath = Application.persistentDataPath + "/" + Define.PlatformStr + resName;
        WWW www = new WWW(resPath);
        yield return www;
        if (callBack != null)
            callBack(www.progress);
        if (www.error != null)
        {
            Debug.LogError(www.error);
        }
        if (www.isDone)
        {
            Object resObj = www.assetBundle.mainAsset;
            AllLoadedResources.Add(resName, resObj);
        }
    }

    IEnumerator LoadResourceAll(List<string> allResName, OnResourcesLoadedCallback callBackOn,
        BeforeResourcesLoadedCallback callBackBefore, AfterResourcesLoadedCallback callBackAfter)
    {
        if (callBackBefore != null)
            callBackBefore();

        for (int i = 0; i < allResName.Count; i++)
            yield return StartCoroutine(LoadResource(allResName[i], callBackOn));

        if (callBackBefore != null)
            callBackBefore();
    }
}
