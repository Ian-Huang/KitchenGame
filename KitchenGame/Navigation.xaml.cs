using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;


namespace KitchenGame
{
    /// <summary>
    /// Navigation.xaml 的互動邏輯
    /// </summary>
    public partial class Navigation : NavigationWindow
    {
        
        public Navigation()
        {
            InitializeComponent();
            this.SizeChanged += new SizeChangedEventHandler(Navigation_SizeChanged);
        }

        void Navigation_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Page page = (Page)this.Content;
            page.Width = e.NewSize.Width;
            page.Height = e.NewSize.Height;
        }
    }
}
