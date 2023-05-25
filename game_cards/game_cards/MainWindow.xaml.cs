using System;
using System.Windows;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Media;

namespace game_cards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public static MainWindow Instance { get; private set; }
        private delegate void printer(string data);
        private delegate void cleaner();
        private delegate void close();
        private delegate void balanceinfo(string data);


        printer Printer;
        cleaner Cleaner;
        close CloseApp;
        balanceinfo Balanceinfo;
        


        public MainWindow()
        {
            InitializeComponent();

            Printer = new printer(print);
            Cleaner = new cleaner(clearChat);
            CloseApp = new close(Close_app);
            Balanceinfo = new balanceinfo(BalanceInfo);

            Instance = this;
            MainFrame.Content = new Page1();

            send("#setname&" + user_settings.user_name);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public static void Close1(string data)
        {

            try
            {
                if (Instance != null)
                {
                    Instance.Closeapp(data);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Ошибка закрытия приложения" + exp.Message);
            }
        }
        private void Closeapp(string data)
        {
            try
            {
                string[] str = data.Split('&');
                if (str[1] == "1")
                {
                    Close_app();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

        }
        private void Close_app()
        {
            try
            {
                if (!Dispatcher.CheckAccess())//я валера турурурруру  жарихин лохъ
                {

                    Dispatcher.Invoke(Printer);//я валерра туррурруруру 
                    return;
                }
                Application.Current.Shutdown();// я валера турруруруру 
            }
            catch (Exception exp)
            {
                MessageBox.Show("a " + exp.Message);// я валерра турруруруру
            }
        }
        public static void UpdateChate(string data)
        {
            try
            {
                if (Instance != null)
                {
                    Instance.UpdateChat(data);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Ошибка иницилизации чата" + exp.Message);
            }
        }
        private void UpdateChat(string data)
        {
            clearChat();
            //#updatechat&user_name~data|user_name~data
            string[] message = data.Split('&')[1].Split('|');
            int countMessage = message.Length;

            if (countMessage <= 0)
            {
                return;
            }
            for (int i = 0; i < countMessage - 1; i++)
            {

                try
                {
                    if (string.IsNullOrEmpty(message[i]))
                    {
                        continue;
                    }
                    print(String.Format("[{0}]:{1}.", message[i].Split('~')[0], message[i].Split('~')[1]));

                }
                catch (Exception exp)
                {
                    ErrorLog.ErrorLogWriter(DateTime.Now + " Error with updateChat: " + exp.Message);
                    continue;
                }
            }
        }
        private void print(string msg)
        {

            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(Printer, msg);
                return;
            }

            if (ChatBox.Text.Length == 0)
            {
                ChatBox.AppendText(msg);
                chat_textBox.Clear();
            }
            else
            {
                ChatBox.AppendText(Environment.NewLine + msg);
                chat_textBox.Clear();
            }
            ChatBox.SelectionStart = ChatBox.Text.Length;
            ChatBox.ScrollToEnd();
        }
        private void clearChat()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(Cleaner);
                return;
            }
            ChatBox.Clear();
        }
        public static void send(string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int bytesSent = connection.connect._serverSocket.Send(buffer);
            }
            catch (Exception e)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now + " error with send " + e.Message);
            }
        }
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string data = chat_textBox.Text;
                if (string.IsNullOrEmpty(data))
                {
                    return;
                }
                send("#newmsg&" + data);
            }
            catch
            {
                MessageBox.Show("Error with send data");
            }


        }
        private  void Create_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new page.create_lobby());
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            if(!user_settings.inGame)
            {
                send("#closeapp&" + user_settings.user_name + "&" + user_settings.id_connection);
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите покинуть игру", "Выход", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    send("#leaving_game&" + user_settings.id_room + "&" + user_settings.user_name + "&" + user_settings.id_connection + "&not_ready&" + user_settings.max_payers_in_room + page.game_window.score + "&" + user_settings.id_user_in_db);
                }
            }
        }

        public static void BalanceFromServer(string data)
        {
            if (Instance != null)
            {
                Instance.BalanceInfo(data);
            }
        }
        private void BalanceInfo(string data)
        {
            if (!Dispatcher.CheckAccess())
            {

                Dispatcher.Invoke(Balanceinfo,data);
                return;
            }
            Balance.Text = "Баланс: " + user_settings.balance;
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new Menu.Results.Result());
        }
        private void balance_Click(object sender, RoutedEventArgs e)
        {
            BalanceForm.DialogBalanceForm dialogBalanceForm = new BalanceForm.DialogBalanceForm();
            dialogBalanceForm.Show();
        }
        private void Find_Click(object sender, RoutedEventArgs e)
        {
            if (!user_settings.inLobby)
            {
                MainWindow.send("#find_lobby");
                MainFrame.NavigationService.Navigate(new Page1());
            }
        }
    }
}
