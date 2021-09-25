using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketSender 
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

   
  

    internal static void SendUsername(int id)
    {
        Packet p = new Packet((int)Packet.ClientPackets.Username);
      
        p.Write(Client.instance.user.username);
        PacketExecutor.instance.AddPacket(p);
        Sceneamager.instance.LoadRoom();
    }

    internal static void ColorChoosed(int color)
    {
        Packet p = new Packet((int)Packet.ClientPackets.ColorChoosed) ;

        p.Write(color);
        p.Write(Client.instance.user.id);

        PacketExecutor.instance.AddPacket(p);
    }

    internal static void SendDiceRollStarted()
    {
        
        Packet p = new Packet((int)Packet.ClientPackets.RequestRandomNum);
      
        p.Write(Client.instance.user.id);
       

        PacketExecutor.instance.AddPacket(p);
    }

    internal static void SendOverTurn(int id)
    {
        Packet p = new Packet((int)Packet.ClientPackets.TurnOver);

        p.Write(id);


        PacketExecutor.instance.AddPacket(p);
    }

    internal static void SendDisconect(int id)
    {

        Packet p = new Packet((int)Packet.ClientPackets.Disconect);
        p.Write(id);
       
        SendTCPData(p);
        
    }

    internal static void RequestAllClients()
    {
        Packet p = new Packet((int)Packet.ClientPackets.RequestAllClients);

        p.Write(Client.instance.user.id);
        PacketExecutor.instance.AddPacket(p);
        
    }

    internal static void RequestReady()
    {
        Packet p = new Packet((int)Packet.ClientPackets.RequestReady);
      
        p.Write(Client.instance.user.id);
        PacketExecutor.instance.AddPacket(p);
    }

    internal static void SendReadyChanged()
    {
        Packet p = new Packet((int)Packet.ClientPackets.ReadyChanged);
        p.Write(Client.instance.user.isReady);
        p.Write(Client.instance.user.id);
        
        PacketExecutor.instance.AddPacket(p);


    }

    internal static void IzbaciIzKuce(int idpi)
    {
        Packet p = new Packet((int)Packet.ClientPackets.IzbaciIzKuce);
  
        p.Write(Client.instance.user.id);
        p.Write(idpi);

        PacketExecutor.instance.AddPacket(p);
    }

    internal static void PomeriZaBrojPolja(int idpi, int odigranBroj)
    {
        Packet p = new Packet((int)Packet.ClientPackets.PomeriZaBroj);

        p.Write(Client.instance.user.id);
        p.Write(idpi);
        p.Write(odigranBroj);

        PacketExecutor.instance.AddPacket(p);
    }

    internal static void SendConnectionResponse()
    {
        Packet paket = new Packet((int)Packet.ClientPackets.ConnectionResponsePcg);
        paket.Write(Client.instance.user.id);
        PacketExecutor.instance.AddPacket(paket);
    }
}
