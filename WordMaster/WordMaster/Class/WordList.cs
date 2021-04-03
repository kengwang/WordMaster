using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace WordMaster.Class
{
    class WordList
    {
        public static List<Word> Words = new List<Word>();
        public static List<Word> WordsToTest = new List<Word>();
        public static Dictionary<int, int> MyTest = new Dictionary<int, int>();
        public static Dictionary<int, bool> Memory = new Dictionary<int, bool>();

        public static void LoadWordList()
        {
            try
            {
                Words = new List<Word>();
                var cmd = new SQLiteCommand("SELECT id,word,sound,chinese from dic", Common.SQL);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var word = new Word()
                    {
                        id = reader.GetInt32(0),
                        english = reader.GetString(1),
                        sound = reader.GetString(2),
                        chinese = reader.GetString(3)
                    };
                    Words.Add(word);
                }

                Words = Words.OrderBy(t => t.english).ToList();

                cmd = new SQLiteCommand("SELECT id,status from mytest", Common.SQL);
                // 0 - 未测试     1 - 测试过关    2 - 测试不过关
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    MyTest[reader.GetInt32(0)] = reader.GetInt32(1);
                }

                cmd = new SQLiteCommand("SELECT id,ischeck,adddate from memory", Common.SQL);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Memory[reader.GetInt32(0)] = reader.GetString(1) == "True";
                }
            }
            catch
            {

            }

        }
    }
}
