using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPXFile
{
    public class CustomFileList<T> : IDisposable where T : CustomFile
    {
        public List<T> list;

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < list.Count)
                {
                    return list[index];
                }
                return null;
            }
        }

        public int Count
        {
            get { return list != null ? list.Count : 0; }
        }

        public CustomFileList()
        {
            list = new List<T>();
        }

        ~CustomFileList()
        {
            Dispose();
        }

        public virtual T Get(int index)
        {
            if (list != null && index >= 0 && index < list.Count)
            {
                T custom = list[index];
                if (custom != null)
                {
                    return (T)custom;
                }
            }
            return null;
        }


        public virtual int FindIndex(string str)
        {
            str = str.ToLower();
            return list.FindIndex(o => o.Name != null && o.Name.ToLower().Contains(str));
        }

        public T Find(string id)
        {
            return list.Find(o => o.ID != null && o.ID == id);
        }

        public virtual void Sort()
        {
            list.Sort((a, b) => a.Name.CompareTo(b.Name));
        }

        public List<T> FindFileAll(string id)
        {
            return list.FindAll(o => o.ID != null && o.ID == id);
        }

        public virtual List<T> FindAll(string str)
        {
            str = str.ToLower();
            return list.FindAll(o => o.Name != null && o.Name.ToLower().Contains(str));
        }


        public void Add(T file)
        {
            if (list != null && file != null)
            {
                list.Add(file);
            }
        }

        public void Clear()
        {
            if (list != null)
            {
                list.Clear();
            }
        }


        public void Dispose()
        {
            if (list != null)
            {
                list.Clear();
                list = null;
            }
        }

        public bool Remove(T file)
        {
            return list.Remove(file);
        }
    }
}
