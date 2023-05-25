using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace game_cards.page
{
    /// <summary>
    /// Логика взаимодействия для create_lobby.xaml
    /// </summary>
    public partial class create_lobby : Page
    {
        
        public create_lobby()
        {
            InitializeComponent();
            user_settings.inLobby = false;
        }
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Page1());
        }

        private void CreateRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (!System.Text.RegularExpressions.Regex.IsMatch(Name_room_textBox.Text, @"^[a-zA-Z]"))
                {
                    MainWindow.send("#create_lobby&" + Name_room_textBox.Text + "&" + bet.Content + "&" + user_settings.user_name + "&" + Slider_players.Value.ToString());
                    NavigationService.Navigate(new Page1());
                }
                else
                {
                    MessageBox.Show("Можно использовать только латиницу");
                }
               
            }
            catch(Exception exp)
            {
                ErrorLog.ErrorLogWriter(DateTime.Now +" Error with create lobby: "+exp.Message);
            }
        }

        
    }
}
