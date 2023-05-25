using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.lobby
{
    class Create_lobby
    {
        public static string str = "0";
        public static int id = 0;
        public static void create_lobby()
        {
            
            string sql = "SELECT * FROM room_table";

            using (SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        id++;
                    }
                }
                reader.Close();
            }

            try
            {
                string sqlExpression = "INSERT INTO room_table (id_room,name_room,bet_room, players_in_room,[max players])" +
                    "VALUES (@id_room, @name_room, @bet_room, @players_in_room,@max_players)";
                using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.DataBase.conn))
                {
                    com.Parameters.AddWithValue("@id_room", id);
                    com.Parameters.AddWithValue("@name_room", Client.str[1]);
                    com.Parameters.AddWithValue("@bet_room", Client.str[2]);
                    com.Parameters.AddWithValue("@players_in_room", 0);
                    com.Parameters.AddWithValue("@max_players", Client.str[4]);

                    int number = com.ExecuteNonQuery();
                    Console.WriteLine("Добавлено объектов: {0}", number);
                    str = "1";
                }
            }catch(Exception e)
            {
                Console.WriteLine("Error with create lobby "+e.Message);
                ErrorLog.Log(DateTime.Now + " Error with create lobby: " + e.Message);
                str = "0";
            }

        }
        public static string info_about_create_lobby()
        {
            string data = "#create_game&"+str+"&"+ id;
            return data;
        }
    }
}
