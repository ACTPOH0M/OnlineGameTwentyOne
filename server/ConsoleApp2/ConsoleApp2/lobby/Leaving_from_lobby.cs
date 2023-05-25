using System;
using System.Data.SqlClient;

namespace ConsoleApp2.lobby
{
    class Leaving_from_lobby
    {
        public static string str = "0";
        public static string player1 = null;
        public static string player2 = null;
        public static string player3 = null;
        public static string player4 = null;
        public static string winner = null;
        public static void leaving()
        {
           

            string sql = "SELECT * FROM room_table";
            int players_in_room = 0;
            try
            {
                using (SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {

                        while (reader.Read()) // построчно считываем данные
                        {
                            
                            if (Convert.ToInt32(Client.str[1]) == Convert.ToInt32(reader.GetValue(0)) && Convert.ToInt32(reader.GetValue(3)) <= Convert.ToInt32(reader.GetValue(4)))
                            {
                                
                                players_in_room = Convert.ToInt32(reader.GetValue(3));
                                player1 = reader.GetValue(5).ToString();
                                player2 = reader.GetValue(6).ToString();
                                player3 = reader.GetValue(7).ToString();
                                player4 = reader.GetValue(8).ToString();
                                winner = reader.GetValue(11).ToString();
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
            }catch(Exception exp)
            {
                Console.WriteLine("Error with reader " +exp.Message);
                ErrorLog.Log(DateTime.Now + " Error with reader in Leaving DB: " + exp.Message);
            }
            Console.WriteLine(str);
            if (str == "1")
            {
                
                
                try
                {
                   
                    string sqlExpression = "UPDATE room_table set players_in_room = @players_in_room, [player 1] = @player1, [player 2] = @player2, [player 3] = @player3, [player 4] = @player4 where id_room = @id_room";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.DataBase.conn))
                    {
                        if (String.IsNullOrEmpty(winner))
                        {
                            com.Parameters.AddWithValue("@id_room", Client.str[1]);
                            com.Parameters.AddWithValue("@players_in_room", players_in_room - 1);

                            if (player1 == Client.str[2])
                            {
                                com.Parameters.AddWithValue("@player1", "");
                                com.Parameters.AddWithValue("@player2", player2);
                                com.Parameters.AddWithValue("@player3", player3);
                                com.Parameters.AddWithValue("@player4", player4);

                            }
                            else if (player2 == Client.str[2])
                            {
                                com.Parameters.AddWithValue("@player1", player1);
                                com.Parameters.AddWithValue("@player2", "");
                                com.Parameters.AddWithValue("@player3", player3);
                                com.Parameters.AddWithValue("@player4", player4);

                            }
                            else if (player3 == Client.str[2])
                            {
                                com.Parameters.AddWithValue("@player1", player1);
                                com.Parameters.AddWithValue("@player2", player2);
                                com.Parameters.AddWithValue("@player3", "");
                                com.Parameters.AddWithValue("@player4", player4);

                            }
                            else if (player4 == Client.str[2])
                            {
                                com.Parameters.AddWithValue("@player1", player1);
                                com.Parameters.AddWithValue("@player2", player2);
                                com.Parameters.AddWithValue("@player3", player3);
                                com.Parameters.AddWithValue("@player4", "");

                            }

                            int number = com.ExecuteNonQuery();
                            Console.WriteLine("Обновлено объектов: {0}", number);
                            str = "1";
                        }
                        else
                        {
                            str = "1";
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with leaving from lobby " + e.Message);
                    ErrorLog.Log(DateTime.Now + " Error with leaving from lobby: " + e.Message);
                    str = "0";
                }
            }
        }
        public static string _leaving()
        {
            string data = "#leaving&" + str;
            Console.WriteLine(data);
            return data;
        }
    }
}
