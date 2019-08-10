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
        BrandView brandViewClass = new BrandView();
        DateImpressionView dateImpressionViewClass = new DateImpressionView();

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
            Properties.Settings.Default.ProjectBaseConnectionString);


        public Home()
        {
            InitializeComponent();
        }

        private void Show_Industry(object sender, RoutedEventArgs e)
        {
            ShowIndustries();
        }

        private void Show_Brand(object sender, RoutedEventArgs e)
        {
            ShowBrands();
        }

        private void Show_DateAndImpressionType(object sender, RoutedEventArgs e)
        {
            ShowDateAndImpressionType();
        }

        #region Show Industry

        private void ShowIndustries()
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            if (chosenItems.Any())
            {
                var data = from p in dc.Industry select p;
                var inner = PredicateBuilder.False<Industry>();
                var outer = PredicateBuilder.True<Industry>();
                var firstItem = chosenItems.ElementAt(0);
                outer = outer.And(p => firstItem.Industry == p.industry);
                if (firstItem.SubIndustry != null) outer = outer.And(p => firstItem.SubIndustry == p.subIndustry);
                if (firstItem.SubIndustry2 != null) outer = outer.And(p => firstItem.SubIndustry2 == p.subIndustry2);
                if (firstItem.SubIndustry3 != null) outer = outer.And(p => firstItem.SubIndustry3 == p.subIndustry3);
                if (chosenItems.Count > 1)
                {
                    for (int i = 1; i < chosenItems.Count; i++)
                    {
                        var outer2 = PredicateBuilder.True<Industry>();
                        var item = chosenItems.ElementAt(i);
                        outer2 = outer2.And(p => item.Industry == p.industry);
                        if (item.SubIndustry != null) outer2 = outer2.And(p => item.SubIndustry == p.subIndustry);
                        if (item.SubIndustry2 != null) outer2 = outer2.And(p => item.SubIndustry2 == p.subIndustry2);
                        if (item.SubIndustry3 != null) outer2 = outer2.And(p => item.SubIndustry3 == p.subIndustry3);
                        inner = inner.Or(outer);
                        inner = inner.Or(outer2);
                    }
                    data = dc.Industry.Where(inner);
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
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
            else
            {
                var data = from p in dc.Industry select p;
                if (dc.DatabaseExists())
                {
                    DataGrid.ItemsSource = null;
                    DataGrid.ItemsSource = data;
                }

            }
        }

        #endregion

        #region Show Brands
        private void ShowBrands()
        {
            List<ChosenBrands> chosenBrands = brandViewClass.BrandsList;
            if (chosenBrands.Any())
            {
                var ChosenBrands = chosenBrands.Select(c => c.Brand);
                var ChosenBrandsOwner = chosenBrands.Select(c => c.BrandOwner);
                ChosenBrands = ChosenBrands.Where(c => c != null);
                ChosenBrandsOwner = ChosenBrandsOwner.Where(c => c != null);
                var BrandOwnerId = dc.BrandOwner.Select(b => b.BrandOwner_id);
                if (ChosenBrands.Any() && ChosenBrandsOwner.Any())
                {
                    var data =
                        from Br in dc.Brand
                        from BrO in dc.BrandOwner
                        where (ChosenBrands.Contains(Br.brandName) || ChosenBrandsOwner.Contains(BrO.brandOwner)) && Br.bOwner_id == BrO.BrandOwner_id
                        select new
                        {
                            Br.Brand_id,
                            Br.brandName,
                            Br.bOwner_id,
                            BrO.brandOwner
                        };
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
                    }
                }
                else if (ChosenBrands.Any() && !ChosenBrandsOwner.Any())
                {
                    var data =
                        from Br in dc.Brand
                        from BrO in dc.BrandOwner
                        where (ChosenBrands.Contains(Br.brandName) && Br.bOwner_id == BrO.BrandOwner_id)
                        select new
                        {
                            Br.Brand_id,
                            Br.brandName,
                            Br.bOwner_id
                        };
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
                    }
                }
                else
                {
                    var data = dc.BrandOwner.Where(p => ChosenBrandsOwner.Contains(p.brandOwner));
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
                    }
                }
            }
            else
            {
               
            }
        }
        #endregion

        #region Show Date and ImpressionType

        private void ShowDateAndImpressionType()
        {
            string logic = dateImpressionViewClass.GetLogicValue();
            DateTime? StartDate = dateImpressionViewClass.GetStartDateValue();
            DateTime? EndDate = dateImpressionViewClass.GetEndDateValue();
            switch (logic)
            {
                case "both":
                    var data = dc.db_main.Where(m => StartDate <= m.xDate && EndDate >= m.xDate);
                    ImpressionTypeAdd(data);
                    break;
                case "start":
                    var data2 = dc.db_main.Where(m => StartDate <= m.xDate);
                    ImpressionTypeAdd(data2);
                    break;
                case "end":
                    var data3 = dc.db_main.Where(m => EndDate >= m.xDate);
                    ImpressionTypeAdd(data3);
                    break;
                case "none":
                    var data4 = dc.db_main;
                    ImpressionTypeAdd(data4);
                    break;
                default:
                    var data5 = dc.db_main;
                    ImpressionTypeAdd(data5);
                    break;
            }
        }

        private void ImpressionTypeAdd(IQueryable<db_main> data)
        {
            string type = dateImpressionViewClass.GetImpressionTypeValue();
            switch (type)
            {
                case "both":
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
                    }
                    break;
                case "PC":
                   var  data2 = data.Where(m => m.impressionType == "pc");
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data2;
                    }
                    break;
                case "MOBILE":
                    var data3 = data.Where(m => m.impressionType == "mobile");
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data3;
                    }
                    break;
                default:
                    if (dc.DatabaseExists())
                    {
                        DataGrid.ItemsSource = null;
                        DataGrid.ItemsSource = data;
                    }
                    break;
            }
        }

        #endregion

    }
}
