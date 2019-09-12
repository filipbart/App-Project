using App_Project.Helper_Classes;
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
    /// Logika interakcji dla klasy BrandView.xaml
    /// </summary>
    /// 
    public partial class BrandView : UserControl, INotifyPropertyChanged
    {

        LINQtoSQLDataContext dc = new LINQtoSQLDataContext(
           Properties.Settings.Default.ProjectBaseConnectionString);

        public static List<ChosenBrands> chosenBrands = new List<ChosenBrands>();
        private static List<ShowChosenBrands> showBrandsList = new List<ShowChosenBrands>();

        public BrandView()
        {
            InitializeComponent();
            ShowBrandsList();
            ShowBrandsOwnerList();
            DataContext = this;
        }

        #region for TextBox

        private string _brandName;
        private string _brandNameOwner;
        public event PropertyChangedEventHandler PropertyChanged;

        public string BrandName
        {
            get
            {
                return _brandName;
            }
            set
            {
                _brandName = value;
                OnPropertyChanged("BrandName");
            }
        }

        public string BrandNameOwner
        {
            get
            {
                return _brandNameOwner;
            }
            set
            {
                _brandNameOwner = value;
                OnPropertyChanged("BrandNameOwner");
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region ShowList

        private void ShowBrandsList()
        {
            if (BrandName == null)
            {
                var data =
                    from br in dc.Brand
                    from brO in dc.BrandOwner
                    where br.bOwner_id == brO.BrandOwner_id
                    select br.brandName;
                if (dc.DatabaseExists()) BrandListBox.ItemsSource = data.ToList();
            }
            else
            {
                var data =
                    from br in dc.Brand
                    from brO in dc.BrandOwner
                    where br.bOwner_id == brO.BrandOwner_id && br.brandName.Contains(BrandName)
                    select br.brandName;
                if (dc.DatabaseExists()) BrandListBox.ItemsSource = data.ToList();
            }
        }

        private void ShowBrandsOwnerList()
        {
            if (BrandNameOwner == null)
            {
                var data =
                    from brO in dc.BrandOwner
                    select brO.brandOwner;
                if (dc.DatabaseExists()) BrandOwnerListBox.ItemsSource = data.ToList();
            }
            else
            {
                var data =
                    from brO in dc.BrandOwner
                    where brO.brandOwner.Contains(BrandNameOwner)
                    select brO.brandOwner;
                if (dc.DatabaseExists()) BrandOwnerListBox.ItemsSource = data.ToList();
            }
        }


        #endregion

        private void Brands_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowBrandsList();
        }

        private void BrandsOwner_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowBrandsOwnerList();
        }


        #region Buttons Function
        public void Brand_Add(object sender, RoutedEventArgs e)
        {
            if(BrandListBox.SelectedIndex >= 0)
            {
                string BrandName = BrandListBox.SelectedItem.ToString();
                var data =
                   from br in dc.Brand
                   from brO in dc.BrandOwner
                   where br.bOwner_id == brO.BrandOwner_id && br.brandName.Contains(BrandName)
                   select brO.brandOwner;
                string BrandOwnerName = string.Join(",",data);
                chosenBrands.Add(new ChosenBrands() { Brand = BrandName, BrandOwner = null });
                showBrandsList.Add(new ShowChosenBrands() { Brand = BrandName, BrandOwner = BrandOwnerName });
                ChosenBrandItems.ItemsSource = null;
                ChosenBrandItems.ItemsSource = showBrandsList;
            }
        }

        public void BrandOwner_Add(object sender, RoutedEventArgs e)
        {
            if (BrandOwnerListBox.SelectedIndex >= 0)
            {
                string BrandName = null;
                string BrandOwnerName = BrandOwnerListBox.SelectedItem.ToString();
                chosenBrands.Add(new ChosenBrands() { Brand = BrandName, BrandOwner = BrandOwnerName });
                showBrandsList.Add(new ShowChosenBrands() { Brand = BrandName, BrandOwner = BrandOwnerName });
                ChosenBrandItems.ItemsSource = null;
                ChosenBrandItems.ItemsSource = showBrandsList;
            }
        }

        public void Remove(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ChosenBrandItems.SelectedIndex;
            if (selectedIndex != -1)
            {
                chosenBrands.RemoveAt(selectedIndex);
                showBrandsList.RemoveAt(selectedIndex);
                ChosenBrandItems.ItemsSource = null;
                ChosenBrandItems.ItemsSource = showBrandsList;
            }
        }

        #endregion


        public List<ChosenBrands> BrandsList
        {
            get { return chosenBrands; }
        }
    }
}
