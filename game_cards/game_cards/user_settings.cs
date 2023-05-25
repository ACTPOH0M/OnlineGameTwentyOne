using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace game_cards
{

    class user_settings
    {
        public static string user_name { get; set; }
        public static string message { get; set; }
        public static string game_message { get; set; }
        public static string id_connection { get; set; }
        public static string id_room { get; set; }
        public static string max_payers_in_room { get; set; }
        public static string id_user_in_db { get; set; }
        public static string balance { get; set; }
        public static bool inGame { get; set; }
        public static bool inLobby { get; set; }
    }
}
