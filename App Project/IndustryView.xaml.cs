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
using System.Diagnostics;

namespace App_Project
{
    /// <summary>
    /// Logika interakcji dla klasy Industry.xaml
    /// </summary>
    public partial class IndustryView : UserControl
    {

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
            Properties.Settings.Default.ProjectBaseConnectionString);

        public static List<ChosenItems> chosenItems = new List<ChosenItems>();

        public IndustryView()
        {
            InitializeComponent();
            showIndustryList();
        }


    #region SelectionChanged
        private void IndustryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var addeditem in e.AddedItems)
            {
                var ind = addeditem as String;
                if (ind != null) showSubIndustryList(ind);
            }
            SubIndustryListBox2.ItemsSource = "";
            SubIndustryListBox3.ItemsSource = "";
        }

        private void SubIndustryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var addeditem in e.AddedItems)
            {
                string subind = addeditem as String;
                if (subind != null) showSubIndustryList2(subind);
            }
            SubIndustryListBox3.ItemsSource = "";
        }

        private void SubIndustryListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var addeditem in e.AddedItems)
            {
                string subind2 = addeditem as String;
                if (subind2 != null) showSubIndustryList3(subind2);
            }
        }

        #endregion

    #region ShowList

        private void showIndustryList()
        {
            var data =
               from i in dc.dict_industries
               group i.industry by i.industry into g
               select g.Key;
            if (dc.DatabaseExists()) IndustryListBox.ItemsSource = data.ToList();
        }
        private void showSubIndustryList(string ind)
        {
            var data =
                from i in dc.dict_industries
                where i.industry == ind
                group i.subIndustry by i.subIndustry into g
                select g.Key;
            if (dc.DatabaseExists()) SubIndustryListBox.ItemsSource = data.ToList();
        }

        private void showSubIndustryList2(string subind)
        {
            var data =
                from i in dc.dict_industries
                where i.subIndustry == subind
                group i.subIndustry2 by i.subIndustry2 into g
                select g.Key;
            if (dc.DatabaseExists()) SubIndustryListBox2.ItemsSource = data.ToList();
        }

        private void showSubIndustryList3(string subind2)
        {
            var data =
                from i in dc.dict_industries
                where i.subIndustry2 == subind2
                group i.subIndustry3 by i.subIndustry3 into g
                select g.Key;
            if (dc.DatabaseExists()) SubIndustryListBox3.ItemsSource = data.ToList();
        }


        #endregion

    #region AddItem
        
        public void AddItem(object sender, RoutedEventArgs e)
        {
            string industry = IndustryListBox.SelectedItem.ToString();
            string subindustry;
            string subindustry2;
            string subindustry3;
            if(SubIndustryListBox.SelectedIndex < 0)
            {
                subindustry = null;
            }
            else
            {
                subindustry = SubIndustryListBox.SelectedItem.ToString();
            }
            if (SubIndustryListBox2.SelectedIndex < 0)
            {
                subindustry2 = null;
            }
            else
            {
                subindustry2 = SubIndustryListBox2.SelectedItem.ToString();
            }
            if (SubIndustryListBox3.SelectedIndex < 0)
            {
                subindustry3 = null;
            }
            else
            {
                subindustry3 = SubIndustryListBox3.SelectedItem.ToString();
            }
            if (subindustry == null) chosenItems.Add(new ChosenItems() { Industry = industry });
            else if (subindustry2 == null) chosenItems.Add(new ChosenItems() { Industry = industry, SubIndustry = subindustry });
            else if (subindustry3 == null) chosenItems.Add(new ChosenItems() { Industry = industry, SubIndustry = subindustry, SubIndustry2 = subindustry2 });
            else chosenItems.Add(new ChosenItems() { Industry = industry, SubIndustry = subindustry, SubIndustry2 = subindustry2, SubIndustry3 = subindustry3 });
            ChosenIndustry.ItemsSource = null;
            ChosenIndustry.ItemsSource = chosenItems;
        }

        #endregion

    #region RemoveItem
        public void RemoveItem(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ChosenIndustry.SelectedIndex;
            if(selectedIndex != -1)
            { 
                chosenItems.RemoveAt(selectedIndex);
                ChosenIndustry.ItemsSource = null;
                ChosenIndustry.ItemsSource = chosenItems;
            }
        }

        

        #endregion

        public List<ChosenItems> IndustryList
        {
            get { return chosenItems; }
        }
    }
}
