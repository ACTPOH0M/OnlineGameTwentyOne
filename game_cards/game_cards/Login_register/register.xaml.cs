using game_cards.Login_register;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace game_cards.login_register
{
    /// <summary>
    /// Логика взаимодействия для register.xaml
    /// </summary>
    public partial class register : Window
    {
        public register()
        {
            InitializeComponent();
        }


        bool isOk, str = false;



        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(mail.Text) && !String.IsNullOrEmpty(Login.Text) && !String.IsNullOrEmpty(Password_register.Password) && !String.IsNullOrEmpty(NickName.Text))
                {
                    validation();
                }
                else
                {
                    ErrorLabel.Text = "Заполните все поля";
                }
            }catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now+" Error with Register "+exp.Message);
                ErrorLabel.Text = "Заполните все поля";
            }
        }

     

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            login log = new login();
            log.Show();
            Close();
        }


        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(Login.Text, "^[a-zA-Z0-9]") && Login.Text.Length != 0)
            {
                ErrorLabel.Text = "Ввод возможен только на латинице";
                Login.Foreground = Brushes.Red;
                Login.Text.Remove(Login.Text.Length - 1);
            }
            else
            {
                Login.Foreground = Brushes.Black;
                ErrorLabel.Text = ""; 
                str = true;
            }
        }

        private void NickName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(NickName.Text, "^[a-zA-Z0-9]") && NickName.Text.Length != 0)
            {
                ErrorLabel.Text = "Ввод возможен только на латинице";
                NickName.Foreground = Brushes.Red;
                NickName.Text.Remove(NickName.Text.Length - 1);
            }
            else
            {
                NickName.Foreground = Brushes.Black;
                ErrorLabel.Text = "";
                str = true;
            }
        }

        private void mail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(mail.Text, "^[a-zA-Z0-9]") && mail.Text.Length != 0)
            {
                ErrorLabel.Text = "Ввод возможен только на латинице";
                mail.Foreground = Brushes.Red;
                mail.Text.Remove(mail.Text.Length - 1);
            }
            else
            {
                mail.Foreground = Brushes.Black;
                ErrorLabel.Text = "";
                str = true;
            }
        }

        private void validation ()
        {
            if (!String.IsNullOrEmpty(mail.Text))
            {
                isOk = validateEmail(mail.Text);
            }
            else
            {
                mail.BorderBrush = Brushes.Red;
            }
            if (isOk)
            {
                mail.BorderBrush = Brushes.Black;
                if (!String.IsNullOrEmpty(Login.Text)&& Login.Text.Length<=20)
                {
                    Login.BorderBrush = Brushes.Black;
                    if (!String.IsNullOrEmpty(NickName.Text) && NickName.Text.Length <= 20)
                    {
                        NickName.BorderBrush = Brushes.Black;
                        if (!String.IsNullOrEmpty(Password_register.Password) && Password_register.Password.Length >= 8)
                        {
                            var pass = HashPassword.hasPassword(Password_register.Password);
                            if (str)
                            {
                                try
                                {
                                    Password_register.BorderBrush = Brushes.Black;
                                    connection.connect.Connect();
                                    MainWindow.send("#register&" + Login.Text + "&" + pass + "&" + mail.Text + "&" + NickName.Text);
                                    Send();
                                } catch (Exception exp)
                                {
                                    ErrorLabel.Text = exp.Message;
                                }
                            }
                        }
                        else
                        {
                            Password_register.BorderBrush = Brushes.Red;
                            ErrorLabel.Text = "Поле Пароль пустое или меньше 8 символов";
                        }
                    }
                    else
                    {
                        NickName.BorderBrush = Brushes.Red;
                        ErrorLabel.Text = "Поле Никнейма пустое или больше 20 символов";
                    }
                }
                else
                {
                    Login.BorderBrush = Brushes.Red;
                    ErrorLabel.Text = "Поле Логина пустое или больше 20 символов";
                }
            }
            else
            {
                mail.BorderBrush = Brushes.Red;
                ErrorLabel.Text = "Почта не подходит";
            }
        }

        static bool validateEmail(string email)
        {
            if (email == null)
            {
                return false;
            }
            if (new EmailAddressAttribute().IsValid(email))
            {
                return true;
            }
            else
            {

                return false;
            }
        }

        private  void Send()
        {
            if (isOk)
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
                    MessageBox.Show(str[1], "Пользователь не зарегистрирован! ");
                }
            }
        }
    }
}
