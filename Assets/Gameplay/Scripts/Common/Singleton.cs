using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T g_Instance = default(T);
    public static T Inst
    {
        get
        {
            if (g_Instance == null)
            {
                T t = GameObject.FindObjectOfType(typeof(T)) as T;
                if (t == null)
                {
                    string strName = typeof(T).ToString();
                    GameObject go = new GameObject(string.Format("[{0}]", strName));
                    g_Instance = go.AddComponent<T>();
                }
                else
                {
                    g_Instance = t;
                }
            }

            return g_Instance;
        }
    }
}
