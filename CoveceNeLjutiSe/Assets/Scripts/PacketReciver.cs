using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PacketReciver
{
    static PacketReciver instance;

    internal static void ReciveID(Packet _packet)
    {
        
        int id = _packet.ReadInt();
        Client.instance.user.id = id;
        PacketSender.SendUsername(id);
    }

    internal static void UserJoined(Packet _packet)
    {
        string name = _packet.ReadString();
        int id = _packet.ReadInt();
        int color = _packet.ReadInt();

        AddToPLayerInfo(id, name,color);
       
        UI_Manager_Room.instance.DodatKlijent(name,id,color);
    }

    private static void AddToPLayerInfo(int id, string name,int color)
    {
        User u = new User();
        u.username = name;
        u.id = id;
        u.colorType = (colorType)color;
        PlayersInfo.instance.AddPlayer(id,u );
    }

    internal static void ClientDisconected(Packet _packet)
    {
        int id = _packet.ReadInt();

        PlayersInfo.instance.RemovePlayer(id);
        UI_Manager_Room.instance.DeleteClientOnDisconnect(id);
    }

    internal static void ServerDownHandler(Packet _packet)
    {
        Client.instance.Disconnect();

        Sceneamager.instance.LoadMainScene();
    }

    internal static void UpdateColor(Packet _packet)
    {
        int id = _packet.ReadInt();
        int color = _packet.ReadInt();
        if (color == -1) { Debug.Log("Neko vec ima tu boju"); }
        else
        {

                PlayersInfo.instance.ChangePlayersInfo(id, (colorType)color);
                UI_Manager_Room.instance.ChangeColor(id, color);
            
        }
    }

    internal static void PositiveReadyHandle(Packet _packet)
    {
        int randomNum = _packet.ReadInt();
        Client.instance.user.whoIsFirst = randomNum;
        Sceneamager.instance.GameScene();
    }

    internal static void NegativeReadyHandle(Packet _packet)
    {
        string message = _packet.ReadString();
        Debug.Log(message);
    }

    internal static void SetAdmin(Packet _packet)
    {
       
        Client.instance.user.isAdmin = true;
        MainThreadManager.ExecuteOnMainThread(() =>
        {
            if (SceneManager.GetActiveScene().buildIndex == Information.scene_Loby)
            {
                UI_Manager_Room.instance.ChangeAdmin(true);
            }
        });
     
        
       
    }
    internal static void HandlePotez(Packet p)
    {
        int id = p.ReadInt();

        MainThreadManager.ExecuteOnMainThread(() =>
        {
            if (Client.instance.user.id == id)
            {
                GameManager.instance.PlayerTurn();
            }
            else
            {
                GameManager.instance.EnemieTurn();
            }

        });
        
      
    }

    internal static void HandleReadyChaned(Packet _packet)
    {
        int id = _packet.ReadInt();
        bool ready = _packet.ReadBool();
      
        UI_Manager_Room.instance.PromenaReadyStanja(id, ready);
    }

    internal static void HandleRandomNum(Packet _packet)
    {
        int id = _packet.ReadInt();
        int num = _packet.ReadInt();
        
        GameManager.instance.dice.StopRoll(num);
    }

    internal static void HandleIzlaz(Packet _packet)
    {
        int id = _packet.ReadInt();
        int idPi = _packet.ReadInt();
        float x = _packet.ReadFloat();
        float y = _packet.ReadFloat();
        if (id == Client.instance.user.id)
        {
           GameManager.instance.Move(idPi, x, y);
        }
        else
        {

            GameManager.instance.IzadjiIzKucice(id, idPi, new Vector2(x, y));            
        }
    }

    internal static void HandleWalk(Packet _packet)
    {
        int id = _packet.ReadInt();
        int idPi = _packet.ReadInt();
        float x = _packet.ReadFloat();
        float y = _packet.ReadFloat();
        bool isFinished = _packet.ReadBool(); 
      

        if (id == Client.instance.user.id)
        {
            if (isFinished)
            {
                GameManager.instance.UsaoUKucincu(idPi,_packet.ReadInt());
            }

            GameManager.instance.MoveForPolje(idPi, x, y);
        }
        else
        {
            if (isFinished)
            {
          
                GameManager.instance.PromeniVarijableNeprijatelja(id,idPi,_packet.ReadInt(), true);
            }

           GameManager.instance.MoveEnemy(id, idPi, new Vector2(x, y));
        }
    }

    internal static void PojedenNekoHandle(Packet _packet)
    {
        int id = _packet.ReadInt();
        int piece_id = _packet.ReadInt();
        float xspawn = _packet.ReadFloat();
        float yspawn = _packet.ReadFloat();
        
        if (id == Client.instance.user.id)
        {          
            GameManager.instance.VratiNaPocetakPlayer(piece_id,xspawn,yspawn);
        }
        else
        {           
            GameManager.instance.VratiNaPocetakEnemy(PlayersInfo.instance.Players[id].id,piece_id, xspawn, yspawn);
        }
    }

    internal static void PobedioNekoHandle(Packet _packet)
    {
        int idPobednika = _packet.ReadInt();
        string imePobednika = _packet.ReadString();
        Debug.Log("Pobednik je " + imePobednika);
       
    }

    internal static void ConnectionHandler(Packet _paket)
    {
        Client.instance.user.potvrdaKonekcije = true;
       
        PacketSender.SendConnectionResponse();

    }
}
