using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.lobby
{
    class Connect_to_lobby
    {
        public static string str = "0";
        public static string max_players = "";
        public static string player1 = null;
        public static string player2 = null;
        public static string player3 = null;
        public static string player4 = null;
        public static void Connect()
        {
            
            string sql = "SELECT * FROM room_table";
            int players_in_room=0;
            using (SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    
                    while (reader.Read()) // построчно считываем данные
                    {
                        if (Convert.ToInt32(Client.str[1]) == Convert.ToInt32(reader.GetValue(0)) && Convert.ToInt32(reader.GetValue(3)) < Convert.ToInt32(reader.GetValue(4)))
                        {
                            max_players = reader.GetValue(4).ToString();
                            players_in_room = Convert.ToInt32(reader.GetValue(3));
                            
                            player1 = reader.GetValue(5)?.ToString();
                            player2 = reader.GetValue(6)?.ToString();
                            player3 = reader.GetValue(7)?.ToString();
                            player4 = reader.GetValue(8)?.ToString();

                            str = "1";
                            break;
                        }
                        else
                        {
                            str = "0";
                        }
                        
                    }
                }
                reader.Close();
            }

            if (str == "1")
            {
                str = "0";
                try
                {
                    
                    //проверить почему не работает прибавление
                    string sqlExpression = "UPDATE room_table set players_in_room = @players_in_room, [player 1] = @player1, [player 2] = @player2, [player 3] = @player3, [player 4] = @player4 where id_room = @id_room";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.DataBase.conn))
                    {
                        
                        com.Parameters.AddWithValue("@id_room", Client.str[1]);
                        com.Parameters.AddWithValue("@players_in_room", players_in_room + 1);

                        if (player1 == "")
                        {
                            
                            com.Parameters.AddWithValue("@player1", Client.str[2]);
                            com.Parameters.AddWithValue("@player2", player2);
                            com.Parameters.AddWithValue("@player3", player3);
                            com.Parameters.AddWithValue("@player4", player4);
                            
                        }
                        else if (player2 == "")
                        {
                            com.Parameters.AddWithValue("@player1", player1);
                            com.Parameters.AddWithValue("@player2", Client.str[2]);
                            com.Parameters.AddWithValue("@player3", player3);
                            com.Parameters.AddWithValue("@player4", player4);
                            
                        }
                        else if (player3 == "")
                        {
                            com.Parameters.AddWithValue("@player1", player1);
                            com.Parameters.AddWithValue("@player2", player2);
                            com.Parameters.AddWithValue("@player3", Client.str[2]);
                            com.Parameters.AddWithValue("@player4", player4);
                            
                        }
                        else if (player4 == "")
                        {
                            com.Parameters.AddWithValue("@player1", player1);
                            com.Parameters.AddWithValue("@player2", player2);
                            com.Parameters.AddWithValue("@player3", player3);
                            com.Parameters.AddWithValue("@player4", Client.str[2]);
                           
                        }

                        str = "1"; 
                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with connect to lobby " + e.Message);
                    str = "0";
                }
            }
        }

        public static string _connect()
        {
            
            string data = "#connect_to_lobby&"+str+"&"+max_players;
            Console.WriteLine(data);
            return data;
        }
    }
}
