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
using MaterialDesignThemes.Wpf;
using WordMaster.Pages;

namespace WordMaster
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Common.MainFrame = MainFrame;
        }

        private void ButtonShowType_OnClick(object sender, RoutedEventArgs e)
        {
            switch (Common.ShowType)
            {
                case WordShowType.All:
                    ButtonShowType.Content = new PackIcon(){Kind = PackIconKind.Abc};
                    ButtonShowType.ToolTip = "隐藏中文";
                    Common.ShowType = WordShowType.NoChinese;
                    break;
                case WordShowType.NoChinese:
                    ButtonShowType.Content = new PackIcon() { Kind = PackIconKind.IdeogramCjkVariant};
                    ButtonShowType.ToolTip = "隐藏英文";
                    Common.ShowType = WordShowType.NoEnglish;
                    break;
                case WordShowType.NoEnglish:
                    ButtonShowType.Content = new PackIcon() { Kind = PackIconKind.Eye};
                    ButtonShowType.ToolTip = "全显示";
                    Common.ShowType = WordShowType.All;
                    break;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Common.MainFrame.Navigate(new MainPage());
        }
    }
}
