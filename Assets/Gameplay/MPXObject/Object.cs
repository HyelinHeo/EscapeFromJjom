using System;

namespace MPXObject
{
    [Serializable]
    public abstract class Object
    {
        public string ID;
        public string Name { get; set; }

        public Object()
        {
            SetNewID();
            Name = string.Empty;
        }

        public virtual string GetNewID()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual void SetNewID()
        {
            ID = GetNewID();
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, Name:{1}", ID, Name);
        }
    }
}
