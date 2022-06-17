using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GCS_Comunication.Comunication
{
    public class Udp_Connect
    {
        private UdpClient _udpClient;
        private string _severIp;
        private int _serverPort;
        private IPEndPoint _serverEP;

        private bool _isOpen = false;
        public bool IsOpen
        {
            get { return _isOpen; }
        }

        public Udp_Connect(string ipAddress, int port) 
        {
            _severIp = ipAddress;
            _serverPort = port;
        }

        public void DoConnect()
        {
            try
            {
                if(_udpClient == null)
                {
                    _serverEP = new IPEndPoint(IPAddress.Parse(_severIp), _serverPort);
                    _udpClient = new UdpClient();
                    _udpClient.Connect(_serverEP);
                    byte[] data = Encoding.ASCII.GetBytes("First Connection");
                    SendData(data);
                }
                _isOpen = true;
            }
            catch
            {
                throw new Exception("Error when connect with UAV");
            }
        }

        public void DoDisconnect()
        {
            try
            {
                if (_udpClient != null && _udpClient.Client.IsBound)
                {
                    _udpClient.Close();
                    _udpClient.Dispose();
                    _udpClient = null;
                }
                _isOpen = false;
            }
            catch
            {
                throw new Exception("Error when disconnect with UAV");
            }
        }
        public int SendData(byte[] data)
        {
            int res = 0;
            try
            {
                if (_udpClient != null && _isOpen)
                    res = _udpClient.Send(data, data.Length);
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ReceiveData(out byte[] data)
        {
            data = _udpClient.Receive(ref _serverEP);
            return data.Length;
        }

    }
}
