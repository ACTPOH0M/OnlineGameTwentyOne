using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game_cards.page
{
    /// <summary>
    /// Логика взаимодействия для game_window.xaml
    /// </summary>
    public partial class game_window : Page
    {
        public static game_window Instance { get; private set; }
        private delegate void set_players(string data);
        private delegate void leaving_player(string data);
        private delegate void card_game(string data);
        private delegate void finish_game(string data);

        public bool GameIsStart = false;
        set_players Set_players;
        leaving_player Leaving_player;
        card_game Card_Game;
        finish_game Finish_Game;
        int Player1 = 0, Player2 = 0, Player3 = 0, Player4 = 0;
        public static string score = null;

        string[] CardType = new string[] { "clubs","diamonds","hearts","spades"};

        private List<Cards> _Cards = new List<Cards>();

        public static List<Game> _game = new List<Game>();
        public static string str;
        public struct Cards
        {
            public string card_type;
            public int score;
            public Cards(string Card_type, int Score)
            {
                card_type = Card_type;
                score = Score;
            }
        }
        public struct Game
        {
            public string userName;
            public int score;
            public Game(string UserName, int Score)
            {

                userName = UserName;
                score = Score;
            }
        }

        public game_window()
        {
            InitializeComponent();
            Instance = this;

            user_settings.inGame = true;
            user_settings.inLobby = false;
            GameIsStart = false;

            Set_players = new set_players(Set_Players);
            Leaving_player = new leaving_player(Leaving_Player);
            Card_Game = new card_game(Card_game);
            Finish_Game = new finish_game(Finish_game);
            switch (user_settings.max_payers_in_room)
            {
                case ("2"):
                    {
                        player3.Visibility = Visibility.Hidden;
                        player3_status.Visibility = Visibility.Hidden;
                        player3_score.Visibility = Visibility.Hidden;

                        player4.Visibility = Visibility.Hidden;
                        player4_status.Visibility = Visibility.Hidden;
                        player4_score.Visibility = Visibility.Hidden;

                        break;
                    }
                case ("3"):
                    {
                        player4.Visibility = Visibility.Hidden;
                        player4_status.Visibility = Visibility.Hidden;
                        player4_score.Visibility = Visibility.Hidden;

                        break;
                    }
            }

            take_card.Visibility = Visibility.Hidden;
            ready_button.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Hidden;

            MainWindow.send("#game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room+"&0");
            
        }
        
        public static void game_(string data)
        {
            try
            {
                //#game&player1~9|player2~5
                string[] str = data.Split("&");
                string[] info = str[1].Split("|");

                for (int i = 0; i < info.Length-1; i++)
                {
                    string[] gg = info[i].Split("~");

                    Game game = new Game(gg[0], Convert.ToInt32(gg[1]));
                    _game.Add(game);
                }

                    if (game_window.Instance != null)
                    {
                          Instance.Calculation(data);
                          Instance.Card_game(data);
                    }
            }
            catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now + " Error with method game_: " + exp.Message);
            }
        }

        private void Card_game(string data)
        {  
            if (!Dispatcher.CheckAccess())
            {
            Dispatcher.Invoke(Card_Game, data);
            return;
            } 
            if(!GameIsStart)
            {
                ready_button.Visibility = Visibility.Hidden;
                take_card.Visibility = Visibility.Visible;
                Stop.Visibility = Visibility.Visible;
            }
            

            try
            {

                if (user_settings.user_name == player1.Content.ToString())
                {
                    player2_status.Visibility = Visibility.Hidden;
                    player3_status.Visibility = Visibility.Hidden;
                    player4_status.Visibility = Visibility.Hidden;

                    StackPlayer2.Visibility = Visibility.Hidden;
                    StackPlayer3.Visibility = Visibility.Hidden;
                    StackPlayer4.Visibility = Visibility.Hidden;

                    StackPlayer1_back.Visibility = Visibility.Hidden;

                    player2_score.Visibility = Visibility.Hidden;
                    player3_score.Visibility = Visibility.Hidden;
                    player4_score.Visibility = Visibility.Hidden;
                }
                else if (user_settings.user_name == player2.Content.ToString())
                {
                    player1_status.Visibility = Visibility.Hidden;
                    player3_status.Visibility = Visibility.Hidden;
                    player4_status.Visibility = Visibility.Hidden;

                    StackPlayer1.Visibility = Visibility.Hidden;
                    StackPlayer3.Visibility = Visibility.Hidden;
                    StackPlayer4.Visibility = Visibility.Hidden;

                    StackPlayer2_back.Visibility = Visibility.Hidden;

                    player1_score.Visibility = Visibility.Hidden;
                    player3_score.Visibility = Visibility.Hidden;
                    player4_score.Visibility = Visibility.Hidden;
                }
                else if (user_settings.user_name == player3.Content.ToString())
                {
                    player1_status.Visibility = Visibility.Hidden;
                    player2_status.Visibility = Visibility.Hidden;
                    player4_status.Visibility = Visibility.Hidden;

                    StackPlayer1.Visibility = Visibility.Hidden;
                    StackPlayer2.Visibility = Visibility.Hidden;
                    StackPlayer4.Visibility = Visibility.Hidden;

                    StackPlayer3_back.Visibility = Visibility.Hidden;

                    player1_score.Visibility = Visibility.Hidden;
                    player2_score.Visibility = Visibility.Hidden;
                    player4_score.Visibility = Visibility.Hidden;
                }
                else if (user_settings.user_name == player4.Content.ToString())
                {
                    player1_status.Visibility = Visibility.Hidden;
                    player2_status.Visibility = Visibility.Hidden;
                    player3_status.Visibility = Visibility.Hidden;

                    StackPlayer1.Visibility = Visibility.Hidden;
                    StackPlayer2.Visibility = Visibility.Hidden;
                    StackPlayer3.Visibility = Visibility.Hidden;

                    StackPlayer4_back.Visibility = Visibility.Hidden;

                    player1_score.Visibility = Visibility.Hidden;
                    player2_score.Visibility = Visibility.Hidden;
                    player3_score.Visibility = Visibility.Hidden;
                }

                GameIsStart = true;
                //#game&player1~9|player2~5
                Player1 = 0;
                Player2 = 0;
                Player3 = 0;
                Player4 = 0;

                StackPlayer1.Children.Clear();
                StackPlayer2.Children.Clear();
                StackPlayer3.Children.Clear();
                StackPlayer4.Children.Clear();

                StackPlayer1_back.Children.Clear();
                StackPlayer2_back.Children.Clear();
                StackPlayer3_back.Children.Clear();
                StackPlayer4_back.Children.Clear();
                //player1~9


                for (int k = 0; k < _game.Count; k++)
                    {

                        int score = 0;
                        score = _game[k].score;
                        string gg = _game[k].userName;

                    string type = _Cards[k].card_type;
                    string count = _Cards[k].score.ToString();

                    if (gg == player1.Content.ToString())
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/{type}/{count}.png"));
                        StackPlayer1.Children.Add(image);

                        image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/BackgroundCard.png"));
                        StackPlayer1_back.Children.Add(image);

                    }
                    else if (gg == player2.Content.ToString())
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/{type}/{count}.png"));
                        StackPlayer2.Children.Add(image);

                        image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/BackgroundCard.png"));
                        StackPlayer2_back.Children.Add(image);
                    }
                    else if (gg == player3.Content.ToString())
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/{type}/{count}.png"));
                        image.LayoutTransform = new RotateTransform(90);
                        StackPlayer3.Children.Add(image);

                        image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/BackgroundCard.png"));
                        image.LayoutTransform = new RotateTransform(90);
                        StackPlayer3_back.Children.Add(image);
                    }
                    else if (gg == player4.Content.ToString())
                    {
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/{type}/{count}.png"));
                        image.LayoutTransform = new RotateTransform(-90);
                        StackPlayer4.Children.Add(image);

                        image = new Image();
                        image.Source = new BitmapImage(new Uri($"pack://application:,,,/Resource/BackgroundCard.png"));
                        image.LayoutTransform = new RotateTransform(-   90);
                        StackPlayer4_back.Children.Add(image);
                    }


                    if (gg == player1.Content.ToString())
                            {
                                Player1 += score;
                                player1_score.Content = Player1.ToString();
                            }
                            else if (gg == player2.Content.ToString())
                            {
                                Player2 += score;
                                player2_score.Content = Player2.ToString();
                            }
                            else if (gg == player3.Content.ToString())
                            {
                                Player3 += score;
                                player3_score.Content = Player3.ToString();
                            }
                            else if (gg == player4.Content.ToString())
                            {
                                Player4 += score;
                                player4_score.Content = Player4.ToString();
                            }
                        
                    }
                
                
            }
            catch (Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now + " Error with set score: " + exp.Message);
            }
        }

        public static void Finish_game(string data)
        {
                try
                {
                    if (game_window.Instance != null)
                    {
                        Instance.Finish(data);
                    }
                }
                catch (Exception exp)
                {
                ErrorLog.ErrorLogWriter(DateTime.Now + " Error with initialization score: " +exp.Message);
                }   
        }

        private void Finish(string data)
        {
            //#finish_game&player~score
            string[] str = data.Split("&");
            string[] info = str[1].Split("~");
            
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(Finish_Game, data);
                return;
            }
            switch (user_settings.max_payers_in_room)
            {
                case ("2"):
                    {
                        player2.Visibility = Visibility.Visible;
                        player2_status.Visibility = Visibility.Visible;
                        StackPlayer2_back.Visibility = Visibility.Hidden;
                        StackPlayer2.Visibility = Visibility.Visible;

                        player1.Visibility = Visibility.Visible;
                        player1_status.Visibility = Visibility.Visible;
                        StackPlayer1_back.Visibility = Visibility.Hidden;
                        StackPlayer1.Visibility = Visibility.Visible;

                        break;
                    }
                case ("3"):
                    {
                        player2.Visibility = Visibility.Visible;
                        player2_status.Visibility = Visibility.Visible;
                        StackPlayer2_back.Visibility = Visibility.Hidden;
                        StackPlayer2.Visibility = Visibility.Visible;

                        player1.Visibility = Visibility.Visible;
                        player1_status.Visibility = Visibility.Visible;
                        StackPlayer1_back.Visibility = Visibility.Hidden;
                        StackPlayer1.Visibility = Visibility.Visible;

                        player3.Visibility = Visibility.Visible;
                        player3_status.Visibility = Visibility.Visible;
                        StackPlayer3_back.Visibility = Visibility.Hidden;
                        StackPlayer3.Visibility = Visibility.Visible;
                        break;
                    }
                case ("4"):
                    {
                        player2.Visibility = Visibility.Visible;
                        player2_status.Visibility = Visibility.Visible;
                        StackPlayer2_back.Visibility = Visibility.Hidden;
                        StackPlayer2.Visibility = Visibility.Visible;

                        player1.Visibility = Visibility.Visible;
                        player1_status.Visibility = Visibility.Visible;
                        StackPlayer1_back.Visibility = Visibility.Hidden;
                        StackPlayer1.Visibility = Visibility.Visible;

                        player3.Visibility = Visibility.Visible;
                        player3_status.Visibility = Visibility.Visible;
                        StackPlayer3_back.Visibility = Visibility.Hidden;
                        StackPlayer3.Visibility = Visibility.Visible;

                        player4.Visibility = Visibility.Visible;
                        player4_status.Visibility = Visibility.Visible;
                        StackPlayer4_back.Visibility = Visibility.Hidden;
                        StackPlayer4.Visibility = Visibility.Visible;
                        break;
                    }
            }
            Result.Content = "Победитель: " + info[0];

            
        }

        public static void users(string data)
        {
            //#game_room&{0}|{1}
            try
            {
                if (game_window.Instance != null)
                {
                    Instance.Set_Players(data);
                }
            }catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now + " Error with initialization players: " + exp.Message);
            }

        }

        public static void status(string data)
        { 
            //#game_room&{0}|{1}
            try
            {
                if (game_window.Instance != null)
                {
                    Instance.Set_Players(data);
                }
            }
            catch (Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now +  " Error with initialization status: " + exp.Message);
            }

        }

        public static void leaving(string data)
        {
            //#game_room&{0}|{1}
            try
            {
                if (game_window.Instance != null)
                {
                    Instance.Leaving_Player(data);
                }
            }
            catch (Exception exp)
            {
                ErrorLog.ErrorLogWriter("Error with initialization exit from lobby: " + exp.Message);
            }

        }

        private void Leaving_Player(string data)
        {
            try
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(Leaving_player, data);
                    return;
                }
                string[] str = data.Split("&");

                if (str[1] == "1")
                {
                    if (Instance != null)
                    {
                        NavigationService.Navigate(new Page1());
                    }
                }
            } catch (Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now+" Error with exit from lobby: " + exp.Message);
            }
            
        }

        private void Set_Players(string data)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(Set_players, data);
                return;
            }
            //daw&player~status|player~status|
            try
            { 
            string[] str = data.Split("&");
            string[] users = str[1].Split("|");
                int k = 0;
                for (int i = 0; i < Convert.ToInt32(user_settings.max_payers_in_room); i++)
                {
                    if (!String.IsNullOrEmpty(users[i]))
                    {
                        string[] set = users[i].Split("~");

                        switch (set[1])
                        {
                            case ("ready"):
                                {
                                    if (i == 0)
                                    {
                                         player1.Content = set[0];
                                        player1_status.Content = "ГОТОВ";
                                    }
                                    if (i == 1)
                                    {
                                        player2.Content = set[0];
                                        player2_status.Content = "ГОТОВ";
                                    }
                                    if (i == 2)
                                    {
                                        player3.Content = set[0];
                                        player3_status.Content = "ГОТОВ";
                                    }
                                    if (i == 3)
                                    {
                                        player4.Content = set[0];
                                        player4_status.Content = "ГОТОВ";
                                    }
                                    break;
                                }
                            case ("not_ready"):
                                {
                                    if (i == 0)
                                    {
                                        player1.Content = set[0];
                                        player1_status.Content = "НЕ ГОТОВ";
                                    }
                                    if (i == 1)
                                    {
                                        player2.Content = set[0];
                                        player2_status.Content = "НЕ ГОТОВ";
                                    }
                                    if (i == 2)
                                    {
                                        player3.Content = set[0];
                                        player3_status.Content = "НЕ ГОТОВ";
                                    }
                                    if (i == 3)
                                    {
                                        player4.Content = set[0];
                                        player4_status.Content = "НЕ ГОТОВ";
                                    }
                                    break;
                                }
                            case ("stop"):
                                {
                                    if (i == 0)
                                    {
                                        player1.Content = set[0];
                                        player1_status.Content = "ЗАКОНЧИЛ";
                                    }
                                    if (i == 1)
                                    {
                                        player2.Content = set[0];
                                        player2_status.Content = "ЗАКОНЧИЛ";
                                    }
                                    if (i == 2)
                                    {
                                        player3.Content = set[0];
                                        player3_status.Content = "ЗАКОНЧИЛ";
                                    }
                                    if (i == 3)
                                    {
                                        player4.Content = set[0];
                                        player4_status.Content = "ЗАКОНЧИЛ";
                                    }
                                    break;
                                }
                        }
                        k++;
                    }
                    else
                    {
                        if (i == 0)
                        {
                            player1.Content = "Игрок 1";
                            player1_status.Content = "НЕ ГОТОВ";
                        }
                        if(i==1)
                        {
                            player2.Content = "Игрок 2";
                            player2_status.Content = "НЕ ГОТОВ";
                        }
                        if(i==2)
                        {
                            player3.Content = "Игрок 3";
                            player3_status.Content = "НЕ ГОТОВ";
                        }
                        if(i==3)
                        {
                            player4.Content = "Игрок 4";
                            player4_status.Content = "НЕ ГОТОВ";
                        }

                    }
                    
                }
                if(k==Convert.ToInt32(user_settings.max_payers_in_room) && !GameIsStart)
                {
                    ready_button.Visibility = Visibility.Visible;
                }
        }catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now+" Error with set players: " + exp.Message);
            }
    }
        
        private void ready_button_Click(object sender, RoutedEventArgs e)
        {
            // если все пользователи подключены то кнопка появится 
            MainWindow.send("#update_status_game&" + user_settings.id_room+"&"+user_settings.user_name + "&" + user_settings.id_connection+"&ready&"+user_settings.max_payers_in_room+"&0");
            
        }

        private void Exite_button_Click(object sender, RoutedEventArgs e)
        {

            //выход
            Score();
            if (player1_status.Content.ToString() == "ГОТОВ" && user_settings.user_name == player1.Content.ToString())
            {
                Exit();
            }else if(player2_status.Content.ToString() == "ГОТОВ" && user_settings.user_name == player2.Content.ToString())
            {
                Exit();
            }
            else if (player3_status.Content.ToString() == "ГОТОВ" && user_settings.user_name == player3.Content.ToString())
            {
                Exit();
            }
            else if (player4_status.Content.ToString() == "ГОТОВ" && user_settings.user_name == player4.Content.ToString())
            {
                Exit();
            }   
            else
            {
                MainWindow.send("#leaving_game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room+ "&" + score + "&" + user_settings.id_user_in_db);
            }
        }

        private void Exit()
        {
            if (MessageBox.Show("Вы действительно хотите покинуть игру", "Выход", MessageBoxButton.YesNo,  MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MainWindow.send("#leaving_game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room + score + "&" + user_settings.id_user_in_db);
            }
        }

        private void take_card_Click(object sender, RoutedEventArgs e)
        {
            // взять еще карту
            // переделать через switch
            Score();

            MainWindow.send("#take_card&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&ready&" + user_settings.max_payers_in_room+"&"+score);
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Score();
            take_card.Visibility = Visibility.Hidden;
            Stop.Visibility = Visibility.Hidden;
            ready_button.Visibility = Visibility.Hidden;
            MainWindow.send("#stop_game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&stop&" + user_settings.max_payers_in_room + "&" + score +"&"+user_settings.id_user_in_db);
        }
        private  void Score()
        {
            
            if (user_settings.user_name == player1.Content.ToString())
            {
                score = player1_score.Content.ToString();
            }
            else if (user_settings.user_name == player2.Content.ToString())
            {
                score = player2_score.Content.ToString();
            }
            else if (user_settings.user_name == player3.Content.ToString())
            {
                score = player3_score.Content.ToString();
            }
            else if (user_settings.user_name == player4.Content.ToString())
            {
                score = player4_score.Content.ToString();
            }
        }

        private void Calculation(string data)
        {
            try
            {
                //#game&player1~9|player2~5
                string[] str = data.Split("&");
                //player2~5
                string[] info = str[1].Split("|");
                bool can = false;
                for (int i = 0; i < info.Length - 1; i++)
                {
                    string[] gg = info[i].Split("~");

                    //проверка на есть такие комбинации или нет
                    Random rd = new Random();
                    int index = rd.Next(0, CardType.Length - 1);
                    string type = CardType[index].ToString();
                    
                    if (_Cards.Count != 0)
                    {

                        for (int k = 0; k < _Cards.Count; k++)
                        {

                            if (type == _Cards[k].card_type && Convert.ToInt32(gg[1]) == _Cards[k].score)
                            {
                                index = rd.Next(0, CardType.Length - 1);
                                type = CardType[index].ToString();
                                can = false;
                                break;
                            }
                            else
                            {
                                can = true;
                            }
                        }
                    }
                    else
                    {
                        can = true;
                    }

                    if (can)
                    {
                        Cards cards = new Cards(type, Convert.ToInt32(gg[1]));
                        _Cards.Add(cards);
                    }
                    else
                    {
                        Calculation(data);
                    }
                }
            }catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now + " Error with calculate cards: " +exp.Message);
            }
        }
    }
}
