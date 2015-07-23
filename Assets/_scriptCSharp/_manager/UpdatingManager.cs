using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
public class UpdatingManager : MonoBehaviour
{

    delegate void OnDownloadOneFileDelegate(object parma);          //下载资源过程中回调函数
    delegate void BeforeDownloadOneFileDelegate(object parma);          //下载资源之前回调函数
    delegate void AfterDownloadOneFileDelegate(object parma);           //下载资源之后回调函数
    delegate void AfterDownloadDelegate(object parma);           //下载资源之后回调函数

    // Use this for initialization
    void Start()
    {
        StartCoroutine(UpdateStart());
    }


    IEnumerator UpdateStart()
    {
        List<string> needToDownloadFileList = new List<string>();
        yield return StartCoroutine(CheckDownloadFiles(needToDownloadFileList));

        StartCoroutine(StartToDownLoad(needToDownloadFileList, AfterDownloadOneFile, AfterDownload));
    }
    IEnumerator CheckDownloadFiles(List<string> needToDownloadFileList)
    {
        Dictionary<string, string> oldList = new Dictionary<string, string>();
        Dictionary<string, string> newList = new Dictionary<string, string>();
        if(File.Exists(Define.VersionFileFullPath))
            FileHelper.ReadVersionFile(oldList);

        yield return StartCoroutine(DownLoadVersionFileFromServer(Define.VersionFileName, null));

        if(File.Exists(Define.VersionFileFullPath))
            FileHelper.ReadVersionFile(newList);

        foreach (KeyValuePair<string, string> kv in newList)
        {
            string fileName = kv.Key;
            string md5 = kv.Value;
            if ((oldList.ContainsKey(fileName) && !oldList[fileName].Equals(md5)) || !oldList.ContainsKey(fileName))
            {
                Debug.Log("will downloading : " + fileName.Replace("\\", "/"));
                needToDownloadFileList.Add(fileName.Replace("\\", "/"));
            }
                
        }
        
    }
    IEnumerator StartToDownLoad(List<string> needToDownloadFile, AfterDownloadOneFileDelegate afterDownloadOneFile, AfterDownloadDelegate afterDownload)
    {
        for (int i = 0; i < needToDownloadFile.Count; i++)
        {
            yield return StartCoroutine(DownLoadFromServer(needToDownloadFile[i], OnDownloadOneFile));
            afterDownloadOneFile(null);
        }
        yield return new WaitForSeconds(0.1f);
        afterDownload(null);
    }

    IEnumerator DownLoadFromServer(string fileName, OnDownloadOneFileDelegate onDownloadOneFile)
    {
        string fullUrl = ServerConf.ResUrl + fileName;
        Debug.Log("downloading : " + fullUrl);
        string persistantFullPath = Application.persistentDataPath + "/" + fileName;
        WWW www = new WWW(fullUrl);
        float downLoadProgress = www.progress;
        if(onDownloadOneFile != null)
            onDownloadOneFile(downLoadProgress);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
        }
        else if (www.isDone)
        {
            byte[] bytes = www.bytes;
            string dir = persistantFullPath.Substring(0, persistantFullPath.LastIndexOf("/"));
            Debug.Log(dir);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            FileStream file = new FileStream(persistantFullPath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(bytes);
            sw.Close();
        }
        yield return 1;
    }
    IEnumerator DownLoadVersionFileFromServer(string fileName, OnDownloadOneFileDelegate onDownloadOneFile)
    {
        string fullUrl = ServerConf.ResUrl + fileName;
        Debug.Log("downloading : " + fullUrl);
        string persistantFullPath = Application.persistentDataPath + "/" + fileName;
        WWW www = new WWW(fullUrl);
        float downLoadProgress = www.progress;
        if (onDownloadOneFile != null)
            onDownloadOneFile(downLoadProgress);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
        }
        else if (www.isDone)
        {
            string txt = www.text;
            string dir = persistantFullPath.Substring(0, persistantFullPath.LastIndexOf("/"));
            Debug.Log(dir);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            FileStream file = new FileStream(persistantFullPath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(txt);
            sw.Close();
        }
    }


    void BeforeDownloadOneFile(object param)
    {

    }
    void OnDownloadOneFile(object param)
    {

    }

    void AfterDownloadOneFile(object param)
    {

    }

    void AfterDownload(object param)
    {

    }
}
