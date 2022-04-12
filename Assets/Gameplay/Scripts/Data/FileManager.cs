using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class FileManager : Singleton<FileManager>
{
    public string BaseFolder;

    private void Awake()
    {
    }


    public T Load<T>(string fileName) where T : GameData
    {
        string filePath = GetFilePath(fileName);

        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            return ByteArrayToObject<T>(bytes);
        }

        return null;
    }

    public string GetFilePath(string fileName)
    {
        if (string.IsNullOrEmpty(BaseFolder))
            BaseFolder = Application.persistentDataPath;

        return BaseFolder + "\\" + fileName;
    }

    public string DeleteFile(string filePath)
    {
        string result = string.Empty;
        if (!string.IsNullOrEmpty(filePath))
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }
            }
        }
        return result;
    }

    public string[] GetFiles(string dir)
    {
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        return Directory.GetFiles(dir);
    }

    public bool Save(GameData file, string fileName, ref string error)
    {
        string filePath = GetFilePath(fileName);

        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        byte[] bytes = ObjectToByteArray(file);
        try
        {
            error = null;
            File.WriteAllBytes(filePath, bytes);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
        return true;
    }

    public byte[] ObjectToByteArray(object obj)
    {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    private T ByteArrayToObject<T>(byte[] arrBytes) where T : GameData
    {
        T obj = null;
        try
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            obj = (T)binForm.Deserialize(memStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return obj;
    }
}
