using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public int id;
    public string username;
    public bool isReady = true;
    public colorType color=0;
    public bool isAdmin=false;
    internal bool isOnMove;
    public Player p;
    internal bool tekUsao = true;

    internal void reset()
    {
        username = "Nema";
        color = 0;
        isAdmin = false;
        tekUsao = true;
        isOnMove = false;
        isReady = true;        
    }
}
public enum colorType
{
    NONE,
    RED,
    GREEN,
    BLUE,
    YELLOW
}
