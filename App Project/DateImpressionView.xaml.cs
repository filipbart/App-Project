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
using App_Project.Helper_Classes;

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
        }

        private void SetPickerDates()
        {
            var MinDate = Dates.MinDate;
            var MaxDate = Dates.MaxDate;
            StartDate.DisplayDate = MinDate;
            StartDate.DisplayDateStart = MinDate;
            StartDate.DisplayDateEnd = MaxDate;
            EndDate.DisplayDate = MaxDate;
            EndDate.DisplayDateStart = MinDate;
            EndDate.DisplayDateEnd = MaxDate;
            generateStringDates(MinDate, MaxDate);
        }

        

        private void generateStringDates(DateTime minDate, DateTime maxDate)
        {
            var MinDate = minDate.ToString("dd/MM/yyyy");
            var MaxDate = maxDate.ToString("dd/MM/yyyy");
            string Dates = MinDate + " - " + MaxDate;
            TextBlockDates.Text = Dates;
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = StartDate.SelectedDate;
            if (date > EndDate.SelectedDate && EndDate.SelectedDate != null)
            {
                MessageBox.Show("Start date cannot be after the end date");
                StartDate.SelectedDate = null;
            }
            else
            {
                SetStartDateValue(date);
                LogicDates();
            }
        }


        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = EndDate.SelectedDate;
            if (date < StartDate.SelectedDate && StartDate.SelectedDate != null)
            {
                MessageBox.Show("End date cannot be before the start date");
                EndDate.SelectedDate = null;
            }
            else
            {
                SetEndDateValue(date);
                LogicDates();
            }
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

