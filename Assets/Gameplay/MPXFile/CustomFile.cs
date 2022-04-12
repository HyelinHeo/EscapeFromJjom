using System;
using System.IO;
using System.Collections.Generic;

namespace MPXFile
{
    [Serializable]
    public abstract class CustomFile
    {
        public string ID;
        public string Name;

        public CustomFile()
        {
            ID = NewID();
        }

        public static string NewID()
        {
            return Guid.NewGuid().ToString();
        }

        public static string SetExtension(string path, string ext)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (string.IsNullOrEmpty(Path.GetExtension(path)))
                {
                    return path += ext;
                }
            }
            return path;
        }
    }
}
