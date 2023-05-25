using System.IO;

namespace game_cards
{
    class ErrorLog
    {
        public static void ErrorLogWriter(string data)
        {
            string path = "ErrorLog.txt";
            
            // добавление в файл
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                 writer.WriteLine(data);
            }
        }
    }
}
