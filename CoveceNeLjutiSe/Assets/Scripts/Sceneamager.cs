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

    public void LoadRoom()
    {
        LoadScene(Information.scene_Loby);
    }

    internal void LoadMainScene()
    {     
        LoadScene(Information.scene_main);
    }

    internal void GameScene()
    {
        LoadScene(Information.game_scene);
    }

    private void LoadScene(int Scene)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            SceneManager.LoadScene(Scene);
        });
    }
}
