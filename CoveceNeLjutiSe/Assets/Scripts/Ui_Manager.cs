using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui_Manager : MonoBehaviour
{
  
  public  InputField username;
  
    public void StartClicked()
    {
        if (username.text.Trim().Length == 0)
        {
            Debug.Log("Unesi ime ");
            return;
        }
          
        Client.instance.user.username = username.text;
        Client.instance.ConnectToServer();
        
    }
  
    public void QuitClicked()
    {
        Application.Quit();
    }
}
