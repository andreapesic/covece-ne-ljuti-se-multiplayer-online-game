using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UI_Manager_Room : MonoBehaviour
{

    public static UI_Manager_Room instance;
    public GameObject dialog;
    public TextMeshProUGUI text;
    public TextMeshProUGUI adminText;
    Client c;
    public GameObject element;
    public GameObject HolderElement;
    private float y = 250f;
    private float x = 82f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {

            Destroy(this);
        }
    }



    public void PromenaReadyStanja(int id,bool ready)
    {
        try
        {
            MainThreadManager.ExecuteOnMainThread(()=> {
                for (int i = 0; i < HolderElement.transform.childCount; i++)
                {
                    GameObject child = HolderElement.transform.GetChild(i).gameObject;

                    if (child.transform.GetChild(0).GetChild(2).GetComponent<Text>().text.Contains(id.ToString()))
                    {
                        child.transform.GetChild(0).GetChild(1).GetComponent<Toggle>().isOn = ready;
                        return;
                    }
                }
            });           
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void PomeriRaspored(int pocetak)
    {
        y += 200f;
     

        for (int i = pocetak; i < HolderElement.transform.childCount; i++)
        {
            GameObject child = HolderElement.transform.GetChild(i).gameObject;
            child.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
            y -= 200f;
        }

    }

    public void DeleteClientOnDisconnect(int id)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            int i;
            for (i = 0; i < HolderElement.transform.childCount; i++)
            {

                GameObject child = HolderElement.transform.GetChild(i).gameObject;

                if (child.transform.GetChild(0).GetChild(2).GetComponent<Text>().text.Contains(id.ToString()))
                {
                    child.transform.SetParent(null);
                    Destroy(child);
                    break;
                }
            }
            PomeriRaspored(i);
        });
       
    }
    public void DodatKlijent(string ime,int id, int color)
    {
        try
        {

            MainThreadManager.ExecuteOnMainThread(() =>
            {

         
                GameObject obj = Instantiate(element) as GameObject;
            
                obj.transform.SetParent(HolderElement.transform);
                obj.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
                obj.GetComponent<RectTransform>().localScale = new Vector2(46f, 46f);
                obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ime;
                obj.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = id.ToString();

                Color c = GetColor(color);
             
                obj.transform.GetChild(0).GetChild(3).GetComponent<Image>().color = c;

                obj.transform.GetChild(0).GetChild(1).GetComponent<Toggle>().isOn = true;
                obj.transform.GetChild(0).GetChild(1).GetComponent<Toggle>().enabled = false;
                y -= 200f;

                
            });

            


        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }




    }

    public void PrikaziMeni()
    {
        dialog.gameObject.SetActive(true);
    }
    public void ChangeColor(int id,int color)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            foreach (Transform child in HolderElement.transform)
            {
                if (child.GetChild(0).GetChild(2).GetComponent<Text>().text.Contains(id.ToString())){

                    Color c = GetColor(color);


                    child.GetChild(0).GetChild(3).GetComponent<Image>().color = c;
                    return;
                }
            }
        });
      
    }

    private Color GetColor(int color)
    {
        switch (color)
        {
            case ((int)colorType.RED): return Color.red;
            case ((int)colorType.BLUE): return Color.blue;
            case ((int)colorType.GREEN): return Color.green; 
            case ((int)colorType.YELLOW): return Color.yellow; 
                
        }
        return Color.black;
    }

    public void ExitButtonClicekd()
    {
        dialog.SetActive(false);
    }
   
    private void Update()
    {
        MainThreadManager.UpdateMain();
    }
    public void Back()
    {
        PacketSender.SendDisconect(Client.instance.user.id);
        Client.instance.Disconnect();
       

    }
    
  
    private void Start()
    {
        
        PlayersInfo.instance.AddPlayer(Client.instance.user.id, Client.instance.user);
        dialog.gameObject.SetActive(false);
        c = Client.instance;
        Client.instance.user.tekUsao = false;
       
        DodatKlijent(c.user.username ,c.user.id,0);
       
        ChangeAdmin(c.user.isAdmin);
        SetName(c.user.username);
        PacketSender.RequestAllClients();
      
    }
   
    private void SetName(string name)
    {
        if (c != null)
        {

            text.text = name;
        }
    }
    public void ChangeAdmin(bool isAdmin)
    {
        
        if (isAdmin)
        {
            adminText.text = "Admin";

           
        }
        else
        {
            adminText.text = "/";
        }
        
    }
    public void StartGame()
    {

        if(Client.instance.user.isAdmin)
                PacketSender.RequestReady();

    }
    public void ReadyChanged()
    {

        c.user.isReady = !c.user.isReady;
        PromenaReadyStanja(Client.instance.user.id, Client.instance.user.isReady);
        PacketSender.SendReadyChanged();
    }
}
