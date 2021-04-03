using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WordMaster.Pages;

namespace WordMaster
{
    public static class Common
    {
        public static Frame MainFrame;
        public static SQLiteConnection SQL = new SQLiteConnection("Data Source=Resources/dic.db;Version=3;").OpenAndReturn();
        public static WordShowType ShowType = WordShowType.All;

        public static void Invoke(Action action)
        {
            Application.Current.Dispatcher.Invoke(() => { action(); });
        }
    }

    public enum WordShowType
    {
        All,
        NoEnglish,
        NoChinese
    }
}
