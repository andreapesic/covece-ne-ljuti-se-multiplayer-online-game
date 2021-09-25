using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public bool InHouse = true;
    public bool isFinished;
    public Player p;
    public int id;
    public colorType color;
    public int currentField;
    public bool onSpawn;
    public new bool enabled=false;
    public bool inFronOf = false;

    public void DestoryMe(bool f,Vector2 pos)
    {                  
        if (f)
        {
           InHouse = true;
           p.PieceEaten();
        }
        onSpawn = false;           
        inFronOf = false;
        currentField = 0;
    }
}
