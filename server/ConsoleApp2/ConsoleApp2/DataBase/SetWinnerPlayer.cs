

using System;
using System.Data.SqlClient;

namespace ConsoleApp2.DataBase
{
    class SetWinnerPlayer
    {
        public static void connect(string userNickname)
        { 
            try
            {
                

                string sqlExpression = "UPDATE room_table set winner = @winner  where id_room = @id_room";
                using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                { 
                    com.Parameters.AddWithValue("@id_room", Client.str[1]);
                    com.Parameters.AddWithValue("@winner", userNickname);
                    
                    com.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with setWinner " + e.Message);
                ErrorLog.Log(DateTime.Now + " Error with setWinner: " + e.Message);
            }
        }
    }
}
