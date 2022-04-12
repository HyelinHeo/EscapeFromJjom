using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUIMessageBox : GUIWindow
{
    public enum MessageButtons
    {
        YESNO,
        OK
    }


    public Text TxtMessage;
    public Button BtnYes;
    public Button BtnNo;

    public MessageButtons BtnType;

    public UnityEvent ResultYes = new UnityEvent();
    public UnityEvent ResultNo = new UnityEvent();

    public override void Awake()
    {
        base.Awake();

        BtnType = MessageButtons.OK;
   
    }

    void Start()
    {
    
    }

    public override void Show()
    {
        switch (BtnType)
        {
            case MessageButtons.YESNO:
                BtnYes.gameObject.SetActive(true);
                BtnNo.gameObject.SetActive(true);
                break;
            case MessageButtons.OK:
                BtnYes.gameObject.SetActive(true);
                BtnNo.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        Time.timeScale = 0;

        base.Show();

        BtnYes.onClick.AddListener(OnClickYes);
        BtnNo.onClick.AddListener(OnClickNo);
    }

    public void Show(string message)
    {
        TxtMessage.text = message;

        Show();
    }

    public void Show(string message, MessageButtons buttons)
    {
        BtnType = buttons;

        Show(message);
    }

    public override void Hide()
    {
        BtnYes.onClick.RemoveAllListeners();
        BtnNo.onClick.RemoveAllListeners();
        ResultYes.RemoveAllListeners();
        ResultNo.RemoveAllListeners();

        Time.timeScale = 1;
        base.Hide();
    }

    public void OnClickYes()
    {
        ResultYes.Invoke();
        Hide();
    }

    public void OnClickNo()
    {
        ResultNo.Invoke();
        Hide();
    }


}
