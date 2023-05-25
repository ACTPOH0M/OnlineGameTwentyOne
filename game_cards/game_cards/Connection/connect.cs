using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace game_cards.connection
{
    class connect
    {
        public static Socket _serverSocket;
        static int port = 9933; // порт сервера
        static string address = "127.0.0.1"; // адрес сервера
        public static void Connect()
        {
            try
            {
                    IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                    _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // подключаемся к удаленному хосту
                    _serverSocket.Connect(ipPoint);
                
            }
            catch (Exception e)
            {
                ErrorLog.ErrorLogWriter("Error with connect " + e.Message);
            }
        }

        
    }
}
