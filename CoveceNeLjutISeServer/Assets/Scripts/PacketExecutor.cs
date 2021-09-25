using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketExecutor :MonoBehaviour
{
    public static PacketExecutor instance;
    Queue<Packet> paketi;
  
  
    private void Awake()
    {
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
    public void Clear()
    {
        paketi.Clear();
    }
    public void AddPacket(Packet p)
    {
     
        paketi.Enqueue(p);
    }
    public void  Start()
    {
        paketi = new Queue<Packet>();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
       
        if (paketi!=null && paketi.Count != 0)
        {
           
            ExecutePacket(paketi.Dequeue());
        }
    }

    private void ExecutePacket(Packet packet)
    {
        packet.WriteLength();      


        switch (packet.type)
        {
            case packetType.ALONE:AloneSentMethod(packet); break;
            case packetType.ALL: AllSentMethod(packet); break;
            case packetType.ALLEXCEPT:  AllExceptSentMethod(packet); break;

        }
       
    }

    private void AllExceptSentMethod(Packet packet)
    {
        foreach (KeyValuePair<int, Client> c in Server.clients)
        {

            if (c.Value.tcp.socket != null && c.Key!=packet.clientId)
            {
             
                Server.clients[c.Key].tcp.SendData(packet);
            }
               
        }
    }

    private void AllSentMethod(Packet packet)
    {
        foreach(KeyValuePair<int,Client> c in Server.clients)
        {
            if(c.Value.tcp.socket != null)
                Server.clients[c.Key].tcp.SendData(packet);
        }
    }

    private void AloneSentMethod(Packet packet)
    {
        Server.clients[packet.clientId].tcp.SendData(packet);
    }

 
}
public enum packetType
{
    ALONE,
    ALL,
    ALLEXCEPT
}
