 using System;
using System.Data.SqlClient;

namespace ConsoleApp2.DataBase
{
    public static class DataBase
    {
        public static SqlConnection conn;

        public static void Connect()
        {
            Console.WriteLine("Getting Connection ...");

            var datasource = @"ACTPOHOM\SQLEXPRESS03";//your server
            var database = "Game"; //your database name
            var username = "GameDev"; //username of server to connect
            var password = "123"; //password

            //your connection string 
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
                      + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;
            //string connString = @"Data Source=ACTPOHOM\SQLEXPRESS03;Initial Catalog=Game; Trusted_Connection=True;";
            //create instanace of database connection
            using (conn = new SqlConnection(connString))

            {
                try
                {
                    Console.WriteLine("Openning Connection ...");

                    //open connection
                    conn.Open();

                    Console.WriteLine("Connection successful!");

                    switch (Client.str[0])
                    {
                        case ("#login"):
                            {
                                Login_register.Login.log_user();
                                break;
                            }
                        case ("#register"):
                            {
                                Login_register.Register.reg_user();
                                break;
                            }
                        case ("#find_lobby"):
                            {
                                lobby.Find_lobby.find_all_lobbi();
                                break;
                            }
                        case ("#create_lobby"):
                            {
                                lobby.Create_lobby.create_lobby();
                                break;
                            }
                        case ("#connect_to_lobby"):
                            {
                                lobby.Connect_to_lobby.Connect();
                                break;
                            }
                        case ("#leaving_game"):
                            {
                                lobby.Leaving_from_lobby.leaving();
                                return;
                            }
                        case ("#update_balance"):
                            {
                                Balance.WorkWithBalance();
                                return;
                            }
                        case ("#balance"):
                            {
                                Balance.InfoAboutBalance();
                                return;
                            }
                        case ("#result_of_games"):
                            {
                                lobby.Find_lobby.find_all_Result_lobbi();
                                return;
                            }


                    }
                }
                catch (Exception e)
                {
                    ErrorLog.Log(DateTime.Now + " Error with connect to database: " + e.Message);
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
        public static void setDate()
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

                    SetDateGame.connect();
                    
                }
                catch (Exception e)
                {
                    ErrorLog.Log(DateTime.Now + " Error with connect to database: " + e.Message);
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
        public static void SetWinner(string userNickname)
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

                    SetWinnerPlayer.connect(userNickname);

                }
                catch (Exception e)
                {
                    ErrorLog.Log(DateTime.Now + " Error with connect to database: " + e.Message);
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
        public static void SetBetResult(string data)
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

                    Bet.Result(data);

                }
                catch (Exception e)
                {
                    ErrorLog.Log(DateTime.Now + " Error with connect to database: " + e.Message);
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
        public static void DeleteBet(string userNickname)
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

                    Bet.Start(userNickname);

                }
                catch (Exception e)
                {
                    ErrorLog.Log(DateTime.Now + " Error with connect to database: " + e.Message);
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }

        public static void ChangeStatusUser()
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

                    ChangeStatus.ChangeStatusUser();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
        public static void ChangeStatusUserDisconnet(string id)
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

                    ChangeStatus.ChangeStatusUserDisconnect(id);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error with connect to database: " + e.Message);
                }
            }
        }
    }
}
