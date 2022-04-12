using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MPXFile
{
    public class MPXFileManager
    {
        public delegate void EventFile(CustomFile file, string filePath);

        public static string BaseFolder = "c:\\";

        public static T Load<T>(string filePath) where T : CustomFile
        {
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null)
                        return ByteArrayToObject<T>(bytes);
                }
                catch { }
            }

            return null;
        }

        public static string DeleteFile(string filePath)
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

        public static string[] GetFiles(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Directory.GetFiles(dir);
        }

        public static bool Save(CustomFile file, string filePath, ref string error)
        {
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

        public static byte[] ObjectToByteArray(object obj)
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

        private static T ByteArrayToObject<T>(byte[] arrBytes) where T : CustomFile
        {
            if (arrBytes != null)
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
            return null;
        }
    }
}
