using game_cards.Login_register;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace game_cards.login_register
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Window
    {
        public login()
        {
            InitializeComponent();
        }

        bool str = false;

        private void buttonLogin_click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(login_box.Text) && !String.IsNullOrEmpty(Password_login.Password))
            {
                login_box.BorderBrush = Brushes.Black;
                Password_login.BorderBrush = Brushes.Black;
                var pass = HashPassword.hasPassword(Password_login.Password);
                if (str)
                {
                    connection.connect.Connect();
                    MainWindow.send("#login&" + login_box.Text + "&" + pass);

                    try
                    {

                        byte[] buffer = new byte[1024];
                        int bytesRec = connection.connect._serverSocket.Receive(buffer);
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);

                        string[] str = data.Split('&');
                        

                        
                        if (str[0] == "1")
                        {
                            user_settings.user_name = str[1];
                            user_settings.id_connection = str[2];
                            user_settings.id_user_in_db = str[3];

                            MainWindow main = new MainWindow();
                            main.Show();
                            Close();
                        }
                        else
                        {
                            login_box.BorderBrush = Brushes.Red;
                            Password_login.BorderBrush = Brushes.Red;
                            ErrorLabel.Text = str[2];
                        }
                    }
                    catch (Exception exp)
                    {
                        ErrorLog.ErrorLogWriter(DateTime.Now + " Error with Login: " + exp.Message);
                        ErrorLabel.Text = "Подключение недоступно, возможно сервер отключен";
                    }
                }
            }
            else
            {
                ErrorLabel.Text = "Поля незаполнены";
                login_box.BorderBrush = Brushes.Red;
                Password_login.BorderBrush = Brushes.Red;
            }
        }

        private void register_click(object sender, RoutedEventArgs e)
        {
            register reg = new register();
            reg.Show();
            this.Close();
        }

        private void login_box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(login_box.Text, "^[a-zA-Z0-9]") && login_box.Text.Length != 0)
            {
                ErrorLabel.Text = "Ввод возможен только на латинице";
                login_box.Foreground = Brushes.Red;
                login_box.Text.Remove(login_box.Text.Length - 1);
            }
            else
            {
                ErrorLabel.Text = "";
                login_box.Foreground = Brushes.Black;
                str = true;
            }
        }

        private void Password_login_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Password_login.Password, "^[a-zA-Z0-9]") && Password_login.Password.Length != 0)
            {
                ErrorLabel.Text = "Ввод возможен только на латинице";
                Password_login.Foreground = Brushes.Red;
                Password_login.Password.Remove(Password_login.Password.Length - 1);
            }
            else
            {
                ErrorLabel.Text = "";
                login_box.Foreground = Brushes.Black;
                str = true;
            }
        }
    }
}
