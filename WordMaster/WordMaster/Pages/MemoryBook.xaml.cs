using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WordMaster.Class;

namespace WordMaster.Pages
{
    /// <summary>
    /// MemoryBook.xaml 的交互逻辑
    /// </summary>
    public partial class MemoryBook : Page
    {
        public ObservableCollection<MemWord> MyList { get; set; }
        public MemoryBook()
        {
            InitializeComponent();
            MyList = new ObservableCollection<MemWord>();
            WordList.Words.ForEach(t =>
            {
                if (WordList.Memory.Keys.Contains(t.id))
                {
                    MyList.Add(new MemWord()
                    {
                        id = t.id,
                        chinese = t.chinese,
                        english = t.english,
                        _ischeck = WordList.Memory[t.id],
                        ismistake = WordList.MyTest.ContainsKey(t.id) && WordList.MyTest[t.id] == 2,
                        sound = t.sound
                    });
                }
            });
            DataGrid.ItemsSource = MyList;
        }
    }

    public class MemWord
    {
        public int id { get; set; }
        public string english { get; set; }
        public string chinese { get; set; }
        public string sound { get; set; }

        public bool ismistake { get; set; }

        public bool _ischeck;
        public bool ischeck
        {
            get => _ischeck;
            set
            {
                _ischeck = value;
                WordList.Memory[id] = value;
                var cmd = new SQLiteCommand("INSERT INTO \"memory\"(\"id\", \"ischeck\" , \"adddate\") VALUES (" + id + ", \"" + value + "\" , \"" + DateTime.Now.Date + "\")", Common.SQL);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
