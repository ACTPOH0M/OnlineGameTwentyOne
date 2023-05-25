using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Windows.Threading;
using System.Diagnostics;

namespace game_cards
{
    public class MyItem
    { 
        public string Id_room { get; set; }
        public string Name_room { get; set; }
        public string Players_in_room { get; set; }
        public string Bet_room { get; set; }
        public string max_players { get; set; }
    }

    public partial class Page1 : Page
    {
        private delegate void lobby(string data);
        private delegate void connect_to_lobby(string data);
        
        public static Page1 Instance { get; private set; }
        MyItem myItem;

        lobby UpdateLobby;
        connect_to_lobby Connect;
        
        private Thread _clientThread;
        bool selectListView = false;


        public static List<string> lobby_from_server = new List<string>();

        public Page1()
        {
            InitializeComponent();
            Instance = this;
            selectListView = false;
            UpdateLobby = new lobby(insert);
            Connect = new connect_to_lobby(connection_to_lobby);

            user_settings.inGame = false;

            MainWindow.send("#find_lobby");

            _clientThread = new Thread(listener);
            _clientThread.IsBackground = true;
            _clientThread.Start();
            user_settings.inLobby = true;
        }



        private void listener()
        {
            while (connection.connect._serverSocket.Connected)
            {
                try
                {
                    byte[] buffer = new byte[8126];
                    int bytesRec = connection.connect._serverSocket.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);

                    if (data.Contains("#updatechat&"))
                    {
                        MainWindow.UpdateChate(data);
                        continue;
                    }
                    if (data.Contains("#find_lobby&"))
                    {
                        updateLobby(data);
                        MainWindow.send("#balance&" + user_settings.id_user_in_db);
                        continue;
                    }
                    if (data.Contains("#updateid&"))
                    {
                        updateIdUser(data);
                        continue;
                    }
                    if (data.Contains("#connect_to_lobby&"))
                    {
                        connection_to_lobby(data);
                        continue;
                    }
                    if (data.Contains("#game_room&"))
                    {
                        page.game_window.users(data);
                        continue;
                    }
                    if (data.Contains("#game_status_room&"))
                    {
                        page.game_window.status(data);
                        continue;
                    }
                    if (data.Contains("#leaving&"))
                    {
                        page.game_window.leaving(data);
                        continue;
                    }
                    if (data.Contains("#game&"))
                    {

                        page.game_window.game_(data);
                        continue;
                    }
                    if (data.Contains("#finish_game&"))
                    {
                        page.game_window.Finish_game(data);
                        continue;
                    }
                    if (data.Contains("#closeapp&"))
                    {
                        MainWindow.Close1(data);
                        continue;
                    }
                    if (data.Contains("#update_game&"))
                    {
                        string[] str = data.Split("&");
                        user_settings.max_payers_in_room = str[1];
                        MainWindow.send("#update_game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&ready&" + user_settings.max_payers_in_room + "&0");
                        continue;
                    }
                    if (data.Contains("#create_game&"))
                    {
                        string[] str = data.Split("&");

                        if (str[1] == "1")
                        {
                            MainWindow.send("#connect_to_lobby&" + str[2] + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room + "&0");
                        }
                        else
                        {
                            MessageBox.Show("Ошибка создания лобби");
                        }
                        continue;
                    }
                    if (data.Contains("#balance&"))
                    {
                        user_settings.balance = data.Split("&")[2];
                        MainWindow.BalanceFromServer(data);
                        continue;
                    }
                    if (data.Contains("#result_of_games&"))
                    {
                        Console.WriteLine(data);
                        Menu.Results.Result.Result_of_games(data);
                        continue;
                    }

                }
                catch (Exception e)
                {
                    ErrorLog.ErrorLogWriter(DateTime.Now + " Error with listern: " + e.Message);
                    //поставить проверку на поток
                    try
                    {
                        Dispatcher.Invoke(() =>
                        {
                            var process = new Process();
                            process.StartInfo.FileName = "game_cards.exe";
                            process.EnableRaisingEvents = true;
                            process.StartInfo.UseShellExecute = true;
                            process.StartInfo.CreateNoWindow = false;
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            process.Start();

                            Application.Current.Shutdown();
                        });
                    } catch (Exception exp)
                    {
                        ErrorLog.ErrorLogWriter(DateTime.Now + " Error with Invoke: " + exp.Message);
                    }


                }
            }
        }

        //проверить
        private void connection_to_lobby(string data)
        {
            try
            {
                string[] str = data.Split("&");
                if (str[1] == "1")
                {
                    if (!Dispatcher.CheckAccess())
                    {
                        Dispatcher.Invoke(Connect, data);
                        return;
                    }
                    user_settings.max_payers_in_room = str[2];
                    // при заходе в новую игру умирает
                    NavigationService.Navigate(new page.game_window());
                    
                }
                else
                {
                    MessageBox.Show("Неудалось подключится к лобби, возсожно оно переполнено");
                }
            } catch (Exception exp)
            { ErrorLog.ErrorLogWriter(DateTime.Now + " Error with method connect_to_lobby: " + exp.Message); }
        }

        private void updateIdUser(string data)
        {
            string[] str = data.Split("&");
            user_settings.id_connection = str[1];

        }

        
        private void updateLobby(string data)
        {

            string[] message = data.Split('&')[1].Split("|");
            int countMessage = message.Length;
            if (countMessage <= 0)
            {
                return;
            }

            for (int i = 0; i <=countMessage; i++)
            {
                try
                {
                    if (string.IsNullOrEmpty(message[i]))
                    {
                        continue;
                    }
                    insert(message[i]);
                }
                catch (Exception exp)
                {
                    ErrorLog.ErrorLogWriter(DateTime.Now + " Error with updateLobby: " + exp.Message);
                    continue;
                }
            }


        }
        private void insert(string data)
        {
               
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(UpdateLobby, data);
                return;
            }
            string[] str = data.Split("/");
            string palayers = str[3] + "/" + str[4];
            List_of_lobby.Items.Add(new MyItem {Id_room = str[0], Name_room = str[1], Players_in_room = palayers, Bet_room = str[2], max_players = str[4]});    
        }
        

        private void List_of_lobby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if(List_of_lobby.SelectedValue != null)
            {
                myItem = (MyItem)List_of_lobby.SelectedItem;
                selectListView = true;
            }
            else
            {
                selectListView = false;
            }
        }
        
        private void Connect_to_lobby_Click(object sender, RoutedEventArgs e)
        {
            
            if(selectListView)
            {
                user_settings.id_room = myItem.Id_room;
                MainWindow.send("#connect_to_lobby&" + myItem.Id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room + "&0");
            }else
            {
                MessageBox.Show("Выберите комнаты для игры");
            }
           
        }

        private void update_lobby_Click(object sender, RoutedEventArgs e)
        {
            List_of_lobby.Items.Clear();
            MainWindow.send("#find_lobby");
        }

        private void List_of_lobby_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            myItem = (MyItem)List_of_lobby.SelectedItem;
            user_settings.id_room = myItem.Id_room;
            MainWindow.send("#connect_to_lobby&" + myItem.Id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room + "&0");
        }
    }
}
