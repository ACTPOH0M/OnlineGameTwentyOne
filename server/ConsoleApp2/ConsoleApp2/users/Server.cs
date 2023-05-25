using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ConsoleApp2
{
    public static class Server
    {
        public static List<Client> Clients = new List<Client>();
        public static Client user_in_list;
        public static Socket user_;
        public static void NewClient(Socket handle)
        {
            try
            {
                Client newClient = new Client(handle);
                user_in_list = newClient;
                Clients.Add(newClient);
                Console.WriteLine("New client connected: {0}", handle.RemoteEndPoint);
            }
            catch (Exception exp) { ErrorLog.Log(DateTime.Now + " Error with addNewClient: " + exp.Message); }
        }
        public static void EndClient(Client client)
        {
            try
            {
                string id = Clients.IndexOf(client).ToString();
                for(int i=0; i< Game.Rival.Rival_list.Count;i++)
                {
                    if(Game.Rival.Rival_list[i].id_player_connection == id)
                    {
                        Game.Rival.Rival_list.RemoveAt(i);
                        DataBase.DataBase.ChangeStatusUser();
                       
                    }
                }

                

                DataBase.ChangeStatus.ForceDisconect = true;
                //DataBase.DataBase.ChangeStatusUserDisconnet();
                DataBase.ChangeStatus.ForceDisconect = false;

                client.End();
                Clients.Remove(client);
                Console.WriteLine("User {0} has been disconnected.", client.UserName);
                UpdateAllIdUsers();
                
            }
            catch (Exception exp) { ErrorLog.Log(DateTime.Now + " Error with endClient: " + exp.Message); }
        }
        public static void UpdateAllIdUsers()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateId();
                }
            }
            catch (Exception exp) { Console.WriteLine("Error with updateAlLChats: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with updateAlLChats: " + exp.Message); }
        }
        public static void UpdateAllChats()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].UpdateChat();
                }
            }
            catch (Exception exp) { Console.WriteLine("Error with updateAlLChats: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with updateAlLChats: " + exp.Message); }
        }

        public static void UpdateUsersInGame(string id_user)
        {
            try
            {
                int i = Convert.ToInt32(id_user);
                Clients[i].UpdateGame();
            }
            catch (Exception exp) { Console.WriteLine("Error with UpdateUsersInGame: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with UpdateUsersInGame: " + exp.Message); }
        }
        public static void UpdateStatusInGame(string id_user)
        {
            try
            {
                int i = Convert.ToInt32(id_user);
                Clients[i].UpdateStatusGame();
            }
            catch (Exception exp) { Console.WriteLine("Error with UpdateStatusInGame: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with UpdateStatusInGame: " + exp.Message); }
        }
        public static void Game_(string id_user)
        {
            try
            {
                int i = Convert.ToInt32(id_user);
                Clients[i].Game_();
            }
            catch (Exception exp) { Console.WriteLine("Error with Game_ : {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with Game_: " + exp.Message); }
        }
        public static void Finish_game(string id_user)
        {
            try
            {
                int i = Convert.ToInt32(id_user);
                Clients[i].Finish_Game();
            }
            catch (Exception exp) { Console.WriteLine("Error with Finish_game: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with Finish_game: " + exp.Message); }
        }
        public static void UpdateGame(string id_user)
        {
            try
            {
                int i = Convert.ToInt32(id_user);
                Clients[i].Update_Game();
            }
            catch (Exception exp) { Console.WriteLine("Error with UpdateGame: {0}.", exp.Message); ErrorLog.Log(DateTime.Now + " Error with UpdateGame: " + exp.Message); }
        }
        public static void UpdateLobby()
        {
            try
            {
                int countUsers = Clients.Count;
                for (int i = 0; i < countUsers; i++)
                {
                    Clients[i].Update_Lobby();
                }
            }
            catch (Exception exp) {  ErrorLog.Log("Error with UpdateLobby: " + exp.Message); Console.WriteLine(DateTime.Now + " Error with UpdateLobby: {0}.", exp.Message); }
        }

    }
}
