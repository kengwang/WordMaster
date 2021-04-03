using System;
using System.Collections.Generic;
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
    /// MainPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Task.Run((WordList.LoadWordList));
        }

        private void OpenWordBook(object sender, MouseButtonEventArgs e)
        {
            Common.MainFrame.Navigate(new WordBook());
        }

        private void OpenSmallTest(object sender, MouseButtonEventArgs e)
        {
            Common.MainFrame.Navigate(new SmallTest());
        }

        private void OpenMemoryBook(object sender, MouseButtonEventArgs e)
        {
            Common.MainFrame.Navigate(new MemoryBook());
        }
    }
}
