using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using System.IO;
using System.Runtime.Caching;
using System.Collections;

namespace App_Project
{
    /// <summary>
    /// Logika interakcji dla klasy Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {

        IndustryView industryViewClass = new IndustryView();

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
            Properties.Settings.Default.ProjectBaseConnectionString);


        public Home()
        {
            InitializeComponent();

        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            Działa();
            //NieDziała();
            //Union();
           
        }
        private void Działa()
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var ChosenIndustries = chosenItems.Select(c => c.Industry);
            var ChosenSubIndustries = chosenItems.Select(c => c.SubIndustry);
            var ChosenSubIndustries2 = chosenItems.Select(c => c.SubIndustry2);
            var ChosenSubIndustries3 = chosenItems.Select(c => c.SubIndustry3);
            ChosenSubIndustries = ChosenSubIndustries.Where(c => c != null);
            ChosenSubIndustries2 = ChosenSubIndustries2.Where(c => c != null);
            ChosenSubIndustries3 = ChosenSubIndustries3.Where(c => c != null);
            var data = dc.Industry.Where(i => ChosenIndustries.Contains(i.industry));
            if (ChosenSubIndustries.Any()) data = data.Where(i => ChosenSubIndustries.Contains(i.subIndustry));
            if (ChosenSubIndustries2.Any()) data = data.Where(i => ChosenSubIndustries2.Contains(i.subIndustry2));
            if (ChosenSubIndustries3.Any()) data = data.Where(i => ChosenSubIndustries3.Contains(i.subIndustry3));
            if (dc.DatabaseExists())
            {
                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = data;
            }
        }

        private void NieDziała()
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var data = from p in dc.Industry select p;
            var inner = PredicateBuilder.False<Industry>();
            var outer = PredicateBuilder.True<Industry>();
            var firstI = chosenItems.ElementAt(0);
            outer = outer.And(p => firstI.Industry.Contains(p.industry));
            if (firstI.SubIndustry != null) outer = outer.And(p => firstI.SubIndustry.Contains(p.subIndustry));
            if (firstI.SubIndustry2 != null) outer = outer.And(p => firstI.SubIndustry2.Contains(p.subIndustry2));
            if (firstI.SubIndustry3 != null) outer = outer.And(p => firstI.SubIndustry3.Contains(p.subIndustry3));
            if (chosenItems.Count > 1)
            {
                for (int i = 1; i < chosenItems.Count; i++)
                {
                    var outer2 = PredicateBuilder.True<Industry>();
                    var item = chosenItems.ElementAt(i);
                    outer2 = outer2.And(p => item.Industry.Contains(p.industry));
                    if (item.SubIndustry != null) outer2 = outer2.And(p => item.SubIndustry.Contains(p.subIndustry));
                    if (item.SubIndustry2 != null) outer2 = outer2.And(p => item.SubIndustry2.Contains(p.subIndustry2));
                    if (item.SubIndustry3 != null) outer2 = outer2.And(p => item.SubIndustry3.Contains(p.subIndustry3));
                    inner = inner.Or(outer2);
                }
                data = dc.Industry.Where(inner);
                if (dc.DatabaseExists())
                {
                    DataGrid.ItemsSource = null;
                    DataGrid.ItemsSource = data.ToList();
                }
            }
            else
            {
                data = dc.Industry.Where(outer);
                if (dc.DatabaseExists())
                {
                    DataGrid.ItemsSource = null;
                    DataGrid.ItemsSource = data;
                }
            }
        }

        private void Union()
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var FirstItem = chosenItems.ElementAt(0);
            var SecondItem = chosenItems.ElementAt(1);
            var data = dc.Industry.Where(p => FirstItem.Industry.Contains(p.industry));
            var data2 = dc.Industry.Where(p => SecondItem.Industry.Contains(p.industry));
            if (FirstItem.SubIndustry != null) data = data.Where(p => FirstItem.SubIndustry.Contains(p.subIndustry));
            if (FirstItem.SubIndustry2 != null) data = data.Where(p => FirstItem.SubIndustry2.Contains(p.subIndustry2));
            if (FirstItem.SubIndustry3 != null) data = data.Where(p => FirstItem.SubIndustry3.Contains(p.subIndustry3));
            if (SecondItem.SubIndustry != null) data = data.Where(p => SecondItem.SubIndustry.Contains(p.subIndustry));
            if (SecondItem.SubIndustry2 != null) data = data.Where(p => SecondItem.SubIndustry2.Contains(p.subIndustry2));
            if (SecondItem.SubIndustry3 != null) data = data.Where(p => SecondItem.SubIndustry3.Contains(p.subIndustry3));

            //data = data.Union(data2);
            if (dc.DatabaseExists())
            {
                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = data;
            }
        }
    }
}
