using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Client :MonoBehaviour
{
    public static Client instance;

    public User user;
    public TCP tcp;
  

    private bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;



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

    private void Start()
    {
        if(instance!=null)
            DontDestroyOnLoad(this);
        user = new User();
        tcp = new TCP();
        
    }

    private void OnApplicationQuit()
    {
        PacketSender.SendDisconect(user.id);

      
    }

    private void Update()
    {
     
        //if (!user.zapoceto && !user.tekUsao)
        //    StartCoroutine(PosaljiConnectionRequest());
        MainThreadManager.UpdateMain();
    }
    //public IEnumerator PosaljiConnectionRequest()
    //{
    //    //pocinje korutina u loby i ako kada se vratimo u main ne sacekamo tipa 5 sekundi da
    //    //prodje sve ovo on kada udje opet u loby a nije isteklo 5 s ulazi da izbaci iz igre

    //    user.zapoceto = true;
    //    yield return new WaitForSeconds(5);
    //    if (!user.potvrdaKonekcije && SceneManager.GetActiveScene().buildIndex!=Information.scene_main)
    //    {
    //        Debug.Log(user.potvrdaKonekcije);
    //        if (SceneManager.GetActiveScene().buildIndex == Information.scene_Loby)
    //            UI_Manager_Room.instance.Back();
    //        else if (SceneManager.GetActiveScene().buildIndex >= Information.game_scene)
    //        {
    //            GameManager.instance.Disconect();
    //        }
    //    }
    //    user.potvrdaKonekcije = false;
    //    user.zapoceto = false;


    //}

    public void ConnectToServer()
    {
        InitializeClientData();

        isConnected = true;

        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP()
        {
            receivedData = new Packet();
        }
        public void Connect()
        {
            Debug.Log("Pocinjem Konekciju");
            socket = new TcpClient()
            {
                ReceiveBufferSize = Information.dataBufferSize,
                SendBufferSize = Information.dataBufferSize
            };

            receiveBuffer = new byte[Information.dataBufferSize];
            socket.BeginConnect(Information.ip, Information.port, ConnectCallback, null);
            
        }

        
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);
           
            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();


           

            stream.BeginRead(receiveBuffer, 0, Information.dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null); 
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to server via TCP: {_ex}");
            }
        }
     

            private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {

                if (stream == null) return;
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    //instance.Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                //posle malo
                receivedData.Reset(HandleData(_data));

                stream.BeginRead(receiveBuffer, 0, Information.dataBufferSize, ReceiveCallback, null);
            }
            catch(Exception e )
            {
                Debug.Log(e.StackTrace);
            }
        }


        private bool HandleData(byte[] _data)
        {
            try
            {
                int _packetLength = 0;

                receivedData.SetBytes(_data);

                if (receivedData.UnreadLength() >= 4)
                {

                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {

                        return true;
                    }
                }

                while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
                {

                    byte[] _packetBytes = receivedData.ReadBytes(_packetLength);

                    Packet _packet = new Packet(_packetBytes);

                    int _packetId = _packet.ReadInt();
                   // Debug.Log(_packetId);
                    packetHandlers[_packetId](_packet);



                    _packetLength = 0;
                    if (receivedData.UnreadLength() >= 4)
                    {

                        _packetLength = receivedData.ReadInt();
                        if (_packetLength <= 0)
                        {

                            return true;
                        }
                    }
                }
                if (_packetLength <= 1)
                {
                    return true;
                }
            }
           


            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

          

            return false;
        }

   
        public void Disconnect()
        {
           
            stream = null;
          
            socket = null;
        }
    }


    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            {(int)Packet.ServerPackets.IdSend,PacketReciver.ReciveID},
            {(int)Packet.ServerPackets.ClientJoined,PacketReciver.UserJoined},
            {(int)Packet.ServerPackets.DisconectedClient,PacketReciver.ClientDisconected},
            {(int)Packet.ServerPackets.ServerDown,PacketReciver.ServerDownHandler},
            {(int)Packet.ServerPackets.ColorUpdate,PacketReciver.UpdateColor},
            {(int)Packet.ServerPackets.PositiveReadyResponse,PacketReciver.PositiveReadyHandle },
            {(int)Packet.ServerPackets.NegativeReadyResponse,PacketReciver.NegativeReadyHandle },

            {(int)Packet.ServerPackets.AdminPacket,PacketReciver.SetAdmin },

            {(int)Packet.ServerPackets.ReadyClientChanged,PacketReciver.HandleReadyChaned },


            {(int)Packet.ServerPackets.SendRandomNum,PacketReciver.HandleRandomNum },

            {(int)Packet.ServerPackets.Potez,PacketReciver.HandlePotez },
            {(int)Packet.ServerPackets.WhereToIzadjes,PacketReciver.HandleIzlaz },

            {(int)Packet.ServerPackets.WhereToIdes,PacketReciver.HandleWalk },
            {(int)Packet.ServerPackets.PojedenNeko,PacketReciver.PojedenNekoHandle},
            {(int)Packet.ServerPackets.PobedioPacket,PacketReciver.PobedioNekoHandle},
              { (int)Packet.ServerPackets.ConnectionPcg,PacketReciver.ConnectionHandler },

        };
        Debug.Log("Initialized packets.");
    }

 
    public void Disconnect()
    {
        if (isConnected)
        {
            PlayersInfo.instance.Clear();
            isConnected = false;
            user.restartinfo();
            tcp.Disconnect();        
            Debug.Log("Disconnected from server.");
            Sceneamager.instance.LoadMainScene();
        }
    }
}
