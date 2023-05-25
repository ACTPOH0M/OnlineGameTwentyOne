using ConsoleApp2.lobby;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2.Game
{
    public class Rival
    {
        //id комнаты - id пользователя на сервере
        public static List<players_in_game> Rival_list = new List<players_in_game>();
        public static string data;
        public static Thread thread;
        public struct players_in_game
        {
            public string userName;
            public string id_room;
            public string id_player_connection;
            public string status;
            public string max_players;
            public string score;
            public players_in_game(string Id_room, string Id_player_connection, string UserName, string Status, string Max_players, string Score)
            {
                id_room = Id_room;
                id_player_connection = Id_player_connection;
                userName = UserName;
                status = Status;
                max_players = Max_players;
                score = Score;
            }
        }

        public static void Add_game()
        {
            try
            {
                //проверить
                for(int i=0; i< Rival_list.Count;i++)
                {
                    if(Client.str[3] == Rival_list[i].id_player_connection)
                    {
                        Rival_list.RemoveAt(i);
                    }
                }

                players_in_game newPlayer = new players_in_game(Client.str[1], Client.str[3], Client.str[2], Client.str[4], Connect_to_lobby.max_players, Client.str[6]);
                Rival_list.Add(newPlayer);

            } catch (Exception exp)
            {
                ErrorLog.Log(DateTime.Now + " Error with add to game: " + exp.Message);
                Console.WriteLine("Error with add to game " + exp.Message);
            }
        }

        public static void Update_game()
        {
            data = "";
            try
            {

                int players_in_room = 0, players_ready = 0;
                string max = "0";
                players_in_game updatePlayer = new players_in_game(Client.str[1], Client.str[3], Client.str[2], Client.str[4], Client.str[5], Client.str[6]);

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    //находим нужную комнату
                    if (Rival_list[i].id_room == Client.str[1])
                    {

                        //находим игрока для изменения статуса
                        if (Rival_list[i].id_player_connection == Client.str[3])
                        {
                            Rival_list[i] = updatePlayer;
                        }
                        data += String.Format("{0}~{1}|", Rival_list[i].userName, Rival_list[i].status);
                    }
                }

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    if (Rival_list[i].id_room == Client.str[1])
                    {

                        Server.UpdateStatusInGame(Rival_list[i].id_player_connection);
                    }
                }

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    if (Rival_list[i].id_room == Client.str[1])
                    {
                        max = Rival_list[i].max_players;
                        players_in_room++;
                        if (Rival_list[i].status == "ready")
                        {

                            players_ready++;
                        }
                    }
                }

                //перерботать отправку данных
                if (players_in_room == players_ready && players_in_room == Convert.ToInt32(max))
                {
                    //добавить занесение времени старта игры в бд
                    DataBase.DataBase.setDate();
                    for (int i = 0; i < Rival_list.Count; i++)
                    {
                        if (Rival_list[i].id_room == Client.str[1])
                        {
                            DataBase.DataBase.DeleteBet(Client.str[2]);
                        }
                    }

                    thread = new Thread(Cards_game);
                    thread.IsBackground = true;
                    thread.Start();

                }
            } catch (Exception exp)
            {
                ErrorLog.Log(DateTime.Now + " Error with update game: " + exp.Message);
                Console.WriteLine(exp.Message);
            }
        }

        public static void Cards_game()
        {
            try
            {
                if (Client.str[0] == "#take_card")
                {
                    data = Game.take_card();
                }
                else
                {
                    data = Game.Take_First_Cards();
                }

                
                for (int i = 0; i < Rival_list.Count; i++)
                {
                    Console.WriteLine(i);
                    if (Rival_list[i].id_room == Client.str[1])
                    {
                        Server.Game_(Rival_list[i].id_player_connection);
                    }

                }
            }catch(Exception exp)
            {
                ErrorLog.Log(DateTime.Now+" Error with Cards_game:  "+exp.Message);
            }
        }   

        public static void Stop()
        {

            try
            { 
            data = "";

            int players_in_room = 0, players_stop = 0;
            string max = "0";
            players_in_game updatePlayer = new players_in_game(Client.str[1], Client.str[3], Client.str[2], Client.str[4], Client.str[5],Client.str[6]);

            for (int i = 0; i <= Rival_list.Count-1; i++)
            {
                //находим нужную комнату
                if (Rival_list[i].id_room == Client.str[1])
                {

                    //находим игрока для изменения статуса
                    if (Rival_list[i].id_player_connection == Client.str[3])
                    {
                            Rival_list[i] = updatePlayer;
                    }
                    data += String.Format("{0}~{1}|", Rival_list[i].userName, Rival_list[i].status);
                }
            }
            
            for (int i = 0; i <= Rival_list.Count-1; i++)
            {
                if (Rival_list[i].id_room == Client.str[1])
                {
                    Server.UpdateStatusInGame(Rival_list[i].id_player_connection);
                }
            }
           
            for (int i = 0; i <= Rival_list.Count-1; i++)
            {
                if (Rival_list[i].id_room == Client.str[1])
                {
                    max = Rival_list[i].max_players;
                    players_in_room++;
                    if (Rival_list[i].status == "stop")
                    {
                        players_stop++;
                    }
                }
            }

                //начинаем подсчет очков и отправляем победителя
                if (players_in_room == players_stop && players_in_room == Convert.ToInt32(max))
                {
                    thread.Abort();

                    List<int> scoreList = new List<int>();

                    for (int i = 0; i < Rival_list.Count; i++)
                    {
                        if (Rival_list[i].id_room == Client.str[1])
                        {
                            scoreList.Add(Convert.ToInt32(Rival_list[i].score));
                        }
                    }

                    var items = scoreList.Where((a) => a <= 21);
                    if (items.Count() > 0)
                    {
                        int maxScore = items.Max();
                        for (int i = 0; i < Rival_list.Count; i++)
                        {
                            if (Rival_list[i].id_room == Client.str[1] && Rival_list[i].score == maxScore.ToString())
                            {
                                data =  Rival_list[i].userName;
                                DataBase.DataBase.SetBetResult(Rival_list[i].userName);
                            }
                        }
                    }
                    else
                    {
                        data = "У всех игроков Значение очков больше 21";
                    }

                    DataBase.DataBase.setDate();
                    DataBase.DataBase.SetWinner(data);

                    

                    for (int i = 0; i < Rival_list.Count; i++)
                    {
                        if (Rival_list[i].id_room == Client.str[1])
                        {
                            Server.Finish_game(Rival_list[i].id_player_connection);
                        }
                    }
                    
                }
            }catch(Exception exp)
            {
                ErrorLog.Log(DateTime.Now + " Error with stop game: " + exp.Message);
                Console.WriteLine(exp.Message);
            }
        }

        public static void Leaving_game()
        {
            try
            {
                int max = 0, players_in_room = 0, players_ready = 0;

                List<string> StatusList = new List<string>();

                for (int i=0;i<Rival_list.Count;i++)
                {
                    if(Rival_list[i].id_room == Client.str[1])
                    {
                        StatusList.Add(Rival_list[i].status);
                    }
                }


                bool DeletePlayer = false;

                for(int i=0;i<Rival_list.Count;i++)
                {
                    if(Rival_list[i].id_room == Client.str[1] && StatusList[i] == "ready")
                    {
                        DeletePlayer = true;
                    }
                }

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    if (Rival_list[i].id_room == Client.str[1] && Rival_list[i].id_player_connection == Client.str[3])
                    {
                        Rival_list.RemoveAt(i);
                        
                        break;
                    }
                }

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    //находим нужную комнату
                    if (Rival_list[i].id_room == Client.str[1])
                    {
                        data += String.Format("{0}~{1}|", Rival_list[i].userName, Rival_list[i].status);
                    }
                }

                Console.WriteLine(data);

                for (int i = 0; i <= Rival_list.Count - 1; i++)
                {
                    if (Rival_list[i].id_room == Client.str[1])
                    {
                        Server.UpdateStatusInGame(Rival_list[i].id_player_connection);
                    }
                }

                if (DeletePlayer)
                {
                   


                    for (int i = 0; i <= Rival_list.Count - 1; i++)
                    {
                        if (Rival_list[i].id_room == Client.str[1])
                        {
                            
                            
                            players_in_room++;
                            if (Rival_list[i].status == "ready")
                            {
                                players_ready++;
                            }
                        }
                    }

                    Console.WriteLine(players_in_room +"_"+players_ready);
                    if (players_in_room == players_ready)
                    {
                        UserDisconnectInGame.Stop_Game();
                    }

                   
                    for (int i = 0; i < Rival_list.Count; i++)
                    {
                        if (Rival_list[i].id_room == Client.str[1])
                        {
                            data = Rival_list[i].userName;
                            Server.Finish_game(Rival_list[i].id_player_connection);
                        }
                    }
                }
                


            }
            catch (Exception exp)
            {
                ErrorLog.Log(DateTime.Now + " Error with leaving from game: " + exp.Message);
                Console.WriteLine("Error with leaving " + exp.Message);
            }
            
        }
        public static void game()
        {
            data = "";
            for (int i = 0; i <= Rival_list.Count-1; i++)
            {
                if (Rival_list[i].id_room == Client.str[1])
                {
                    data += String.Format("{0}~{1}|", Rival_list[i].userName, Rival_list[i].status);
                }
            }

            
            for (int i = 0; i <= Rival_list.Count-1; i++)
            {
                if (Rival_list[i].id_room == Client.str[1])
                {
                    Server.UpdateUsersInGame(Rival_list[i].id_player_connection);
                }
            }        
        }

    }
}
