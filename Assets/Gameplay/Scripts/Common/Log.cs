using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class Log
{
    public const int NONE = 0;
    public const int ERROR = 1;
    public const int WARNING = 2;
    public const int INFO = 3;

    public static int CurrentLogLv = 0;
    public static StringBuilder sb;

    static string tmpTxt;
    static string folder;
    static string fileName;
    static string path;

    static int count;
    static string[] files;
    static DateTime now;
    static FileInfo fi;

    static void AddText(string txt)
    {
        if (sb == null)
            sb = new StringBuilder();

        try
        {
            sb.Append(txt);
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
        }
    }

    public static void Init()
    {
        count = 0;
        folder = Application.dataPath + "\\..\\Log";

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        CurrentLogLv = INFO;
    }

    public static void Info(object message)
    {
        if (CurrentLogLv >= INFO)
        {
            now = DateTime.Now;
            tmpTxt = string.Format("\n[Info][{0}] {1}", now, message);
#if UNITY_EDITOR
            Debug.Log(tmpTxt);
#endif
            AddText(tmpTxt);
        }
    }

    public static void Error(object message)
    {
        if (CurrentLogLv >= ERROR)
        {
            now = DateTime.Now;
            tmpTxt = string.Format("\n[Error][{0}] {1}", now, message);
#if UNITY_EDITOR
            Debug.LogError(tmpTxt);
#endif
            AddText(tmpTxt);
        }
    }

    public static void Warning(object message)
    {
        if (CurrentLogLv >= WARNING)
        {
            now = DateTime.Now;
            tmpTxt = string.Format("\n[Warning][{0}] {1}", now, message);
#if UNITY_EDITOR
            Debug.LogWarning(tmpTxt);
#endif
            AddText(tmpTxt);
        }
    }

    static StreamWriter sw;
    public static void Save()
    {
        try
        {
            if (sb != null && sb.Length > 0)
            {
                now = DateTime.Now;
                fileName = string.Format("{0}_{1:yyyy-MM-dd_HH_mm_ss}.txt", count, now);
                path = string.Format("{0}\\{1}", folder, fileName);

                using (sw = new StreamWriter(path))
                {
                    sw.Write(sb.ToString());
                }

                sb.Clear();
                Debug.LogFormat("Save Log File. {0}", path);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }


    static void RemoveOldLog()
    {
        now = DateTime.Now;
        //now = now.AddDays(Config.Inst.SAVE_LOG_DAYS * -1);
        now = now.AddDays(10);

        files = Directory.GetFiles(folder);
        for (int i = 0; i < files.Length; i++)
        {
            fi = new FileInfo(files[i]);
            if (DateTime.Compare(fi.LastAccessTime, now) < 0)
            {
                Debug.LogFormat("Delete Log File. {0}", files[i]);
                fi.Delete();
            }
        }
    }
}
