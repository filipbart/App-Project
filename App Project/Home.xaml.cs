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
           foreach (ChosenItems item in chosenItems)
           {
               if (chosenItems.Count == 1)
               {
                    if (item.SubIndustry == null)
                    {
                        var data =
                             from industry in dc.Industry
                             where industry.industry == item.Industry
                             select industry;
                        if (dc.DatabaseExists()) DataGrid.ItemsSource = data.ToList();
                    }
                    else
                    {
                        if (item.SubIndustry2 == null)
                        {
                            var data =
                                 from industry in dc.Industry
                                 where industry.industry == item.Industry 
                                    && industry.subIndustry == item.SubIndustry
                                 select industry;
                            if (dc.DatabaseExists()) DataGrid.ItemsSource = data.ToList();
                        }
                        else
                        {
                            if (item.SubIndustry3 == null)
                            {
                                var data =
                                    from industry in dc.Industry
                                    where industry.industry == item.Industry 
                                        && industry.subIndustry == item.SubIndustry 
                                        && industry.subIndustry2 == item.SubIndustry2
                                    select industry;
                                if (dc.DatabaseExists()) DataGrid.ItemsSource = data.ToList();
                            }
                            else
                            {
                                var data =
                                    from industry in dc.Industry
                                    where industry.industry == item.Industry
                                        && industry.subIndustry == item.SubIndustry
                                        && industry.subIndustry2 == item.SubIndustry2
                                        && industry.subIndustry3 == item.SubIndustry3
                                    select industry;
                                if (dc.DatabaseExists()) DataGrid.ItemsSource = data.ToList();
                            }
                        }
                    }
                }
                else if (chosenItems.Count > 1)
                {
                    if (item.SubIndustry == null)
                    {
                       
                    }
                }

        }

        //  GetList();

    }

        public void GetList()
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            //DataGrid.ItemsSource = null;
            //DataGrid.ItemsSource = chosenItems;
            foreach(ChosenItems item in chosenItems)
            {
                MessageBox.Show(item.Industry);
            }
            
        }
    }
}
