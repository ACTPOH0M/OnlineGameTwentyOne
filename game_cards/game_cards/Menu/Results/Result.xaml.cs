using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game_cards.Menu.Results
{
    public class MyItem
    {
        public string Id_room { get; set; }
        public string Name_room { get; set; }
        public string Bet_room { get; set; }
        public string Winner { get; set; }
    }
    public partial class Result : Page
    {
        public static Result Instance { get; private set; }

        private delegate void lobby(string data);
        lobby UpdateLobby;

        public static List<string> lobby_from_server = new List<string>();

        public Result()
        {
            InitializeComponent();
            Instance = this;
            UpdateLobby = new lobby(insert);
            user_settings.inLobby = false;
            MainWindow.send("#result_of_games&" + user_settings.user_name);
        }

        public static void Result_of_games(string data)
        {
            if (Instance != null)
            {
                Instance.updateLobby(data);
            }
        }

        private void updateLobby(string data)
        {

            string[] message = data.Split('&')[1].Split('|');
            int countMessage = message.Length;
            if (countMessage <= 0)
            {
                return;
            }

            for (int i = 0; i < countMessage; i++)
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
                    ErrorLog.ErrorLogWriter(DateTime.Now + " Error with updateResultLobby: " + exp.Message);
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
            List_of_Result.Items.Add(new MyItem { Id_room = str[0], Name_room = str[1],  Bet_room = str[2], Winner = str[3] });

        }

    }
}
