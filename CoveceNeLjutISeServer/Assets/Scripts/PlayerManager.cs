using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :  MonoBehaviour
{
    public static PlayerManager instance;

   
    public Player p;

    internal void resetPlayer()
    {

        p.PieceEaten();
    }

    private void Awake()
    {
        p = new Player();
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

   
}
