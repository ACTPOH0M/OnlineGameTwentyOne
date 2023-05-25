using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp2
{
    public class Client
    {
        public static string[] str;
        public static Client user; //определенный пользователь
        private string _userName;
        private  Socket _handler;
        private Thread _userThread;
        
        public Client(Socket socket)
        {
             user = this;
            _handler = socket;
            _userThread = new Thread(listner);
            _userThread.IsBackground = true;
            _userThread.Start();
        }
        public string UserName
        {
            get { return _userName; }
        }
        private void listner()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = _handler.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    
                    HandleCommand(data);
                    
                }
                //доделать обработчик выхода из сервера удаление из списка игры
                catch(Exception e) { Console.WriteLine(e.Message); ErrorLog.Log(DateTime.Now+" Error with listner: " + e.Message); Server.EndClient(this); return; }
            }
        }
        public void End()
        {
            try
            {
                _handler.Close();
                try
                {
                    _userThread.Abort();
                }
                catch { } 
            }
            catch (Exception exp) { Console.WriteLine("Error with end: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with end: " + exp.Message); }
        }

        private void HandleCommand(string data)
        {
             str = data.Split('&');
            Console.WriteLine(data);
            switch (str[0])
            {
                
                case ("#setname"):
                    {
                        _userName = data.Split('&')[1];
                        UpdateChat();
                        return;
                    }
                case ("#newmsg"):
                    {
                        string message = data.Split('&')[1];
                        ChatController.AddMessage(_userName, message);
                        return;
                    }
                case ("#login"):
                    {
                        DataBase.DataBase.Connect();
                        Send(Login_register.Login.output());
                        return;
                    }
                case ("#register"):
                    {
                        DataBase.DataBase.Connect();
                        Send(Login_register.Register.output());
                        return;
                    }
                case ("#find_lobby"):
                    {
                        DataBase.DataBase.Connect();
                        Send("#find_lobby&"+lobby.Find_lobby.info_from_database());
                        return;
                    }
                case ("#create_lobby"):
                    {
                        DataBase.DataBase.Connect();
                        Send(lobby.Create_lobby.info_about_create_lobby());
                        return;
                    }
                case ("#connect_to_lobby"):
                    {
                        DataBase.DataBase.Connect();
                        Send(lobby.Connect_to_lobby._connect());

                        if (lobby.Connect_to_lobby.str == "1")
                        {
                            Game.Rival.Add_game();
                        }
                        return;
                    }
                case ("#game"):
                    {
                        Game.Rival.game();
                        return;
                    }
                case ("#leaving_game"):
                    {
                        DataBase.DataBase.Connect();
                        Send(lobby.Leaving_from_lobby._leaving());
                        if (lobby.Connect_to_lobby.str == "1")
                        {
                            Game.Rival.Leaving_game();
                            Game.Rival.game();
                        }
                        return;
                    }
                case ("#update_status_game"):
                    {
                        Game.Rival.Update_game();
                        return;
                    }
                case ("#take_card"):
                    {
                        Game.Rival.Cards_game();
                        return;
                    }
                case ("#stop_game"):
                    {
                        Game.Rival.Stop();
                        return;
                    }
                case ("#closeapp"):
                    {
                        DataBase.DataBase.ChangeStatusUser();
                        Send(DataBase.ChangeStatus.Data());
                        return;
                    }
                case ("#update_game"):
                    {
                        Game.UserDisconnectInGame.UpdateList();
                        return;
                    }
                case ("#update_balance"):
                    {
                        DataBase.DataBase.Connect();
                        Send(DataBase.Balance.info());
                        return;
                    }
                case ("#balance"):
                    {
                        DataBase.DataBase.Connect();
                        Send("#balance&"+DataBase.Balance.info());
                        return;
                    }
                case ("#result_of_games"):
                    {   
                        DataBase.DataBase.Connect();
                        Send("#result_of_games&" + lobby.Find_lobby.info_from_database());
                        return;
                    }
            }

        }
        public void UpdateId()
        {
            
            Send("#updateid&"+Server.Clients.IndexOf(this).ToString());
        }
        public void UpdateChat()
        {
            Send(ChatController.GetChat());
        }
        public void UpdateGame()
        {
            Send("#game_room&"+ Game.Rival.data);
        }
        public void UpdateStatusGame()
        {   
            Send("#game_status_room&" + Game.Rival.data);
        }
        public void Game_()
        {
            Send("#game&" + Game.Rival.data);
        }
        public void Finish_Game()
        {
            Send("#finish_game&" + Game.Rival.data);
        }
        public void Update_Game()
        {
            Send("#update_game&" + Game.UserDisconnectInGame.players);
        }
        public void Update_Lobby()
        {
            Send( lobby.Find_lobby.info_from_database());
        }
        public void Send(string command)
        {
            try
            {
                int bytesSent = _handler.Send(Encoding.UTF8.GetBytes(command));
                if (bytesSent > 0) Console.WriteLine("Success");
            }
            catch (Exception exp) { Console.WriteLine("Error with send command: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with send command: " + exp.Message); Server.EndClient(this); }
        }
    }
}
