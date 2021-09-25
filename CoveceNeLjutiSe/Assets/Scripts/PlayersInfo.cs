using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfo : MonoBehaviour
{
    public static PlayersInfo instance;
    private Dictionary<int, User> players;
    public Dictionary<int, User> Players { get => players; }

    private void Start()
    {
     
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
        players = new Dictionary<int, User>();
    }
 

    public  void Clear()
    {
        players.Clear();
    }

    public void ChangePlayersInfo(int id, colorType colorType=colorType.NONE)
    {
        try
        {
           players[id].colorType = colorType;
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void AddPlayer(int id,User u)
    {
        players.Add(id, u);
    }
    public void RemovePlayer(int id)
    {
        players.Remove(id);
    }
}
