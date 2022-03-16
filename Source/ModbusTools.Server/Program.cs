using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ModbusTools.Server
{
    internal class Program
    {
        private TcpListener tcpListener;

        static void Main(string[] args)
        {
            var program = new Program();
            program.Initialize();
        }

        public void Initialize()
        {
            if (this.tcpListener == null)
            {
                this.tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 502);
                this.tcpListener.Start();

                Console.WriteLine("this.tcpListener.Start():502");

                while (true)
                {
                    var tcpClient = tcpListener.AcceptTcpClient();
                    System.Threading.Tasks.Task.Factory.StartNew(() => DoListener(tcpClient));
                    Thread.Sleep(100);
                }
            }
        }

        public void DoListener(TcpClient tcpClient)
        {
            tcpClient.SendTimeout = 30000;
            tcpClient.ReceiveTimeout = 30000;

            try
            {
                tcpClient.Client.Poll(0, SelectMode.SelectRead);
                byte[] testRecByte = new byte[1];

                while (tcpClient.Connected)
                {
                    if (tcpClient.Client.Receive(testRecByte, SocketFlags.Peek) == 0)
                        break;

                    var data = this.DoRead(tcpClient);
                    data = this.DoSend(tcpClient, data);
                    
                    Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public Byte[] DoRead(TcpClient tcpClient)
        {
            var size = tcpClient.Client.ReceiveBufferSize;
            var temp = new byte[size];
            var count = tcpClient.Client.Receive(temp, size, SocketFlags.None);

            var data = new byte[count];
            for (int i = 0; i < count; i++)
                data[i] = temp[i];

            Console.WriteLine("TX：" + BitConverter.ToString(data));

            return data;
        }

        public byte[] DoSend(TcpClient tcpClient, byte[] data)
        {
            for (int i = 11; i < data.Length; i++)
                data[i] = 0x00;

            var result = data;

            //var crc16 = this.GetCRC16ByLH(result);

            ////// TODO: 希望這個可以回應網站對我的說得位元組問題 - _ -!!
            //result = result.Append(crc16[0]).ToArray();
            //result = result.Append(crc16[1]).ToArray();

            tcpClient.Client.Send(result, SocketFlags.None);
            Console.WriteLine("RX：" + BitConverter.ToString(result));

            return data;
        }

        public byte[] GetCRC16ByLH(byte[] pDataBytes)
        {
            ushort crc = 0xffff;
            ushort polynom = 0xA001;

            for (int i = 0; i < pDataBytes.Length; i++)
            {
                crc ^= pDataBytes[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x01) == 0x01)
                    {
                        crc >>= 1;
                        crc ^= polynom;
                    }
                    else
                    {
                        crc >>= 1;
                    }
                }
            }

            byte[] result = BitConverter.GetBytes(crc);
            return result;
        }
    }
}
