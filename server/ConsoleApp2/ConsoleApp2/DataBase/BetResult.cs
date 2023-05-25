using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.DataBase
{
    class Bet
    {
        public static void Result(string userNickname)
        {

            string sql = "SELECT * FROM room_table";

            int bet=0, players=0;

            using (SqlCommand command = new SqlCommand(sql, DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        if (Client.str[1] == reader.GetValue(0).ToString())
                        {
                            bet = Convert.ToInt32(reader.GetValue(2));
                            players = Convert.ToInt32(reader.GetValue(4));
                        }
                    }
                    
                }
                reader.Close();
            }
            
            sql = "SELECT * FROM users";

            int money = 0;

            using (SqlCommand command = new SqlCommand(sql, DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        if (userNickname == reader.GetValue(1).ToString())
                        {
                            money = Convert.ToInt32(reader.GetValue(5));
                        }
                    }

                }
                reader.Close();
            }

            try
            {
                string sqlExpression = "UPDATE users set money = @money  where nick_name = @nick_name";
                using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                {
                    int result = bet * players;
                    com.Parameters.AddWithValue("@nick_name", userNickname);
                    com.Parameters.AddWithValue("@money", money + result);


                    com.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with SetBetResult " + e.Message);
                ErrorLog.Log(DateTime.Now + " Error with SetBetResult: " + e.Message);
            }
        }
        public static void Start(string userNickname)
        {

            string sql = "SELECT * FROM room_table";

            int bet = 0;

            using (SqlCommand command = new SqlCommand(sql, DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        if (Client.str[1] == reader.GetValue(0).ToString())
                        {
                            bet = Convert.ToInt32(reader.GetValue(2));
                        }
                    }

                }
                reader.Close();
            }

            sql = "SELECT * FROM users";

            int money = 0;

            using (SqlCommand command = new SqlCommand(sql, DataBase.conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {

                    while (reader.Read()) // построчно считываем данные
                    {
                        if (userNickname == reader.GetValue(1).ToString())
                        {
                            money = Convert.ToInt32(reader.GetValue(5));
                        }
                    }

                }
                reader.Close();
            }

            try
            {
                string sqlExpression = "UPDATE users set money = @money  where nick_name = @nick_name";
                using (SqlCommand com = new SqlCommand(sqlExpression, DataBase.conn))
                {
                 
                    com.Parameters.AddWithValue("@nick_name",userNickname);
                    com.Parameters.AddWithValue("@money", money - bet);


                    com.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error with SetBetStart " + e.Message);
                ErrorLog.Log(DateTime.Now + " Error with SetBetStart: " + e.Message);
            }
        }
    }
}

