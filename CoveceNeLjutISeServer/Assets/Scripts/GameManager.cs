using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Spawner spawner;

    Vector2 pomerajIgraca = Vector2.zero;
    Piece zaPomeraj;

   public void Update() {


        
        if (pomerajIgraca!=Vector2.zero && zaPomeraj !=null&& (Vector2)zaPomeraj.gameObject.transform.position != pomerajIgraca)
        {
           
                zaPomeraj.transform.position = Vector2.Lerp(zaPomeraj.transform.position, pomerajIgraca, 0.4f);
          
        }
        



    }
    public void IzvediIgraca(Piece p, int clientID)
    {

        //MainThreadManager.ExecuteOnMainThread(() => { })
        
            MainThreadManager.ExecuteOnMainThread(() => { 


                try { 
        
                Vector2 spawnPos = Vector2.zero;
                switch (p.color)
                {
                    case colorType.RED: spawnPos = spawner.redSpawner.IzlazPos; break;
                    case colorType.YELLOW: spawnPos = spawner.yellowSpawner.IzlazPos; break;
                    case colorType.BLUE: spawnPos = spawner.blueSpawner.IzlazPos; break;
                    case colorType.GREEN: spawnPos = spawner.greenSpawner.IzlazPos; break;
                }


                 pomerajIgraca = spawnPos;
                zaPomeraj = p;
                PacketSender.SendWhereToIzadjes(spawnPos, clientID, p.id);

            }catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    });
       
    }

  

    internal void SpawnPojedenog(User user, int id)
    {
        pomerajIgraca = Vector2.zero;
        spawner.SpawnPojedenog(user,id);
    }

    public void IdiKaPolju(Piece p,int clientId)
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            Transform zeljenaPos = null;
            switch (p.color)
            {
                case colorType.RED: zeljenaPos = spawner.PathRed.transform.GetChild(p.currentField); break;
                case colorType.YELLOW: zeljenaPos = spawner.PathYellow.transform.GetChild(p.currentField); break;
                case colorType.BLUE: zeljenaPos = spawner.PathBlue.transform.GetChild(p.currentField); break;
                case colorType.GREEN: zeljenaPos = spawner.PathGreen.transform.GetChild(p.currentField); break;
            }


            pomerajIgraca = zeljenaPos.transform.position;
            zaPomeraj = p;
            PacketSender.SendWhereToIdes(zeljenaPos, clientId, p.id,false,p.currentField);
        });
     
      
   
    }
   

    private void Awake()
    {
        Server.instance.StopListenning();
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
    private void Start()
    {
     
      
      
       foreach(Client c in Server.clients.Values)
            if(c.tcp.socket!=null)
                SpawnPlayer(c.user);
        
     
       
    }
    public void DeletePiece(int id)
    {
        foreach(Piece p in Server.clients[id].user.p.player)
        {
            Destroy(p.gameObject);
        }
        Server.clients[id].Disconnect();
        Server.instance.numOfConnected--;
        Debug.Log(Server.instance.numOfConnected);

    }
 
  

  public void IzbaciIzKucice(int idpi,int klijentId)
    {
        try
        {

            
            Piece p = Server.clients[klijentId].user.p.player[idpi];
            Server.clients[klijentId].user.p.IzasaoIzKucice(idpi);
            IzvediIgraca(p, klijentId);
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
 
    private void SpawnPlayer(User c)
    {
        spawner.SpawnPlayer(c);
    }

    internal void PomeriZaBroj(int idpi, int fromClient, int broj)
    {
        try
        {

            Server.clients[fromClient].user.p.PomerioSeZaPolje(idpi, broj);
            Piece p = Server.clients[fromClient].user.p.player[idpi];
         
          
            IdiKaPolju(p, fromClient);
        }catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
   

    internal void PomeriUGlavnuKucicu(int uKuciciFinalDaIde, int idpi, int _fromClient)
    {
 
        Server.clients[_fromClient].user.p.UsaoUKucicuFinal(idpi, uKuciciFinalDaIde,_fromClient);
        UdjiUKucicuFinal(Server.clients[_fromClient].user.p.player[idpi],_fromClient);
    }

 
    internal void UdjiUKucicuFinal(Piece p,int clientId )
    {
        Debug.Log(p.currentField - 1);
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            Transform zeljenaPos = null;
            switch (p.color)
            {
                case colorType.RED: zeljenaPos = spawner.FinalHouseRed.transform.GetChild(p.currentField-1); break;
                case colorType.YELLOW: zeljenaPos = spawner.FinalHouseYellow.transform.GetChild(p.currentField-1); break;
                case colorType.BLUE: zeljenaPos = spawner.FinalHouseBlue.transform.GetChild(p.currentField-1); break;
                case colorType.GREEN: zeljenaPos = spawner.FinalHouseGreen.transform.GetChild(p.currentField-1); break;
            }

            if (zeljenaPos == null) Debug.Log("null je ");

            pomerajIgraca = zeljenaPos.transform.position;
            zaPomeraj = p;
            PacketSender.SendWhereToIdes(zeljenaPos, clientId, p.id,true,p.currentField);
        });
    }
}
