using System;
using System.Collections.Concurrent;
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

namespace App_Project
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDictionary<string, UserControl> views = new ConcurrentDictionary<string, UserControl>();
        public MainWindow()
        {
            InitializeComponent();
            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
            MaximizeButton.Click += (s, e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            CloseButton.Click += (s, e) => Close();
            StartPage();
        }

        private void StartPage()
        {
            Home home = new Home();
            MainGrid.Children.Add(home);
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            var item = ListViewMenu.Items[index] as ListViewItem;
            string viewName = "HOME";
            if (item.Content != null)
            {
                viewName = (item.Content as TextBlock).Text;
            }
            try
            {
                DisplayView(viewName);
            }
            catch(ArgumentException ex)
            {

            }
            
        }

        private void DisplayView(string viewName)
        {
            if(MainGrid == null)
            {
                return;
            }
            MainGrid.Children.Clear();
            MainGrid.Children.Add(PickCorrectView(viewName));
        }
        
        private UserControl PickCorrectView(string viewName)
        {
            if (views.ContainsKey(viewName))
            {
                return views[viewName];
            }
            else
            {
                return AddNewView(viewName);
            }
        }

        private UserControl AddNewView(string viewName)
        {
            UserControl view;
            switch (viewName)
            {
                case "HOME":
                    view = new Home();
                    break;
                case "INDUSTRY":
                    view = new IndustryView();
                    break;
                default:
                    throw new ArgumentException("Can't recognize specified view name", "viewName");
            }
            views.Add(viewName, view);
            return view;
        }
        
    }
}
