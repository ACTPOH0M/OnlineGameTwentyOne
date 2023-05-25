using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Login_register
{
    class Register
    {
        public static string str = "0";
        public static void reg_user()
        {
            bool can_reg = true;
           
            string sql = "SELECT * FROM users";
            string sqlExpression = "INSERT INTO users (id,nick_name,login, password, mail, money, status)" +
                "VALUES (@id, @nick_name, @login, @password, @mail, @money,@status)";

            int id = 0;
            using (SqlCommand command = new SqlCommand(sqlExpression, DataBase.DataBase.conn))
            {
                SqlCommand com = new SqlCommand(sql, DataBase.DataBase.conn);
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read()) // построчно считываем данные
                {

                    object login = reader.GetValue(2);
                    object password = reader.GetValue(3);
                    object mail = reader.GetValue(4);
                    object nick_name = reader.GetValue(1);

                    if (Client.str[1] == login.ToString())
                    {
                        if (Client.str[4] == nick_name.ToString())
                        {
                            if (Client.str[3] == mail.ToString())
                            {
                                can_reg = false;
                                str = "0&Маил уже занят";
                                break;
                            }
                            can_reg = false;
                            str = "0&Никнайм уже занят";
                            break;
                        }
                        can_reg = false;
                        str = "0&Логин уже занят";
                        break;
                    }
                    id++;
                }
                reader.Close();

                Console.WriteLine(str);

                if (can_reg)
                {
                    try
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@nick_name", Client.str[4]);
                        command.Parameters.AddWithValue("@login", Client.str[1]);
                        command.Parameters.AddWithValue("@password", Client.str[2]);
                        command.Parameters.AddWithValue("@mail", Client.str[3]);
                        command.Parameters.AddWithValue("@money", 1000);
                        command.Parameters.AddWithValue("@status", "online");
                        str = "1" + "&" + Client.str[4] + "&" + Server.Clients.IndexOf(Server.user_in_list)+"&"+id;
                    }
                    catch { 
                    
                    }
                }
                

                int number = command.ExecuteNonQuery();
                Console.WriteLine("Добавлено объектов: {0}", number);
                
            }
        }

        public static string output()
        {
            string data = str;
            Console.WriteLine(data);
            return data;
        }
    }
}
