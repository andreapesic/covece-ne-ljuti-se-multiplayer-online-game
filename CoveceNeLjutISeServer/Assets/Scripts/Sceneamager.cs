using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sceneamager : MonoBehaviour
{
    public static Sceneamager instance;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

 
    internal void LoadMainScene()
    {     
        LoadScene(Information.MainScene);
    }

    internal void GameScene()
    {
        LoadScene(Information.GameScene);
    }

    internal void LoadWinningScene()
    {
        LoadScene(Information.WinningScene);
    }

    private void LoadScene(int Scene)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            if (Scene != 2)
            {
                PacketExecutor.instance.Clear();
            }            
            SceneManager.LoadScene(Scene);
        });
    }
}
