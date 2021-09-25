using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    public List<Piece> player ;
    public string name;
    public bool playAgain;
    public int numberOfThrows;
    public int numberInHouse = 4;
    public bool allInHouse = true;
    public bool turn=false;
    public bool gotSix = false;
    public int odigranBroj = 0;
    public bool onSpawner = false;
    int numInFinishHouse=0;
    

    internal void PieceEaten()
    {
        numberInHouse++;
        if (numberInHouse == 4)
        {
            allInHouse = true;
        }
    }
    public Player()
    {
        player = new List<Piece>();
    }

   

    public void IzasaoIzKucice(int idpi)
    {
        allInHouse = false;
        numberInHouse--;
        player[idpi].InHouse = false;
        player[idpi].currentField = 0;
        player[idpi].onSpawn = true;
    }
    public void PomerioSeZaPolje(int idpi,int num)
    {
      
       
      if(player[idpi].onSpawn==true)
            player[idpi].onSpawn = false;
       
        player[idpi].currentField += num;

    }

    public void UsaoUKucicuFinal(int idpi, int num, int _fromClient)
    {
        if (!player[idpi].isFinished)
            numInFinishHouse++;
        player[idpi].isFinished = true;
        player[idpi].currentField = num;
        
        Debug.Log("Broj u kucici" + numInFinishHouse);
        if (numInFinishHouse == 4) {
            Debug.Log("Pobednik je " + Server.clients[_fromClient].user.username);
            PacketSender.SendPobedioPacket(_fromClient);
            Packet packet = new Packet((int)Packet.ServerPackets.ServerDown);
            PacketSender.SendTCPDataToAll(packet);
            Sceneamager.instance.LoadWinningScene();

        }
    }

    

}