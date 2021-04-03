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
    /// WordBook.xaml 的交互逻辑
    /// </summary>
    public partial class WordBook : Page
    {
        private List<Word> res = new List<Word>();
        public WordBook()
        {
            InitializeComponent();
        }

        private void TextBoxWordSearch_OnKeyUp(object sender, KeyEventArgs e)
        {
            GridWordContainer.Visibility = Visibility.Collapsed;
            ListBoxSelection.Visibility = Visibility.Visible;
            res = WordList.Words.OrderBy(t => t.english.IndexOf(TextBoxWordSearch.Text, StringComparison.CurrentCultureIgnoreCase) == -1 ? 9999999 : t.english.IndexOf(TextBoxWordSearch.Text, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (res.Count == 0 || !res[0].english.Contains(TextBoxWordSearch.Text))
            {
                ListBoxSelection.Visibility = Visibility.Collapsed;
            }
            else
            {
                res = res.GetRange(0, Math.Min(res.Count, Math.Max(res.FindIndex(t => !t.english.Contains(TextBoxWordSearch.Text)), 0)));
                ListBoxSelection.ItemsSource = res.Select(t => t.english);
                ListBoxSelection.Visibility = Visibility.Visible;
            }

        }

        private void ListBoxSelection_OnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                ListBoxSelection.Visibility = Visibility.Collapsed;
                GridWordContainer.Visibility = Visibility.Visible;
                TextBoxWordSearch.Text = res[ListBoxSelection.SelectedIndex].english;
                GridWordContainer.Children.Clear();
                GridWordContainer.Children.Add(new WordItem(res[ListBoxSelection.SelectedIndex]));
            }
            catch { }

        }
    }
}
