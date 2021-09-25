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
        if (paketi.Count != 0)
        {
            ExecutePacket(paketi.Dequeue());
        }
    }

    private void ExecutePacket(Packet packet)
    {
        packet.WriteLength();
    if(Client.instance.tcp.socket!=null)
        Client.instance.tcp.SendData(packet);
    }
}
