using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager :  MonoBehaviour
{

    public UIControl ui_Contorl;
    public Player p;

    public void AddPlayer(Piece p ,int id, colorType color)
    {
      
        this.p.AddPlayer(p,id,color);
       
    }

    
    private void Awake()
    {
        p = new Player(this);
      
    }

 
    public void Play()
    {
        p.YourTurn();
    }
    public void ThrowDice(int num)
    {
        p.ThrowDice(num);

    }

    internal void Move(int idPi, float x, float y)
    {
        GameManager.instance.IzvediIgraca(p.player[idPi],new Vector2(x,y));
    }
    internal void MoveForPolje(int idPi,float x,float y) {
        GameManager.instance.IdiKaPolju(p.player[idPi], new Vector2(x, y));
    }

    internal void RemoveFromGame(int piece_id, Vector2 vector2)
    {
        p.removePiece(piece_id,vector2);
    }

    internal void UsaoUKucincu(int idPi, int v)
    {
        p.UsaoUKucicu(idPi,v);
    }
}
