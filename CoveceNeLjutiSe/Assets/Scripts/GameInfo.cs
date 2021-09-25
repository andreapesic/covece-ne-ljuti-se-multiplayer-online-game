using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo 
{
    public int idTurn;
    public int WhoisFirst;

    internal void SetFirst(int randomNum)
    {
        WhoisFirst = randomNum;
        idTurn = WhoisFirst;
    }
}
