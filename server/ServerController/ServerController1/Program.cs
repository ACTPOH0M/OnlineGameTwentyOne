using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;

namespace ServerController1
{
    class Program
    {
        public static SqlConnection conn;
        static void Main(string[] args)
        {
            StartProcess();
        }

        private static void StartProcess()
        {

            var process = new Process();
            process.StartInfo.FileName = "D:\\учеба\\ООП курсач\\server\\ConsoleApp2\\ConsoleApp2\\bin\\Debug\\ConsoleApp2.exe";
            process.EnableRaisingEvents = true;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Exited += Process_Exit;
            process.Start();
            Log("Сервер был запущен " + process.StartTime);

            Console.ReadLine();
        }

        private static void Process_Exit(object sender, EventArgs e)
        {
            var process = sender as Process;
            Log(DateTime.Now + " Сервер прекратил работу, id процесса " + process.Id);
            Connect();
            StartProcess();
        }

        private static void Log(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("ServerControllerLog.txt", true))
                {
                    writer.WriteLine(data);
                    writer.Close();
                    Console.WriteLine("Запись была совершена");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка записи в ServerControllerLog : " + e.Message);
            }
        }


        public static void Connect()
        {
            Console.WriteLine("Getting Connection ...");

            var datasource = @"ACTPOHOM\SQL";//your server
            var database = "Game"; //your database name
            var username = "GameDev"; //username of server to connect
            var password = "4287"; //password

            //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                        + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            //create instanace of database connection
            using (conn = new SqlConnection(connString))
            {
                try
                {
                    Console.WriteLine("Openning Connection ...");

                    //open connection
                    conn.Open();

                    Console.WriteLine("Connection successful!");

                    string sql = "SELECT * FROM users";
                    SqlCommand command = new SqlCommand(sql, conn);
                    SqlDataReader reader = command.ExecuteReader();

                    List<int> ListId = new List<int>();

                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read()) // построчно считываем данные
                        {
                            ListId.Add(Convert.ToInt32(reader.GetValue(0)));
                        }
                        reader.Close();
                    }


                    for (int k = 0; k < ListId.Count; k++)
                    {
                        try
                        {
                            string sqlExpression = "UPDATE users set status = @status  where id = @id";
                            using (SqlCommand com = new SqlCommand(sqlExpression, conn))
                            {

                                com.Parameters.AddWithValue("@id", ListId[k]);
                                com.Parameters.AddWithValue("@status", "offline");

                                com.ExecuteNonQuery();

                            }
                        }
                        catch (Exception e)
                        {
                            Log(DateTime.Now + " Error with update status " + e.Message);
                        }
                    }



                }
                catch (Exception e)
                {
                    Log(DateTime.Now+ " Error with connect to database: " + e.Message);
                }
            }
        }
    }
}
