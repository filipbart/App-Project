/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace App_Project
{
    public class ViewChanger : ObservableObject
    {
        private ICommand _changePageCommand;

        private IPageViewModel _currentPage;
        private List<IPageViewModel> _pageViewModels;
        public ViewChanger()
        {
            //PageViewModels.Add(new HomeView());
            //PageViewModels.Add(new IndustryView());

            CurrentPage = PageViewModels[0];
        }


        public ICommand ChangePageCommand
        {
            get
            {
                if(_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }

                return _changePageCommand;
            }
        }
        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if(_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged("CurrentPage");
                }
            }
        }

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPage = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}*/
