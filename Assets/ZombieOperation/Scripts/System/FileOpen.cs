using UnityEngine;
using System.IO;
using System;
using System.Text;

// ファイル読み込み
public class FileOpen : MonoBehaviour
{
    public static string FileRead(string _fileName)
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/Database/" + _fileName + ".txt");
        string returnSt = "";

        try
        {
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                returnSt = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            print(e.Message);
            returnSt = "READ ERROR: " + _fileName;
        }

        return returnSt;
    }
}