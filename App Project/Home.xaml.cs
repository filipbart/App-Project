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
    public partial class Home : UserControl {

        IndustryView industryViewClass = new IndustryView();

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
            Properties.Settings.Default.ProjectBaseConnectionString);


        public Home()
        {
            InitializeComponent();
            
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var item = chosenItems.ElementAt(0);
            if (chosenItems.Count == 1)
            {
                if (item.SubIndustry == null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry);
                    showList(data);
                }
                else if (item.SubIndustry != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry);
                    showList(data);
                }
                if (item.SubIndustry2 != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry)
                    .Where(x => x.subIndustry2 == item.SubIndustry2);
                    showList(data);
                }
                if (item.SubIndustry3 != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry)
                    .Where(x => x.subIndustry2 == item.SubIndustry2)
                    .Where(x => x.subIndustry3 == item.SubIndustry3);
                    showList(data);
                }
            }
            else if (chosenItems.Count > 1)
            {
                if (item.SubIndustry == null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry);
                    moreItems(data);
                }
                else if (item.SubIndustry != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry);
                    moreItems(data);
                }
                if (item.SubIndustry2 != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry)
                    .Where(x => x.subIndustry2 == item.SubIndustry2);
                    moreItems(data);
                }
                if (item.SubIndustry3 != null)
                {
                    var data = dc.Industry.Where(x => x.industry == item.Industry)
                    .Where(x => x.subIndustry == item.SubIndustry)
                    .Where(x => x.subIndustry2 == item.SubIndustry2)
                    .Where(x => x.subIndustry3 == item.SubIndustry3);
                    moreItems(data);
                }
            }
        }

        private void moreItems(IQueryable<Industry> data)
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var inner = PredicateBuilder.False<Industry>();
            var outer = PredicateBuilder.True<Industry>();
            for (int i = 1; i < chosenItems.Count; i++ )
            {
                var item = chosenItems.ElementAt(i);
                if (item.SubIndustry == null)
                {
                    outer = outer.And(x => x.industry == item.Industry);
                    inner = inner.Or(outer);
                    data = data.Where(inner);
                    showList(data);
                }
                else if (item.SubIndustry != null)
                {
                    inner = inner.Or(x => x.industry == item.Industry);
                    outer = outer.And(x => x.subIndustry == item.SubIndustry);
                    outer = outer.And(inner);
                    data = data.Where(outer);
                    showList(data);
                }
                if (item.SubIndustry2 != null)
                {
                    inner = inner.Or(x => x.industry == item.Industry);
                    outer = outer.And(x => x.subIndustry == item.SubIndustry);
                    outer = outer.And(x => x.subIndustry2 == item.SubIndustry2);
                    outer = outer.And(inner);
                    data = data.Where(outer);
                    showList(data);
                }
                if (item.SubIndustry3 != null)
                {
                    inner = inner.Or(x => x.industry == item.Industry);
                    outer = outer.And(x => x.subIndustry == item.SubIndustry);
                    outer = outer.And(x => x.subIndustry2 == item.SubIndustry2);
                    outer = outer.And(x => x.subIndustry3 == item.SubIndustry3);
                    outer = outer.And(inner);
                    data = data.Where(outer);
                    showList(data);
                }
            }
            
        }

        private void showList(IQueryable<Industry> data)
        {
            if (dc.DatabaseExists())
            {
                DataGrid.ItemsSource = null;
                DataGrid.ItemsSource = data.ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var i = chosenItems.Count;
            MessageBox.Show(i.ToString());
            /*foreach (ChosenItems i in chosenItems)
            {
                MessageBox.Show(i.Industry);
            }*/


        }
    }
}
