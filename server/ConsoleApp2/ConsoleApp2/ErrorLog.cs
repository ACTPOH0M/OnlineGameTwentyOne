using System;
using System.IO;

namespace ConsoleApp2
{
    class ErrorLog
    {
        public static void Log(string data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("ServerErrorLog.txt", true))
                {
                    writer.WriteLine(data);
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка записи в ServerLog : " + e.Message);
            }
        }
    }
}
