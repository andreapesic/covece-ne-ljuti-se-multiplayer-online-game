using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PacketSender
{
    private  static int onMovePrev=-1;

    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }


 
    internal static void SendPojeoGa(int id, int mainParentId, Vector2 spawnPos)
    {
        Packet p = new Packet((int)Packet.ServerPackets.PojedenNeko);
        Debug.Log("Izbacujem " + id +" figuricu " + mainParentId);
        p.Write(mainParentId);
        p.Write(id);
        p.Write(spawnPos.x); 
        p.Write(spawnPos.y);
        p.clientId = mainParentId;
        p.type = packetType.ALL;

        AddToExecuted(p);
    }

    internal static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Information.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    internal static void SendDisconectedClient(int fromClient)
    {
        Packet p = new Packet((int)Packet.ServerPackets.DisconectedClient);
      
        p.Write(fromClient);
        p.clientId = fromClient;
        p.type = packetType.ALLEXCEPT;
      
        AddToExecuted(p);
    }

    internal static void SendPobedioPacket(int fromClient)
    {
        Packet p = new Packet((int)Packet.ServerPackets.PobedioPacket);
        p.Write(fromClient);
        p.Write(Server.clients[fromClient].user.username);
        p.clientId = fromClient;
        p.type = packetType.ALL;
        AddToExecuted(p);

    }

    internal static void SendConnectionPcg()
    {
        foreach(Client c in Server.clients.Values)
        {
            if (c.tcp.socket != null)
            {
                Packet _paket = new Packet((int)Packet.ServerPackets.ConnectionPcg);
                _paket.clientId = c.user.id;
                _paket.type = packetType.ALONE;

                AddToExecuted(_paket);
            }
        }
    
    }

    internal static void SendColorUpdate(int fromClient, int color)
    {
        Packet p = new Packet((int)Packet.ServerPackets.ColorUpdate);
        p.Write(fromClient);
        p.Write(color);
        p.clientId = fromClient;
        p.type = packetType.ALL;
        AddToExecuted(p);

    }

    internal static void SendWhereToIzadjes(Vector2 spawnPos,int clientId, int idpi)
    {

        Packet p = new Packet((int)Packet.ServerPackets.WhereToIzadjes);
        p.Write(clientId);
        p.Write(idpi);
        p.Write(spawnPos.x);
        p.Write(spawnPos.y);
        p.clientId = clientId;
     
        p.type = packetType.ALL;
        AddToExecuted(p);
    }

    internal static void ServerGoingDown(int id)
    {
        Packet p = new Packet((int)Packet.ServerPackets.ServerDown);

        p.Write(id);
        Debug.Log("Server down");
     
        SendTCPData(id, p);
    }

    internal static void SendWhereToIdes(Transform spawnPos, int clientId, int idpi,bool daLiJeGotov,int num)
    {
        Packet p = new Packet((int)Packet.ServerPackets.WhereToIdes);
        p.Write(clientId);
        p.Write(idpi);
        p.Write(spawnPos.position.x);
        p.Write(spawnPos.position.y);
        p.Write(daLiJeGotov);
        if (daLiJeGotov)
            p.Write(num);
        p.clientId = clientId;

        p.type = packetType.ALL;
        AddToExecuted(p);
    }

    internal static void SendAllClietsTOClient(int fromClient)
    {
       // Packet p = new Packet((int)Packet.ServerPackets.AllClients);
        foreach(KeyValuePair<int,Client> pair in Server.clients)
        {
            if(pair.Value.tcp.socket!=null && pair.Value.user.id!=fromClient)
                ClientJoined(pair.Key);
        }
     
    }
   
    internal static void SendReadyResponsePositive()
    {
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            Packet p = new Packet((int)Packet.ServerPackets.PositiveReadyResponse);
            int r = 0;
            
           do{//ko prvi igra
                r = UnityEngine.Random.Range(1, Information.MaxPlayers);
                
            } while (Server.clients[r].tcp.socket == null);
            p.type = packetType.ALL;
            onMovePrev = r;
            Server.clients[r].user.isOnMove = true;
            p.Write(r);
            AddToExecuted(p);
            Sceneamager.instance.GameScene();
        });

      
    }

    internal static void SendReadyResponseNegative(int id,string v)
    {
        Packet p = new Packet((int)Packet.ServerPackets.NegativeReadyResponse);
        p.Write(v);

        p.clientId=id;
        p.type = packetType.ALONE;
        
        AddToExecuted(p);
    }

    internal static void SendAdmin(int id)
    {
        Packet p = new Packet((int)Packet.ServerPackets.AdminPacket);
        p.Write(id);
        p.type = packetType.ALONE;
        p.clientId = id;
        AddToExecuted(p);
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Information.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    internal static void SendReadyChanged(int fromClient, bool readyState)
    {
        Packet p = new Packet((int)Packet.ServerPackets.ReadyClientChanged);
     
        p.Write(fromClient);
        p.Write(readyState);
        p.clientId = fromClient;
        p.type = packetType.ALLEXCEPT;
        AddToExecuted(p);

    }

    internal static void SendId(int i)
    {
        Packet p = new Packet((int)Packet.ServerPackets.IdSend);
        p.clientId = i;
        p.type = packetType.ALONE;
        p.Write(i);
       
        AddToExecuted(p);
        
        
    }
    internal static void ClientJoined(int id)
    {
      
        Packet p = new Packet((int)Packet.ServerPackets.ClientJoined);
        p.Write(Server.clients[id].user.username);
        p.Write(id);
        p.Write((int)Server.clients[id].user.color);
        p.clientId = id;
        p.type = packetType.ALLEXCEPT;
       
        AddToExecuted(p);

    }

    internal static void SendPlayerMove(int i)
    {
        Packet p = new Packet((int)Packet.ServerPackets.Potez);
        p.Write(i);
   

        p.clientId = i;
        p.type = packetType.ALL;
        AddToExecuted(p);
    
        }

    internal static void SendRandomNum(int random, int fromClient)
    {
        if (onMovePrev != -1 && fromClient != onMovePrev)
        {
           
            Server.clients[onMovePrev].user.isOnMove = false;
        }
       
        

            onMovePrev = fromClient;
            Server.clients[fromClient].user.isOnMove = true;
        

        Packet p = new Packet((int)Packet.ServerPackets.SendRandomNum);
        p.Write(fromClient);
        p.Write(random);

       
        p.clientId = fromClient;
        p.type = packetType.ALONE;
        AddToExecuted(p);
    }

    private static void AddToExecuted(Packet p)
    {

        PacketExecutor.instance.AddPacket(p);
    }
}
