using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.DataBase
{
    class ChangeStatus
    {
        public static string str;
        public static bool ForceDisconect;
        public static void ChangeStatusUser()
        {
            try
            {
                    string sqlExpression = "UPDATE users set status = @status  where nick_name = @nick_name";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                    {
                        com.Parameters.AddWithValue("@nick_name", Client.str[1]);
                        com.Parameters.AddWithValue("@status", "offline");

                        com.ExecuteNonQuery();
                        str = "1";
                    }
                
            }
            catch (Exception e)
            {
                ErrorLog.Log(DateTime.Now + " Error with close app: " + e.Message);
                Console.WriteLine("Error with close app" + e.Message);
                str = "0";
            }
           
        }

        public static void ChangeStatusUserDisconnect(string id)
        {
            try
            {
                string sqlExpression = "UPDATE users set status = @status  where id_connection = @id_connection";
                using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                {
                    com.Parameters.AddWithValue("@id_connection",id);
                    com.Parameters.AddWithValue("@status", "offline");

                    com.ExecuteNonQuery();
                    str = "1";
                }
            }
            catch (Exception e)
            {
                ErrorLog.Log(DateTime.Now + " Error with close app ChangeStatusUserDisconnect : " + e.Message);
                Console.WriteLine("Error with close app ChangeStatusUserDisconnect " + e.Message);
                str = "0";
            }

        }
        public static string Data()
        {
            string data = "#closeapp&" + str;
            Console.WriteLine(data);
            return data;
        }
    }
}
