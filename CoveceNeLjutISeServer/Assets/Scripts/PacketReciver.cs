using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PacketReciver
{
    internal static void UsernameRecived(int _fromClient, Packet _packet)
    {

        string name = _packet.ReadString();
        Server.clients[_fromClient].user.username = name;
        PacketSender.ClientJoined(_fromClient);
    }

    internal static void ClientDisconected(int _fromClient, Packet _packet)
    {
        

        MainThreadManager.ExecuteOnMainThread(() =>
        {
            
            if (SceneManager.GetActiveScene().buildIndex == Information.MainScene)
            {

                DiskonektSaMainScene(_fromClient);
            }
            else
            {
               
                DiskonetIzIgre(_fromClient);

            }
        });
        
    }

    public static void DiskonetIzIgre(int _fromClient)
    {
        GameManager.instance.DeletePiece(_fromClient);

      
        if (Server.instance.numOfConnected == 1)
        {
            int cId = Server.clients.First(x => x.Value.tcp.socket != null).Value.user.id;
            PacketSender.ServerGoingDown(cId);
            Server.clients[cId].Disconnect();
            Server.instance.sendConnectionPCg.Clear();
            Server.instance.startListening();
            Server.instance.numOfConnected--;
            Server.instance.Adminexists = false;          
            Sceneamager.instance.LoadMainScene();
        }
        else if (Server.instance.numOfConnected == 0)
        {
            Server.instance.sendConnectionPCg.Clear();
            Server.instance.startListening();
            Sceneamager.instance.LoadMainScene();
            Server.instance.Adminexists = false;
        }
        else
        {
            Server.instance.sendConnectionPCg.Remove(_fromClient);
            PacketSender.SendDisconectedClient(_fromClient);
            Server.instance.Adminexists = false;
        }
    }

    public static void DiskonektSaMainScene(int _fromClient)
    {
        
        bool admin = Server.clients[_fromClient].user.isAdmin;
        Server.clients[_fromClient].Disconnect();
        Server.instance.sendConnectionPCg.Remove(_fromClient);

        if (admin)
        {
            
            Client c = Server.clients.FirstOrDefault(x =>x.Value.tcp.socket!=null  && x.Value.user.id != _fromClient).Value;
            if (c != null)
            {
                Debug.Log("Namestam na admina" + c.user.username);

                c.user.isAdmin = true;
                PacketSender.SendAdmin(c.user.id);
            }else
            {
                Server.instance.Adminexists = false;
            }
          
        
              
        }
        Ui_Manager.instance.OduzmiIgraca();
        PacketSender.SendDisconectedClient(_fromClient);
    }

    internal static void SendAllClientsTo(int _fromClient, Packet _packet)
    {

        PacketSender.SendAllClietsTOClient(_fromClient);
        


    }

    internal static void ClientCoosedColor(int _fromClient, Packet _packet)
    {
        int color = _packet.ReadInt();

        if (CheckColorAvailability(color))
        {
            Server.clients[_fromClient].user.color = (colorType)color;
        }
        else color = -1;

        PacketSender.SendColorUpdate(_fromClient,color);
    }

    private static bool CheckColorAvailability(int color)
    {
        foreach(Client c in Server.clients.Values)
        {
            if ( c.tcp.socket!=null && c.user.color == (colorType)color)
            {
                return false;
            }
        }
        return true;
        
    }

    internal static void ReadyHandle(int _fromClient, Packet _packet)
    {      
        string clientCausing = "";
        int errorType= CanStart(ref clientCausing);

        switch (errorType)
        {
            case -1: PacketSender.SendReadyResponsePositive(); break;
            case 1: PacketSender.SendReadyResponseNegative(_fromClient,clientCausing +" Didnt select color"); break;
            case 0: PacketSender.SendReadyResponseNegative(_fromClient,clientCausing + " isnt ready"); break;
        }
    }
   
    private static int CanStart( ref string  clientid)
    {
       foreach(Client c in Server.clients.Values)
        {
            if (c.tcp.socket != null)
            {
                if (!c.user.isReady)
                {
                    clientid = c.user.username;
                    return 0;
                }
                if (c.user.color == 0)
                {
                    clientid = c.user.username;
                    return 1;
                }
            }
        }
        return -1;
    }

    internal static void ReadyChangedHandle(int _fromClient, Packet _packet)
    {
        bool readyState = _packet.ReadBool();
        Server.clients[_fromClient].user.isReady = readyState;
        
        PacketSender.SendReadyChanged(_fromClient,readyState);
    }

    internal static void RandomNumHandle(int _fromClient, Packet _packet)
    {


        MainThreadManager.ExecuteOnMainThread(() =>
        {

            int random = UnityEngine.Random.Range(1, 7);
            PacketSender.SendRandomNum(random, _fromClient);
        });

    }
    //ko sledeci igra
    internal static void TurnOverHandle(int _fromClient, Packet _packet)
    {
        int i;
        for(i=_fromClient+1; i <= Information.MaxPlayers; i++)
        {
            if (Server.clients[i].tcp.socket != null)
            {
                //his turn;
                PacketSender.SendPlayerMove(i);
                return;
            }
        }
        i = 1;
        //ako ne nadje nikog posle trenutnog ide od prvog
        //1 je na potezu
        PacketSender.SendPlayerMove(i);

    }

    internal static void ConnectionResponseHandle(int _fromClient, Packet _packet)
    {

        Server.instance.sendConnectionPCg[_fromClient] = true;
    }

    internal static void IzbaciIzKuceHandle(int _fromClient, Packet _packet)
    {
        int idKlijenta = _packet.ReadInt();
        int idpi = _packet.ReadInt();

        GameManager.instance.IzbaciIzKucice(idpi, _fromClient);
      
    }

    internal static void PomeriZaBrojHandle(int _fromClient, Packet _packet)
    {
        int idklijenta = _packet.ReadInt();
        int idpi = _packet.ReadInt();
        int broj = _packet.ReadInt();
        Piece p = Server.clients[_fromClient].user.p.player[idpi];

        if (p.currentField + broj > 39 )
        {
            int trebaDaIdeDoKrajaMape = 39 - p.currentField;
            int uKuciciFinalDaIde = Math.Abs(trebaDaIdeDoKrajaMape - broj);
            if (uKuciciFinalDaIde >= 5) return;
            GameManager.instance.PomeriUGlavnuKucicu(uKuciciFinalDaIde, idpi, _fromClient);

        }else if (p.isFinished && p.currentField+broj<=4)
        {
            p.currentField +=broj;

            GameManager.instance.PomeriUGlavnuKucicu(p.currentField, idpi, _fromClient);
        }else if(p.isFinished && p.currentField + broj > 4)
        {
            return;
        }
        else
        {
            GameManager.instance.PomeriZaBroj(idpi, _fromClient, broj);
        }
      

    }
}
