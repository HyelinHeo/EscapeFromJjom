using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class OnClickMoveScene : MonoBehaviour
{
    public bool Async;
    public string SceneName;
    public int SceneIndex;

    [SerializeField]
    private float progress;

    public UnityEvent<float> ChanegedPregressValue = new UnityEvent<float>();
    public UnityEvent LoadCompleted = new UnityEvent();
    public UnityEvent LoadStarted = new UnityEvent();

    private bool loading;

    void Start()
    {
        loading = false;

    }

    public void OnClick()
    {
        if (Async)
        {
            if (!loading)
            {
                loading = true;
                LoadStarted.Invoke();
                StartCoroutine(LoadSceneAsync());
            }
        }
        else
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    public void MoveScene(string sceneName)
    {
        SceneName = sceneName;
        OnClick();
    }

    public void MoveScene(int sceneIndex)
    {
        SceneIndex = sceneIndex;
        OnClick();
    }

    IEnumerator LoadSceneAsync()
    {
        ChanegedPregressValue.Invoke(0);

        AsyncOperation oper = null;
        if (!string.IsNullOrEmpty(SceneName))
        {
            oper = SceneManager.LoadSceneAsync(SceneName);
        }
        else if (SceneIndex >= 0)
        {
            oper = SceneManager.LoadSceneAsync(SceneIndex);
        }

        while (!oper.isDone)
        {
            progress = oper.progress;
            if (progress >= 0.9f)
            {
                ChanegedPregressValue.Invoke(1f);
                LoadCompleted.Invoke();
            }
            yield return null;
        }
    }
}
