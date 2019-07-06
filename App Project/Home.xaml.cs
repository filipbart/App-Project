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

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
            Properties.Settings.Default.ProjectBaseConnectionString);
    
        public Home()
        {
            InitializeComponent();

            

            //string connection = "SERVER=(LocalDb)\Pivot Creator;DATABASE=P ";
        }

        private void ViewButton_Click(object sender, RoutedEventArgs e)
        {
            var data =
                from brand in dc.Brand
                from brandO in dc.BrandOwner
                where brand.brandName == "Audi" && brand.bOwner_id == brandO.BrandOwner_id
                select new
                {
                    brand.brandName,
                    brandO.brandOwner
                };
            if (dc.DatabaseExists()) DataGrid.ItemsSource = data.ToList();
        }
    }
}
