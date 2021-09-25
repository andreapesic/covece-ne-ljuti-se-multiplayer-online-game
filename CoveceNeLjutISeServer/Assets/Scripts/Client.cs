using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client 
{
    
    public User user;
    public TCP tcp;
 

    public Client(int _clientId)
    {
        user = new User();
        user.id = _clientId;
        tcp = new TCP(user.id);
   
    }
  
  
    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

       
        public void Connect(TcpClient _socket)
        {
         
           
            socket = _socket;
            socket.ReceiveBufferSize = Information.dataBufferSize;
            socket.SendBufferSize = Information.dataBufferSize;
            
            stream = socket.GetStream();
            
            receivedData = new Packet();
            receiveBuffer = new byte[Information.dataBufferSize];
           
            Debug.Log("Konektovan");
            Ui_Manager.instance.DodajIgraca();
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
                Console.WriteLine($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

   
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.clients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data)); 
                stream.BeginRead(receiveBuffer, 0, Information.dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"Error receiving TCP data: {_ex}");
                Server.clients[id].Disconnect();
            }
        }

       
        private bool HandleData(byte[] _data)
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
                        Server.packetHandlers[_packetId](id, _packet); 
                    
               

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

            return false;
        }

    
        public void Disconnect()
        {
            if(socket!=null)
                socket.Close();
            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
           
        }
    }

   

   
    public void Disconnect()
    {
       
      
        user.reset();
      
       
        tcp.Disconnect();
       
    }

}
