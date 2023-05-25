using System;
using System.Windows;
using System.Windows.Media;

namespace game_cards.BalanceForm
{
    /// <summary>
    /// Логика взаимодействия для DialogBalanceForm.xaml
    /// </summary>
    public partial class DialogBalanceForm : Window
    {
        public DialogBalanceForm()
        {
            InitializeComponent();
        }

        private void Update_Balance_Click(object sender, RoutedEventArgs e)
        {
            //валидацию
            if (!String.IsNullOrEmpty(card_number.Text))
            {
                Validate(card_number.Text);
                card_number.BorderBrush = Brushes.Black;
            }
            else
            {
                card_number.BorderBrush = Brushes.Red;
                ErrorLabel.Content = "Поле Номер карты пустое";
            }
        }

        private void Validate(string data)
        {

            //if (!System.Text.RegularExpressions.Regex.IsMatch(data, "^[0-9]+"))
            //{
                int x = Convert.ToInt32(data);
                if ( x == 12)
                {
                    if (!String.IsNullOrEmpty(balance.Text))
                    {
                        int y = Convert.ToInt32(balance.Text);
                        if (y>=1000)
                        {
                            balance.BorderBrush = Brushes.Black;
                            MainWindow.send("#update_balance&" + user_settings.id_user_in_db + "&" + card_number.Text + "&" + balance.Text);
                            this.Close();
                        }else
                        {
                            balance.BorderBrush = Brushes.Red;
                            ErrorLabel.Content = "Минмальная сумма пополнения 1000";
                        }
                    }
                    else
                    {
                        balance.BorderBrush = Brushes.Red;
                        ErrorLabel.Content = "Поле Сумма пополнения пустое";
                    }
                }
                else
                {
                    ErrorLabel.Content = "Длина номера карты должна быть 12 символов";
                }
            //}
            //else
            //{
            //    ErrorLabel.Content = "Некорректное заполнение поля Номер карты";
            //}
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
