using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Game
{
    class UserDisconnectInGame
    {
        public static string players = "0";
        public static void Stop_Game()
        {
            try
            {
                int x = 0;
                for (int i = 0; i < Rival.Rival_list.Count; i++)
                {
                    if (Rival.Rival_list[i].id_room == Client.str[1])
                    {
                        x++;
                        int y = Convert.ToInt32(Rival.Rival_list[i].max_players) - 1;
                        players = y.ToString();
                        Server.UpdateGame(Rival.Rival_list[i].id_player_connection);
                    }
                }
                
                if (x < 2)
                {
                    //присудить автомат победу
                    for (int i = 0; i < Rival.Rival_list.Count; i++)
                    {
                        if (Rival.Rival_list[i].id_room == Client.str[1])
                        {
                            Client.str[4] = "stop";
                            DataBase.DataBase.SetBetResult(Rival.Rival_list[i].userName);
                            DataBase.DataBase.setDate();
                            DataBase.DataBase.SetWinner(Rival.Rival_list[i].userName);
                            break;

                        }
                    }
                }
            }catch(Exception exp)
            {
                Console.WriteLine("Error with Stop_Game UserDisconnect "+exp.Message);
                ErrorLog.Log(DateTime.Now+ " Error with Stop_Game UserDisconnect " + exp.Message);
            }
        }

        public static void UpdateList()
        {
            Rival.players_in_game updatePlayer = new Rival.players_in_game(Client.str[1], Client.str[3], Client.str[2], Client.str[4], Client.str[5], Client.str[6]);
            for (int i = 0; i <= Rival.Rival_list.Count - 1; i++)
            {
                //находим нужную комнату
                if (Rival.Rival_list[i].id_room == Client.str[1])
                {
                    //находим игрока для изменения статуса
                    if (Rival.Rival_list[i].id_player_connection == Client.str[3])
                    {
                        Rival.Rival_list[i] = updatePlayer;
                    }
                }
            }
        }
    }
}
