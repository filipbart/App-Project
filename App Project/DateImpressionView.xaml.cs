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
    /// Logika interakcji dla klasy DataImpressionView.xaml
    /// </summary>
    public partial class DateImpressionView : UserControl
    {

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
           Properties.Settings.Default.ProjectBaseConnectionString);
        public DateImpressionView()
        {
            InitializeComponent();
            DatePickerDates();
            generateStringDates();
        }

        private void DatePickerDates()
        {
            var MinDate = (from d in dc.db_main select d.xDate).Min();
            var MaxDate = (from d in dc.db_main select d.xDate).Max();
            StartDate.DisplayDateStart = MinDate;
            StartDate.DisplayDateEnd = MaxDate;
            EndDate.DisplayDateStart = MinDate;
            EndDate.DisplayDateEnd = MaxDate;
        }

        private void generateStringDates()
        {
            var MinDate = (from d in dc.db_main select d.xDate).Min().ToString();
            var MaxDate = (from d in dc.db_main select d.xDate).Max().ToString();
            string Dates = MinDate + " - " + MaxDate;
            TextBlockDates.Text = Dates;
        }
        
    }
}

