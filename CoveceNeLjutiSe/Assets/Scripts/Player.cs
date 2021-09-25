using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    public List<Piece> player;
    public string name;
    public bool playAgain;
    public int numberOfThrows;
    public int numberInHouse = 4;
    public bool allInHouse = true;
    public bool turn=false;
    public bool gotSix = false;
    public int odigranBroj = 0;
    public bool onSpawner = false;
    public int numberInGame = 4;
    public bool iCanPlaySmth = false;
    private PlayerManager pManager;
     PieceAnimControler animControler;
    public bool canPlayThatNum = false;


    
    public void DisableAll()
    {
        foreach (Piece p in player)
        {
            p.enabled = false;
        }
    }
    public void EnableAll()
    {
        foreach (Piece p in player)
        {
            p.enabled = true;
        }
    }

    internal void PieceEaten()
    {
        numberInHouse++;
        if (numberInHouse == 4)
        {
            allInHouse = true;
            Debug.Log("Svi u kucici");
            YourTurn();
        }
    }

    public void EnableNotHouse()
    {
        foreach (Piece p in player)
        {
            if(!p.InHouse)
                p.enabled = true;
        }
    }
    public Player(PlayerManager pManagger)
    {
        player = new List<Piece>();
        this.pManager = pManagger;
    }
    public void AddPlayer(Piece p,int id, colorType color)
    {
        p.color = color;
        p.id = id;
        p.currentField = -1;
        p.InHouse = true;
        p.p = this;
        if (name == null) name = p.gameObject.name;
        player.Add(p);
        if(player.Count==4) animControler = new PieceAnimControler(player);

    }

    internal void UsaoUKucicu(int idPi, int v)
    {
       
      player[idPi].currentField = v;
        
        if (!player[idPi].isFinished)
        {
            player[idPi].isFinished = true;
            numberInGame--;
            if (numberInGame == numberInHouse)
            {
                allInHouse = true;
                Debug.Log("isti broj u kucici i u final");
            }
        }
    }

    internal void removePiece(int piece_id,Vector2 pos)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            onSpawner = false;
            player[piece_id].DestoryMe(true,pos);
            player[piece_id].transform.position = pos;

        });
        
    }

    public void YourTurn()
    {       
        if (allInHouse)
        {
            numberOfThrows = 3;
            UpdateBacanja();
        }
        else
        {
            numberOfThrows = 1;
            UpdateBacanja();
        }      
    }

    public void ThrowDice(int num)
    {
        turn = false;
        numberOfThrows--;
        UpdateBacanja();
        OdigrajPotez(num);
        canPlayThatNum = true;
        if ((numberOfThrows == 0 && !turn )|| numberOfThrows<0)
        {
            ResetPieces();
            EndTurn();
        }
       

    }

    private void UpdateBacanja()
    {
       
       pManager.ui_Contorl.UpdateBacanja(numberOfThrows);
    }

    internal void Dobio6()
    {
        gotSix = true;
        numberOfThrows++;
        UpdateBacanja();
       
    }
    public void EndTurn()
    {
        // turn = false;
        GameManager.instance.StopDice();
        animControler.StopAnim();
        PacketSender.SendOverTurn(Client.instance.user.id);
    }
    public void TryToEndTurn(int id)
    {
     
        DisableAll();
     
        turn = false;
        
        GameManager.instance.StartDice();
        animControler.StopAnim();
        Piece p = player[id];

        if (p.InHouse && gotSix && !onSpawner)
        {
          
            PacketSender.IzbaciIzKuce(p.id);
            MovePlayerForSix(p.id);
        }
        else
        {
            
            MovePlayerForNum(p.id);
            PacketSender.PomeriZaBrojPolja(p.id, odigranBroj);
           
        }
        gotSix = false;
        ResetPieces();
        //pomeri igraca
        if (numberOfThrows == 0)
        {
            DisableAll();
            animControler.StopAnim();
            EndTurn();

        }
    }

    private void ResetPieces()
    {
        foreach(Piece p in player)
        {
            p.inFronOf = false;
        }
    }
    

    private void MovePlayerForNum(int id)
    {


        if (player[id].onSpawn)
        {
            player[id].onSpawn = false;
            onSpawner = false;
        }
        player[id].currentField += odigranBroj;
    
    }

    private void MovePlayerForSix(int id)
    {

            player[id].currentField = 0;
        player[id].onSpawn = true;
        player[id].InHouse = false;
        onSpawner = true;
            IzvediIzKucice();
          
    }

    public void IzvediIzKucice()
    {

            numberOfThrows = 1;
            UpdateBacanja();

        numberInHouse--;
            allInHouse = false;

    }
  

    public void OdigrajPotez(int num)
    {
        CheckWhoCanPlay(num);
        CheckWhoCanPlay1(num);
        CheckWhoCanPlay2(num);


        if (allInHouse && num == 6 )
        {

         
           // EnableAll();
           // GameManager.instance.StopDice();
            Dobio6();
            canPlayThatNum= animControler.StartAnim();
            turn = true;
        }
        else if (!allInHouse && num == 6 && onSpawner)
        {

           
            //EnableNotHouse();
           // GameManager.instance.StopDice();
            odigranBroj = num;
            Dobio6();
            canPlayThatNum= animControler.StartAnimForNotHause(num);
            turn = true;

        }
        else if (!allInHouse && num == 6)
        {

            //EnableAll();
           // GameManager.instance.StopDice();
            odigranBroj = num;
            Dobio6();
            canPlayThatNum=animControler.StartAnim();
            turn = true;

        }
        else if (!allInHouse  && num!=6)
        {

         
            //EnableNotHouse();
           // GameManager.instance.StopDice();
            odigranBroj = num;
            canPlayThatNum=animControler.StartAnimForNotHause(num);
                turn = true;

        }
        Debug.Log("can play?" + canPlayThatNum);
        if (!canPlayThatNum && numberOfThrows<=0)
        {
            ResetPieces();
            EndTurn();
        }
      

    }
    //da li moze na terenu
    private void CheckWhoCanPlay(int num)
    {
        if(!allInHouse)
        foreach (Piece p in player)
        {
           Piece lisP = player.FirstOrDefault(x => !x.isFinished && !p.isFinished &&
           !x.InHouse && x.id != p.id && x.currentField == p.currentField + num);
           
            if (lisP != null)
            {
                //postoji neko ko je ispred tebe na tom polju ne mozes ti p da svetlis
                p.inFronOf = true;
                    Debug.Log("1 namesta da je true" + p.id);
                  
            }
        }
    }
    //da li moze na terenu ipri ulazu u kucici
    private void CheckWhoCanPlay1(int num)
    {
        if (!allInHouse)
            foreach (Piece p in player)
            {
                if (p.currentField + num > 39)
                {
                    int trebaDaIdeDoKrajaMape = 39 - p.currentField;
                    int uKuciciFinalDaIde = Math.Abs(trebaDaIdeDoKrajaMape - num);
                    Piece lisP = player.FirstOrDefault(x =>x.isFinished && 
                    !x.InHouse && x.id != p.id && x.currentField == uKuciciFinalDaIde);
                    if (lisP != null)
                    {
                        //postoji neko ko je ispred tebe na tom polju ne mozes ti p da svetlis
                        p.inFronOf = true;
                        Debug.Log("2 namesta da je true" + p.id);
                    }
                }
            }
    }
    //da li moze nu kucici final
    private void CheckWhoCanPlay2(int num)
    {
      
        if (!allInHouse)
            foreach (Piece p in player)
            {
                Piece lisP = player.FirstOrDefault(x => x.isFinished && p.isFinished&&
                !x.InHouse && x.id != p.id && x.currentField == p.currentField + num);

                if (lisP != null)
                {
                    //postoji neko ko je ispred tebe na tom polju ne mozes ti p da svetlis
                    p.inFronOf = true;
                    Debug.Log("3 namesta da je true" + p.id);
                }
            }
    }
}