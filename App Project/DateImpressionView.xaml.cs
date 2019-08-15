using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static DateTime? startDateValue;
        private static DateTime? endDateValue;
        private static string logicValue = "none";
        private static string impressionTypeValue = "PC";

        public DateImpressionView()
        {
            InitializeComponent();
            SetPickerDates();
            generateStringDates();
        }

        private void SetPickerDates()
        {
            var MinDate = (from d in dc.db_main select d.xDate.Value.Date).Min();
            var MaxDate = (from d in dc.db_main select d.xDate.Value.Date).Max();
            StartDate.DisplayDate = MinDate;
            StartDate.DisplayDateStart = MinDate;
            StartDate.DisplayDateEnd = MaxDate;
            EndDate.DisplayDate = MaxDate;
            EndDate.DisplayDateStart = MinDate;
            EndDate.DisplayDateEnd = MaxDate;
        }

        

        private void generateStringDates()
        {
            var MinDate = (from d in dc.db_main select d.xDate.Value.Date).Min().ToString("dd/MM/yyyy");
            var MaxDate = (from d in dc.db_main select d.xDate.Value.Date).Max().ToString("dd/MM/yyyy");
            string Dates = MinDate + " - " + MaxDate;
            TextBlockDates.Text = Dates;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = StartDate.SelectedDate;
            SetStartDateValue(date);
            LogicDates();
        }


        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = EndDate.SelectedDate;
            SetEndDateValue(date);
            LogicDates();
        }

        public void LogicDates()
        {
            if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                string logic = "both";
                SetLogicValue(logic);
                ImpressionType();
            }
            else if (StartDate.SelectedDate.HasValue && !EndDate.SelectedDate.HasValue)
            {
                string logic = "start";

                SetLogicValue(logic);
                ImpressionType();
            }
            else if (!StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                string logic = "end";
                SetLogicValue(logic);
                ImpressionType();
            }
            else
            {
                string logic = "none";
                SetLogicValue(logic);
                ImpressionType();
            }
        }

        public void ImpressionType()
        {
            if (ImpressionTypeGrid.Visibility.ToString() == "Visible")
            {
                if ((PC.IsChecked == true) && (MOBILE.IsChecked == true))
                {
                    string type = "both";
                    SetImpressionTypeValue(type);
                }
                else if ((PC.IsChecked == true) && (MOBILE.IsChecked == false))
                {
                    string type = "PC";
                    SetImpressionTypeValue(type);
                }
                else if ((PC.IsChecked == false) && (MOBILE.IsChecked == true))
                {
                    string type = "MOBILE";
                    SetImpressionTypeValue(type);
                }
            }
        }

        #region Set Values
        public void SetStartDateValue(DateTime? value)
        {
            startDateValue = value;
        }

        public void SetEndDateValue(DateTime? value)
        {
            endDateValue = value;
        }

        public void SetLogicValue(string value)
        {
            logicValue = value;
        }

        public void SetImpressionTypeValue(string value)
        {
            impressionTypeValue = value;
        }

        #endregion

        #region Get Values
        public DateTime? GetStartDateValue()
        {
            return startDateValue;
        }

        public DateTime? GetEndDateValue()
        {
            return endDateValue;
        }

        public string GetLogicValue()
        {
            return logicValue;
        }

        public string GetImpressionTypeValue()
        {
            return impressionTypeValue;
        }

        #endregion

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox type = sender as CheckBox;
            switch (type.Name.ToString())
            {
                case "PC":
                    if(MOBILE.IsChecked == false)
                        PC.IsChecked = true;
                    break;
                case "MOBILE":
                    if (PC.IsChecked == false)
                        MOBILE.IsChecked = true;
                    break;
                default:
                    break;
            }

        }

        private void Checkbox_Click(object sender, RoutedEventArgs e)
        {
            ImpressionType();
        }
    }
}

