using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUIManager : Singleton<GUIManager>
{
    private Dictionary<string, GUIBase> m_hashList = new Dictionary<string, GUIBase>();
    private readonly string PATH = "UI/";

    //public Canvas UICanvas;


    void Awake()
    {
        Init();
    }

    public void Init()
    {
        Add<GUIPlaying>();
        Add<GUIGameOver>();
        Add<GUIClearGame>();
        Add<GUIQuestionDungeonName>();
        Add<GUIMessageBox>();

        Get<GUIPlaying>().Show();
    }

    private void Start()
    {
    }


    public void Add<T>() where T : GUIBase, new()
    {
        string strUI = typeof(T).ToString();

        GUIBase item;
        if (!m_hashList.TryGetValue(strUI, out item))
        {
            item = GameObject.FindObjectOfType<T>();
            if (item == null)
            {
                GameObject obj = Instantiate(Resources.Load(PATH + strUI)) as GameObject;

                if (obj == null)
                {
                    Debug.Log(PATH + strUI);
                }
                obj.transform.SetParent(this.transform);
                obj.transform.localScale = Vector3.one;
                item = obj.GetComponent<T>();
            }
            item.Hide();
            m_hashList.Add(strUI, item);
        }
    }

    public T Get<T>() where T : GUIBase
    {
        string strUI = typeof(T).ToString();
        GUIBase guiBase = null;
        if (false == m_hashList.TryGetValue(strUI, out guiBase))
        {
            Debug.LogError("GUIManager :: Dont registration this Class" + strUI + "");
        }
        return (T)guiBase;
    }

    public void Release()
    {
        m_hashList.Clear();
    }

    public void Remove(string strName)
    {

        GUIBase guiBase = null;
        if (false == m_hashList.TryGetValue(strName, out guiBase))
        {
            Debug.LogError("GUIManager :: already Not exist UI" + strName);
        }
        m_hashList.Remove(strName);
        GameObject.Destroy(guiBase.gameObject);
    }
}

