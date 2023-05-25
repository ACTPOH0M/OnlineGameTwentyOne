using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.DataBase
{
    class Balance
    {
        private static string str = null;
        public static void WorkWithBalance()
        {
                try
                {
                str = "0";
                    string sqlExpression = "Insert into Balance_Update (id_user,[Card number], Money, Date) Values(@id_user,@card,@money,@date)";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                    {
                        com.Parameters.AddWithValue("@id_user", Client.str[1]);
                        com.Parameters.AddWithValue("@card", Client.str[2]);
                        com.Parameters.AddWithValue("@money", Client.str[3]);
                        com.Parameters.AddWithValue("@date", DateTime.Now);

                        com.ExecuteNonQuery();
                        str = "1";
                    }

                    if (str == "1")
                    {

                    sqlExpression = "SELECT * FROM users";

                    int money = 0;

                    using (SqlCommand command = new SqlCommand(sqlExpression, DataBase.conn))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows) // если есть данные
                        {

                            while (reader.Read()) // построчно считываем данные
                            {
                                if (Client.str[1] == reader.GetValue(0).ToString())
                                {
                                    money = Convert.ToInt32(reader.GetValue(5));
                                }
                            }

                        }
                        reader.Close();
                    }

                    sqlExpression = "UPDATE users set money = @money where id = @id_user";
                         using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                         {
                             com.Parameters.AddWithValue("@id_user", Client.str[1]);
                             com.Parameters.AddWithValue("@money", money+Convert.ToInt32(Client.str[3]));
                             com.ExecuteNonQuery();
                             str = "1";
                         }

                    }else
                     {
                       str = "0";
                     }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with Balance " + e.Message);
                    ErrorLog.Log(DateTime.Now + " Error with Balance: " + e.Message);
                }
            
        }

        public static void InfoAboutBalance()
        {
            try
            {
                string sql = "SELECT * FROM users";
                SqlCommand command = new SqlCommand(sql, DataBase.conn);
                SqlDataReader reader = command.ExecuteReader();

                str = "0";
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read()) // построчно считываем данные
                    {
                        
                        string id = reader.GetValue(0).ToString();
                        string money = reader.GetValue(5).ToString();
                        if(Client.str[1] == id)
                        {
                            str = "1&" + money;
                            break;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with InfoAboutBalance " + e.Message);
                ErrorLog.Log(DateTime.Now + " Error with InfoAboutBalance: " + e.Message);
            }

        }

        public static string info()
        {
            string data = str;
            Console.WriteLine(data);
            return data;
        }
    }
}
