using System;
using System.Net;
using System.Net.Sockets;

namespace ModbusTools.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ipAddress = IPAddress.Parse("127.0.0.1");
            var ipEndPoint = new IPEndPoint(ipAddress, 502);

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
            var endPoint = (EndPoint)ipEndPoint;

            socket.Connect(endPoint);

            if (socket.Connected)
            {
                //var request = CreateReadHeader(1, 1, 0, 10);
                //this._socketCliect.Send(request);
                //var result = Receive();
            }
        }
    }
}