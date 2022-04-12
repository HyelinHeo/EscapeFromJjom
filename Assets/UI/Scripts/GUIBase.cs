using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;



public class GUIBase : MonoBehaviour
{
    public class OnChangeShow : UnityEvent<bool> { }

    public RectTransform Rect;

    private bool m_isShow = false;
    public bool isShow
    {
        get { return m_isShow; }
    }

    public OnChangeShow OnChangedShow = new OnChangeShow();
    public UnityEvent Shown = new UnityEvent();
    public UnityEvent Hided = new UnityEvent();

    public virtual void Awake()
    {
        if (Rect == null)
            Rect = GetComponent<RectTransform>();
    }

    public void AddListener(Button btn, UnityAction call)
    {
        if (btn != null)
        {
            btn.onClick.AddListener(call);
        }
    }

    public void RemoveListener(Button btn, UnityAction call)
    {
        if (btn != null)
        {
            btn.onClick.RemoveListener(call);
        }
    }

    private void Show(bool isShow)
    {
        if (isShow)
        {
            m_isShow = true;
            Rect.SetAsLastSibling();
            Rect.gameObject.SetActive(true);

            Shown.Invoke();

        }
        else if (!isShow)
        {
            m_isShow = false;
            Rect.SetAsFirstSibling();
            Rect.gameObject.SetActive(false);

            Hided.Invoke();
        }

        if (OnChangedShow != null)
        {
            OnChangedShow.Invoke(m_isShow);
        }
    }

    public virtual void Refresh()
    {

    }

    public virtual void Hide()
    {
        Show(false);
    }

    public virtual void Show()
    {
        Show(true);

    }


    public virtual void Release()
    {
        Hide();
    }
}
