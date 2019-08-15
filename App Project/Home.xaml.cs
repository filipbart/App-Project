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
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

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

        private void ShowResults(object sender, RoutedEventArgs e)
        {
            ShowIndustries("show");
        }
        private void SaveButton(object sender, RoutedEventArgs e)
        {
            ShowIndustries("save");
        }

        #region Show Industry

        private void ShowIndustries(string type)
        {
            List<ChosenItems> chosenItems = industryViewClass.IndustryList;
            var data = from p in dc.DbAll select p;
            var inner = PredicateBuilder.False<DbAll>();
            var outer = PredicateBuilder.True<DbAll>();
            if (chosenItems.Any())
            {
                var firstItem = chosenItems.ElementAt(0);
                outer = outer.And(p => firstItem.Industry == p.industry);
                if (firstItem.SubIndustry != null) outer = outer.And(p => firstItem.SubIndustry == p.subIndustry);
                if (firstItem.SubIndustry2 != null) outer = outer.And(p => firstItem.SubIndustry2 == p.subIndustry2);
                if (firstItem.SubIndustry3 != null) outer = outer.And(p => firstItem.SubIndustry3 == p.subIndustry3);
                if (chosenItems.Count > 1)
                {
                    for (int i = 1; i < chosenItems.Count; i++)
                    {
                        var outer2 = PredicateBuilder.True<DbAll>();
                        var item = chosenItems.ElementAt(i);
                        outer2 = outer2.And(p => item.Industry == p.industry);
                        if (item.SubIndustry != null) outer2 = outer2.And(p => item.SubIndustry == p.subIndustry);
                        if (item.SubIndustry2 != null) outer2 = outer2.And(p => item.SubIndustry2 == p.subIndustry2);
                        if (item.SubIndustry3 != null) outer2 = outer2.And(p => item.SubIndustry3 == p.subIndustry3);
                        inner = inner.Or(outer);
                        inner = inner.Or(outer2);
                    }

                    ShowBrands(inner, type);
                }
                else
                {
                    ShowBrands(outer, type);
                }
            }
            else
            {
                ShowBrands(outer, type);
            }
        }

        #endregion

        #region Show Brands
        private void ShowBrands(Expression<Func<DbAll, bool>> value, string type)
        {
            List<ChosenBrands> chosenBrands = brandViewClass.BrandsList;
            var inner = PredicateBuilder.False<DbAll>();
            var outer = PredicateBuilder.True<DbAll>();
            if (chosenBrands.Any())
            {
                var data = from p in dc.DbAll select p;
                var ChosenBrands = chosenBrands.Select(c => c.Brand);
                var ChosenBrandsOwner = chosenBrands.Select(c => c.BrandOwner);
                ChosenBrands = ChosenBrands.Where(c => c != null);
                ChosenBrandsOwner = ChosenBrandsOwner.Where(c => c != null);
                if (ChosenBrands.Any() && ChosenBrandsOwner.Any())
                {
                    inner = inner.Or(p => ChosenBrands.Contains(p.brand));
                    inner = inner.Or(p => ChosenBrandsOwner.Contains(p.brandOwner));
                    if (outer.ToString() == value.ToString()) ShowDateAndImpressionType(inner, type);
                    else
                    {
                        inner = inner.Or(value);
                        ShowDateAndImpressionType(inner, type);
                    }
                }
                else if (ChosenBrands.Any() && !ChosenBrandsOwner.Any())
                {
                    inner = inner.Or(p => ChosenBrands.Contains(p.brand));
                    if (outer.ToString() == value.ToString()) ShowDateAndImpressionType(inner, type);
                    else
                    {
                        inner = inner.Or(value);
                        ShowDateAndImpressionType(inner, type);
                    }
                }
                else
                {
                    inner = inner.Or(p => ChosenBrandsOwner.Contains(p.brandOwner));
                    if (outer.ToString() == value.ToString()) ShowDateAndImpressionType(inner, type);
                    else
                    {
                        inner = inner.Or(value);
                        ShowDateAndImpressionType(inner, type);
                    }
                }
            }
            else
            {
                ShowDateAndImpressionType(value, type);
            }
        }
        #endregion

        #region Show Date and ImpressionType

        private void ShowDateAndImpressionType(Expression<Func<DbAll, bool>> value, string type)
        {
            var data = from p in dc.DbAll select p;
            string logic = dateImpressionViewClass.GetLogicValue();
            DateTime? StartDate = dateImpressionViewClass.GetStartDateValue();
            DateTime? EndDate = dateImpressionViewClass.GetEndDateValue();
            var inner = PredicateBuilder.False<DbAll>();
            var outer = PredicateBuilder.True<DbAll>();
            switch (logic)
            {
                case "both":
                    outer = outer.And(m => m.xDate >= StartDate && m.xDate <= EndDate);
                    outer = outer.And(value);
                    ImpressionTypeAdd(outer, type);
                    break;
                case "start":
                    outer = outer.And(m => m.xDate >= StartDate);
                    outer = outer.And(value);
                    ImpressionTypeAdd(outer, type);
                    break;
                case "end":
                    outer = outer.And(m => m.xDate <= EndDate);
                    outer = outer.And(value);
                    ImpressionTypeAdd(outer, type);
                    break;
                case "none":
                    ImpressionTypeAdd(value, type);
                    break;
                default:
                    ImpressionTypeAdd(value, type);
                    break;
            }
        }

        private void ImpressionTypeAdd(Expression<Func<DbAll, bool>> value, string type)
        {
            string iType = dateImpressionViewClass.GetImpressionTypeValue();
            var inner = PredicateBuilder.False<DbAll>();
            var outer = PredicateBuilder.True<DbAll>();
            switch (iType)
            {
                case "both":
                    dataTableCreate(value, type);
                    break;
                case "PC":
                    outer = outer.And(m => m.impressionType == "pc");
                    outer = outer.And(value);
                    dataTableCreate(outer, type);
                    break;
                case "MOBILE":
                    outer = outer.And(m => m.impressionType == "mobile");
                    outer = outer.And(value);
                    dataTableCreate(outer, type);
                    break;
                default:
                    dataTableCreate(value, type);
                    break;
            }
        }

        #endregion

        public DataTable ToDataTable(System.Data.Linq.DataContext ctx, object query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            IDbCommand cmd = ctx.GetCommand(query as IQueryable);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = (SqlCommand)cmd;
            DataTable dt = new DataTable("sd");

            try
            {
                cmd.Connection.Open();
                adapter.FillSchema(dt, SchemaType.Source);
                adapter.Fill(dt);
            }
            finally
            {
                cmd.Connection.Close();
            }
            return dt;
        }

        private void dataTableCreate(Expression<Func<DbAll, bool>> value, string type)
        {
            var data = dc.DbAll.Where(value);
            DataTable dataTableSource = ToDataTable(dc, data);
            if (dc.DatabaseExists())
            {
                DataGrid.DataContext = null;
                DataGrid.DataContext = dataTableSource.DefaultView;
            }
            if(type == "save")
            {
                XLWorkbook workbook = new XLWorkbook();
                var sourceTableSheet = workbook.Worksheets.Add("DataSource");
                var SourceTable = sourceTableSheet.Cell(1, 1).InsertTable(dataTableSource, "DataSource",true);
                var pivotTableSheet = workbook.Worksheets.Add("PivotTable");
                var PivotTable = pivotTableSheet.PivotTables.AddNew("PivotTable", pivotTableSheet.Cell(1, 1), SourceTable.AsRange());
                PivotTable.RowLabels.Add("brand");
                PivotTable.RowLabels.Add("brandOwner");
                PivotTable.RowLabels.Add("secondaryBrand");
                PivotTable.RowLabels.Add("secondaryBrandOwner");
                PivotTable.RowLabels.Add("product");
                PivotTable.RowLabels.Add("industry");
                PivotTable.RowLabels.Add("subIndustry");
                PivotTable.RowLabels.Add("subIndustry2");
                PivotTable.RowLabels.Add("subIndustry3");
                PivotTable.RowLabels.Add("subIndustry4");
                PivotTable.RowLabels.Add("publisher");
                PivotTable.RowLabels.Add("website");
                PivotTable.RowLabels.Add("subWebsite");
                PivotTable.RowLabels.Add("contentType");
                PivotTable.RowLabels.Add("adType");
                PivotTable.RowLabels.Add("adTypeGroup");
                PivotTable.RowLabels.Add("isAutopromotion");
                PivotTable.RowLabels.Add("adUrl");
                PivotTable.RowLabels.Add("adSize");
                PivotTable.RowLabels.Add("impressionType");
                PivotTable.RowLabels.Add("xYear");
                PivotTable.RowLabels.Add("xMonth");
                PivotTable.RowLabels.Add("xWeek");
                PivotTable.RowLabels.Add("xDate");
                PivotTable.Values.Add("impressions");
                PivotTable.Values.Add("cost");
                PivotTable.Values.Add("cost_gross");
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Test.xlsx";
                workbook.SaveAs(path);
            }
        }
    }
}
