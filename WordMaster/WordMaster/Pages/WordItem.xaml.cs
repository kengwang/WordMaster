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
using WordMaster.Class;
using SpeechLib;

namespace WordMaster.Pages
{
    /// <summary>
    /// WordItem.xaml 的交互逻辑
    /// </summary>
    public partial class WordItem : UserControl
    {
        private Word word;

        public void LoadWord(Word word)
        {
            this.Visibility = Visibility.Visible;
            this.word = word;
            TextBlockChinese.Text = word.chinese;
            TextBlockWord.Text = word.english;
            TextBlockSound.Text = word.sound;
            switch (Common.ShowType)
            {
                case WordShowType.NoChinese:
                    TextBlockChinese.Text = "****";
                    break;
                case WordShowType.NoEnglish:
                    TextBlockWord.Text = "****";
                    TextBlockSound.Visibility = Visibility.Hidden;
                    break;
            }
            try
            {
                this.Background = null;
                if (!WordList.MyTest.ContainsKey(word.id))
                {
                    WordStatusText.ToolTip = "未检测";
                    WordStatus.Kind = PackIconKind.Circle;
                }
                if (WordList.MyTest[word.id] == 2)
                {
                    WordStatusText.ToolTip = "错误";
                    WordStatus.Kind = PackIconKind.CloseThick;
                    this.Background = Brushes.IndianRed;
                }
                else if (WordList.MyTest[word.id] == 1)
                {
                    WordStatusText.ToolTip = "正确";
                    WordStatus.Kind = PackIconKind.Tick;
                    this.Background = Brushes.Green;
                }
            }
            catch { }
            if (WordList.Memory.ContainsKey(word.id))
                ButtonAddMem.IsEnabled = false;
            else ButtonAddMem.IsEnabled = true;
        }

        public WordItem(Word word)
        {
            InitializeComponent();
            LoadWord(word);
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
            LoadWord(WordList.Words[word.id++]);
        }

        private void TextBlockWord_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlockWord.Text = word.english;
            TextBlockSound.Visibility = Visibility.Visible;
        }

        private void TextBlockChinese_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlockChinese.Text = word.chinese;
        }

        private void WordStatusText_OnClick(object sender, RoutedEventArgs e)
        {
            if (!WordList.WordsToTest.Contains(word))
                WordList.WordsToTest.Add(word);
        }
    }
}
