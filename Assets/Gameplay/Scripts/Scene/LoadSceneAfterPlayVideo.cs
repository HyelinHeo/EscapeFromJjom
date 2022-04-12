using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class LoadSceneAfterPlayVideo : MonoBehaviour
{
    public string NextSceneName;
    public VideoPlayer Player;

    void Start()
    {
        Player.loopPointReached += Player_loopPointReached;
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
       if (!string.IsNullOrEmpty(NextSceneName))
        {
            SceneManager.LoadScene(NextSceneName);
        }
    }
}
