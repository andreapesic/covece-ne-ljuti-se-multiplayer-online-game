using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Manager : MonoBehaviour
{
    public Text brojIgraca;
    
    public static Ui_Manager instance;

    private void Start()
    {
        UpateText();
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
    public void DodajIgraca()
    {
        Server.instance.numOfConnected++;
        UpateText();
    }
    public void OduzmiIgraca()
    {
        Server.instance.numOfConnected--;
        UpateText();

    }
    public void UpateText()
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            brojIgraca.text = Server.instance.numOfConnected.ToString();
        });
       
    }

}
