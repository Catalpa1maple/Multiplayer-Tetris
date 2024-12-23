using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Microsoft.VisualBasic;
using System.IO;

namespace TCP
{
    [Serializable]
    public struct Message
    {
        public int score;
        public int lineToSend;
    }

    public class ConnectionClosedException : Exception
    {

    }

    public class TCPSocket
    {
        private Socket? sockfd;
        private Socket? listener;
        private int port = 3080;

        public void TCPconnect(string IP)
        {
            try
            {
                sockfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sockfd.Connect(IPAddress.Parse(IP), port);
                Console.WriteLine("success to connect\n");
            }
            catch
            {
                //handle exception
                sockfd?.Close();
                Console.WriteLine("failed to connect");
                throw;
            }
        }

        public void TCPlisten()
        {
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint localAddr = new IPEndPoint(IPAddress.Any, 3080);
                listener.Bind(localAddr);
                listener.Listen(1);
                Console.WriteLine("listening...");
            }
            catch
            {
                //handle exception
                listener?.Close();
                Console.WriteLine("failed to listen");
                throw;
            }
            //TCPAccept();
            TCPaccept();
        }

        private async void TCPaccept()
        {
            if (listener != null)
            {
                try
                {
                    sockfd = await listener.AcceptAsync();
                    Console.WriteLine("success accepted");
                }
                catch
                {
                    sockfd?.Close();
                    Console.WriteLine("failed to accept");
                    throw;
                }
            }
        }

        private void TCPAccept()
        {
            if (listener != null)
            {
                try
                {
                    sockfd = listener.Accept();
                    Console.WriteLine("success accepted");
                }
                catch
                {
                    sockfd?.Close();
                    Console.WriteLine("failed to accept");
                    throw;
                }
            }
        }

        public Message TCPupdate(Message message)
        {
            try
            {
                if (sockfd != null)
                {
                    int r;
                    byte[] sendBuffer = structToBinary(message);
                    byte[] recvBuffer = new byte[256];
                    //send
                    r = sockfd.Send(sendBuffer);
                    if (r == 0) throw new ConnectionClosedException();
                    //recv
                    r = sockfd.Receive(recvBuffer);
                    if (r == 0) throw new ConnectionClosedException();

                    message = BinaryToStruct(recvBuffer);
                }
                return message;
            }
            catch
            {
                //handle send/recv exception 
                Console.WriteLine("Send/Recv Error");
                sockfd?.Close();
                listener?.Close();
                throw;
                //return message;
            }

        }

        private static byte[] structToBinary(Message message)
        {
            using (MemoryStream memstream = new MemoryStream())
            {
                using (BinaryWriter binwriter = new BinaryWriter(memstream))
                {
                    binwriter.Write(message.score);
                    binwriter.Write(message.lineToSend);
                }
                return memstream.ToArray();
            }
        }

        private static Message BinaryToStruct(byte[] input)
        {
            using (MemoryStream memstream = new MemoryStream(input))
            {
                using (BinaryReader binreader = new BinaryReader(memstream))
                {
                    Message message;
                    message.score = binreader.ReadInt32();
                    message.lineToSend = binreader.ReadInt32();
                    return message;
                }
            }
        }

        public void TCPClose()
        {
            sockfd?.Close();
            listener?.Close();
        }

        public bool TCPConnected()
        {
            if (sockfd != null && sockfd.Connected)
            {
                return true;
            }
            else return false;
        }
    }
}