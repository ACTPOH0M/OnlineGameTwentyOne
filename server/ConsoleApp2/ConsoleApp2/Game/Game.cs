using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.Game
{


    public class Game
    {
        public static List<string> cards = new List<string>()
        {
            //36 карт
        "11","2","3","4","9","10","8","6","7",
        "11","2","3","4","9","10","8","6","7",
        "11","2","3","4","9","10","8","6","7",
        "11","2","3","4","9","10","8","6","7"
        };
        public static string Take_First_Cards()
        {
            
                string data = "";
                for (int i = 0; i < Rival.Rival_list.Count; i++)
                {
                    if (Rival.Rival_list[i].id_room == Client.str[1])
                    {
                        int index = new Random().Next(0, cards.Count-1);
                        int score = Convert.ToInt32(Rival.Rival_list[i].score);
                        score += Convert.ToInt32(cards[index]);
                        Rival.players_in_game updateScore = new Rival.players_in_game(Rival.Rival_list[i].id_room, Rival.Rival_list[i].id_player_connection, Rival.Rival_list[i].userName, Rival.Rival_list[i].status, Rival.Rival_list[i].max_players, score.ToString());
                        Rival.Rival_list[i] = updateScore;
                        data += String.Format("{0}~{1}|", Rival.Rival_list[i].userName, cards[index]);
                        cards.RemoveAt(index);
                    }
                }

            return data;
        }
        public static string take_card()
        {
            string data = "";
            for (int i = 0; i < Rival.Rival_list.Count; i++)
            {
                if (Rival.Rival_list[i].id_room == Client.str[1] && Rival.Rival_list[i].userName == Client.str[2])
                {
                    int index = new Random().Next(0, cards.Count-1);
                    int score = Convert.ToInt32(Rival.Rival_list[i].score);
                    score += Convert.ToInt32(cards[index]);
                    Rival.players_in_game updateScore = new Rival.players_in_game(Rival.Rival_list[i].id_room, Rival.Rival_list[i].id_player_connection, Rival.Rival_list[i].userName, Rival.Rival_list[i].status, Rival.Rival_list[i].max_players, score.ToString());
                    Rival.Rival_list[i] = updateScore;
                    data += String.Format("{0}~{1}|", Rival.Rival_list[i].userName, cards[index]);
                    cards.RemoveAt(index);
                    Console.WriteLine(data);
                }
            }
            return data;
        }
    }
}
