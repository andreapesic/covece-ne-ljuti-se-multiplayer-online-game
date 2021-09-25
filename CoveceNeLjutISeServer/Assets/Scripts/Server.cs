using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
    public int numOfConnected = 0;
    public static Server instance;
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;
    public int randomFirst;
    private bool poslato = false;
    public bool Adminexists = false;
    private static TcpListener tcpListener;
    public Dictionary<int, bool> sendConnectionPCg;


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
  
    
    public void StopListenning()
    {
        tcpListener.Stop();
       
    }
   
    private void OnApplicationQuit()
    {
        foreach(Client c in clients.Values)
            if(c.tcp.socket!=null)
                PacketSender.ServerGoingDown(c.user.id);
    }
   
    
    private void Update()
    {
        if (!poslato && Server.clients.Count != 0)
        {

            poslato = true;
            PacketSender.SendConnectionPcg();
            //StartCoroutine(PosaljiConnectionRequest());

        }
        MainThreadManager.UpdateMain();
    }
    //public IEnumerator PosaljiConnectionRequest()
    //{

    //    yield return new WaitForSeconds(5);

    //    IzbaciIzNizaKoJeFalse();



    //}
    //private void IzbaciIzNizaKoJeFalse()
    //{
    //    try
    //    {
           

    //        foreach (KeyValuePair<int, bool> pair in sendConnectionPCg)
    //        {
                

    //            if (!pair.Value && clients[pair.Key].user.tekUsao)
    //            {
    //                clients[pair.Key].user.tekUsao = false;
    //            }
    //            else if (!pair.Value)
    //            {

    //                Debug.Log("Vrednost je " + pair.Value + pair.Key);
    //                DiskonektujNemanet(pair.Key);
    //                clients.Remove(pair.Key);
    //            }

    //        }
    //        foreach (Client c in clients.Values)
    //        {
    //            if(c.tcp.socket!=null)
    //                sendConnectionPCg[c.user.id] = false;
    //        }
    //        poslato = false;
    //    }
    //    catch (Exception e)
    //    {

    //    }

    //}
    //private void DiskonektujNemanet(int idKlijenta)
    //{
    //    if (SceneManager.GetActiveScene().buildIndex == 0)
    //    {
    //        PacketReciver.DiskonektSaMainScene(idKlijenta);

    //    }
    //    else if (SceneManager.GetActiveScene().buildIndex != 0)
    //    {

    //        PacketReciver.DiskonetIzIgre(idKlijenta);


    //    }
    //}

    public void Start()
    {

        DontDestroyOnLoad(this);
       

        Debug.Log("Starting server...");
        InitializeServerData();

        
       

       
      
        startListening();

        Debug.Log($"Server started on port {Information.Port}.");
    }


    private  void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        

        for (int i = 1; i <= Information.MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                Server.instance.sendConnectionPCg.Add(i,false) ;
               
                clients[i].tcp.Connect(_client);
                if (i == 1 && !Adminexists)
                {

                    clients[i].user.isAdmin = true;
                     Adminexists = true;
                    PacketSender.SendAdmin(i);
                }

                PacketSender.SendId(i);
                
                return;
            }
        }
     

      
    }

    internal void startListening()
    {
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
      
       
    }

   
    

    private  void InitializeServerData()
    {
        tcpListener = new TcpListener(IPAddress.Any, Information.Port);
        sendConnectionPCg = new Dictionary<int, bool>();
        for (int i = 1; i <= Information.MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
            {  
            {(int)Packet.ClientPackets.Username,PacketReciver.UsernameRecived },
              {(int)Packet.ClientPackets.Disconect,PacketReciver.ClientDisconected },
            {(int)Packet.ClientPackets.RequestAllClients,PacketReciver.SendAllClientsTo },

            {(int)Packet.ClientPackets.ColorChoosed,PacketReciver.ClientCoosedColor },
                  {(int)Packet.ClientPackets.RequestReady,PacketReciver.ReadyHandle },

     {(int)Packet.ClientPackets.ReadyChanged,PacketReciver.ReadyChangedHandle },

     {(int)Packet.ClientPackets.RequestRandomNum,PacketReciver.RandomNumHandle },
         {(int)Packet.ClientPackets.TurnOver,PacketReciver.TurnOverHandle },
          {(int)Packet.ClientPackets.IzbaciIzKuce,PacketReciver.IzbaciIzKuceHandle },
           {(int)Packet.ClientPackets.PomeriZaBroj,PacketReciver.PomeriZaBrojHandle },
        {(int)Packet.ClientPackets.ConnectionResponsePcg,PacketReciver.ConnectionResponseHandle }

        };

        Debug.Log("Initialized packets.");
    }

}
