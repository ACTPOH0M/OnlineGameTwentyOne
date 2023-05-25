
using System;
using System.Data.SqlClient;


namespace ConsoleApp2.Login_register
{
    class Login
    {
        //добавить обновление статуса в бд онлайн - офлайн
        public static string str="0";
        private static bool can_login = false;
        public static void log_user()
        {
            string sql = "SELECT * FROM users";
            SqlCommand command = new SqlCommand(sql, DataBase.DataBase.conn);
            SqlDataReader reader = command.ExecuteReader();
            can_login = false;
            if (reader.HasRows) // если есть данные
            {
                while (reader.Read()) // построчно считываем данные
                {
                    string nickname = reader.GetValue(1).ToString();
                    string login = reader.GetValue(2).ToString();
                    string password = reader.GetValue(3).ToString();
                    string status = reader.GetValue(6).ToString();
                    string id = reader.GetValue(0).ToString();

                    if (Client.str[1] == login && Client.str[2] == password)
                    {
                        if (status == "offline")
                        {
                            can_login = true;
                            str = "1" + "&" + nickname + "&" + Server.Clients.IndexOf(Server.user_in_list) + "&" + id;
                            break;
                        }
                        else
                        {
                            str = "#login&0&Пользователь уже онлайн";
                            can_login = false;
                        }
                    }
                    else
                    {
                        can_login = false;
                        str = "#login&0&логин или пароль неверный";
                    }
                }
                reader.Close();
            }


            if (can_login)
            {
                try
                {
                    string sqlExpression = "UPDATE users set status = @status  where login = @login";
                    using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.DataBase.conn))
                    {
                        com.Parameters.AddWithValue("@login", Client.str[1]);
                        com.Parameters.AddWithValue("@status", "online");

                        com.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with register " + e.Message);
                }
            }
            
        }
        public static string output()
        {
            string data = str;
            return data;
        }
    }
}
