using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
public class FileHelper
{

    public static void GetAllFileFullPath(string rootPath, ref List<string> prefabsFullPaths, string[] formStr)
    {
        if(formStr == null || formStr.Length == 0)
            return;
        string[] files = Directory.GetFiles(rootPath);
        if (files != null)
        {
            for (int i = 0; i < files.Length; i++)
            {
                for (int j = 0; j < formStr.Length; j++)
                {
                    if (files[i].EndsWith(formStr[j]))
                        prefabsFullPaths.Add(files[i].Replace("/", "\\"));
                }
            }
        }

        string[] directories = Directory.GetDirectories(rootPath);
        if (directories != null)
        {
            for (int i = 0; i < directories.Length; i++)
            {
                GetAllFileFullPath(directories[i], ref prefabsFullPaths, formStr);
            }

        }

    }

    public Dictionary<string, List<string>> GetAllPrefabsFullPath()
    {
        Dictionary<string, List<string>> prefabsFullPaths = new Dictionary<string, List<string>>();




        return prefabsFullPaths;
    }

    public static byte[] ReadBytes(string fullPath)
    {
        FileStream fsSource = File.OpenRead(fullPath);
        byte[] bytes = new byte[fsSource.Length];
        int numBytesToRead = (int)fsSource.Length;
        int numBytesRead = 0;
        while (numBytesToRead > 0)
        {
            int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);
            if (n == 0)
                break;
            numBytesRead += n;
            numBytesToRead -= n;
        }
        fsSource.Close();
        return bytes;
    }

    public static void ReadVersionFile(Dictionary<string, string> list)
    {
        string versionFile = Define.VersionFileFullPath;
        FileStream fs = null;
        if (!File.Exists(versionFile))
            return;
        else
            fs = new FileStream(versionFile, FileMode.Open, FileAccess.Read);  //打开读文件

        StreamReader sr = new StreamReader(fs);
        StringBuilder sb = new StringBuilder();
        string line = "";
        while ((line = sr.ReadLine()) != null)
        {
            sb.Append(line);
        }
        sr.Close();
        fs.Close();

        List<Define.VersionInfo> tmp = JsonMapper.ToObject<List<Define.VersionInfo>>(sb.ToString());
        if (tmp == null || tmp.Count == 0)
            return;
        for (int i = 0; i < tmp.Count; i++)
            list.Add(tmp[i].fileName, tmp[i].md5);
    }

    public static string ReadLuaScriptFile(string filePath)
    {
        StringBuilder sb = new StringBuilder();

        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        string str = "";
        while ((str = sr.ReadLine()) != null)
        {
            sb.Append(str + "\n");
        }


        sr.Close();
        fs.Close();

        return sb.ToString();
    }
}


