using System;
using System.Collections.Generic;
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
using MaterialDesignThemes.Wpf;
using SpeechLib;
using WordMaster.Class;

namespace WordMaster.Pages
{
    /// <summary>
    /// SmallTest.xaml 的交互逻辑
    /// </summary>
    public partial class SmallTest : Page
    {
        bool answerd = false;
        public SmallTest()
        {
            InitializeComponent();
            memidx = WordList.Memory.Count - 1;
            ButtonBase1_OnClick(null, null);
        }
        private Word word;
        private int memidx = 0;
        public void LoadWord(Word word)
        {
            answerd = false;
            TextBoxAns.Text = "";
            this.Visibility = Visibility.Visible;
            this.word = word;
            TextBlockChinese.Text = word.chinese;
            TextBlockWord.Text = "****";
            TextBlockSound.Text = "****";
            this.Background = null;

            try
            {
                if (!WordList.MyTest.ContainsKey(word.id))
                {
                    WordStatusText.ToolTip = "未检测";
                    WordStatus.Kind = PackIconKind.Circle;
                }
                if (WordList.MyTest[word.id] == 2)
                {
                    WordStatusText.ToolTip = "错误";
                    WordStatus.Kind = PackIconKind.CloseThick;
                }
                else if (WordList.MyTest[word.id] == 1)
                {
                    WordStatusText.ToolTip = "正确";
                    WordStatus.Kind = PackIconKind.Tick;
                }
            }
            catch { }
            if (WordList.Memory.ContainsKey(word.id))
                ButtonAddMem.IsEnabled = false;
            else ButtonAddMem.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run((() =>
            {
                SpVoice voice = new SpVoice
                {
                    Rate = 0, //语速,[-10,10]
                    Volume = 100 //音量,[0,100]
                };
                voice.Voice = voice.GetVoices().Item(0); //语音库
                voice.Speak(word.english);
            }));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Run((() =>
            {
                WordList.Memory[word.id] = false;
                var cmd = new SQLiteCommand("INSERT INTO \"memory\"(\"id\", \"ischeck\" , \"adddate\") VALUES (" + word.id + ", \"False\" , \"" + DateTime.Now.Date + "\")", Common.SQL);
                cmd.ExecuteNonQuery();
                Common.Invoke((() =>
                {
                    ButtonAddMem.IsEnabled = false;
                }));
            }));

        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            if (WordList.Memory.Count != 0 && memidx >= 0)
            {
                TextBlockSource.Text = "单词来源: 记忆本";
                
                LoadWord(WordList.Words[WordList.Memory.Keys.ToArray()[memidx--]]);
            }
            else if (WordList.WordsToTest.Count != 0)
            {
                TextBlockSource.Text = "单词来源: 我的选测";
                var word = WordList.WordsToTest[new Random().Next(WordList.WordsToTest.Count - 1)];
                WordList.WordsToTest.Remove(word);
                LoadWord(word);
            }
            else
            {
                TextBlockSource.Text = "单词来源: 随机选词";
                LoadWord(WordList.Words[new Random().Next(WordList.Words.Count - 1)]);
            }
            
        }

        private void WordStatusText_OnClick(object sender, RoutedEventArgs e)
        {
            if (!WordList.WordsToTest.Contains(word))
                WordList.WordsToTest.Add(word);
        }

        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !answerd)
            {
                answerd = true;
                TextBlockWord.Text = word.english;
                TextBlockSound.Text = word.sound;
                if (TextBoxAns.Text.ToLower().Trim() != word.english.Trim().ToLower())
                {
                    WordList.MyTest[word.id] = 2;
                    var cmd = new SQLiteCommand("INSERT INTO \"mytest\"(\"id\", \"status\") VALUES (" + word.id + ", 2 )", Common.SQL);
                    cmd.ExecuteNonQuery();
                    WordStatusText.ToolTip = "错误";
                    WordStatus.Kind = PackIconKind.CloseThick;
                    this.Background = Brushes.IndianRed;
                }
                else
                {
                    WordList.MyTest[word.id] = 1;
                    var cmd = new SQLiteCommand("INSERT INTO \"mytest\"(\"id\", \"status\") VALUES (" + word.id + ", 1 )", Common.SQL);
                    cmd.ExecuteNonQuery();
                    WordStatusText.ToolTip = "正确";
                    WordStatus.Kind = PackIconKind.Tick;
                    this.Background = Brushes.Green;
                }
            }
        }
    }
}
