using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public int id;
    public string username;
    public bool isReady = true;
    internal bool isAdmin;
    public colorType colorType;
    public int whoIsFirst;
    internal bool potvrdaKonekcije=false;
    internal bool tekUsao=true;
    internal bool zapoceto;

    internal void restartinfo()
    {
        id = 0;
        username = "";
        isReady = true;
        isAdmin = false;
        colorType = colorType.NONE;
        whoIsFirst = 0;
        tekUsao = true;
        zapoceto = false;
        potvrdaKonekcije = false;
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
