using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.DataBase
{
    class SetDateGame
    {
        public static void connect()
        {

            string sql = "SELECT * FROM room_table";

            string DataStart = null, DataEnd = null;

            using (SqlCommand command = new SqlCommand(sql, DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        if (Client.str[1] == reader.GetValue(0).ToString())
                        {
                            DataStart = reader.GetValue(9).ToString();
                            DataEnd = reader.GetValue(10).ToString();
                        }
                    }
                }
                reader.Close();
            }

            try
                {
                    string sqlExpression = "UPDATE room_table set [date start] = @DateStart, [date end] = @DateEnd  where id_room = @id_room";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                    {
                        
                        if (Client.str[4] == "ready")
                        {
                         
                            com.Parameters.AddWithValue("@id_room", Client.str[1]);
                            com.Parameters.AddWithValue("@DateStart", DateTime.Now.ToString());
                            com.Parameters.AddWithValue("@DateEnd",DataEnd);
                        }
                        else if(Client.str[4] == "stop")
                        {
                            com.Parameters.AddWithValue("@id_room", Client.str[1]);
                            com.Parameters.AddWithValue("@DateEnd", DateTime.Now.ToString());
                            com.Parameters.AddWithValue("@DateStart", DataStart);
                        }

                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with SetDateGame " + e.Message);
                    ErrorLog.Log(DateTime.Now + " Error with SetDateGame: " + e.Message);
                }
            }
        }
}
