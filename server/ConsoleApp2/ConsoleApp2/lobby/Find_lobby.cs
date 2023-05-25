using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.lobby
{
    class Find_lobby
    {
        public static List<string> info_about_lobby = new List<string>();
        public static object id_room, name_room, bet_room, players_in_room, max_players,winner=null;
        public static void find_all_lobbi()
        {
            string sql = "SELECT * FROM room_table";



            SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {

                while (reader.Read()) // построчно считываем данные
                {
                    id_room = reader.GetValue(0);
                    name_room = reader.GetValue(1);
                    bet_room = reader.GetValue(2);
                    players_in_room = reader.GetValue(3);
                    max_players = reader.GetValue(4);
                    winner = reader.GetValue(11);

                    if (Convert.ToInt32(players_in_room) < Convert.ToInt32(max_players))
                    {
                        info_about_lobby.Add(id_room + "/" + name_room + "/" + bet_room + "/" + players_in_room+"/"+ max_players);
                    }
                }
                reader.Close();
            }
        }


        public static void find_all_Result_lobbi()
        {

            string sql = "SELECT * FROM users";
            SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn);
            SqlDataReader reader = command.ExecuteReader();

            string name=null;
            string player1 = null;
            string player2 = null;
            string player3 = null;
            string player4 = null;

            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    if(Client.str[1] == reader.GetValue(1).ToString())
                    {
                        name = reader.GetValue(1).ToString();
                        break;
                    }
                }
                reader.Close();
            }

             sql = "SELECT * FROM room_table";
             command = new SqlCommand(sql, DataBase.DataBase.conn);
             reader = command.ExecuteReader();

            if (reader.HasRows) // если есть данные
            {

                while (reader.Read()) // построчно считываем данные
                {
                    id_room = reader.GetValue(0);
                    name_room = reader.GetValue(1);
                    bet_room = reader.GetValue(2);
                    winner = reader.GetValue(11);

                    player1 = reader.GetValue(5)?.ToString();
                    player2 = reader.GetValue(6)?.ToString();
                    player3 = reader.GetValue(7)?.ToString();
                    player4 = reader.GetValue(8)?.ToString();

                    if (name ==player1|| name == player2 || name == player3 || name == player4 )
                    {
                        info_about_lobby.Add(id_room + "/" + name_room + "/" + bet_room + "/" + winner);
                    }
                }
                reader.Close();
            }

        }

        public static string info_from_database()
        {
            string data=null;
            
            for (int i = 0; i<info_about_lobby.Count; i++)
                {
                    data += String.Format("{0}|",info_about_lobby[i]);
                }
            info_about_lobby.Clear();
            Console.WriteLine(data);
            return data;
        }
        
    }

}

