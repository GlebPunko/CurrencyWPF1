using CurrencyWPF.CustomNavigation;
using CurrencyWPF.Interfaces;
using CurrencyWPF.Services.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace CurrencyWPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public static readonly string HomeViewModelAlias = "HomeVM";
        public static readonly string DayCurrencyViewModelAlias = "DayCurrency2VM";
        public static readonly string IntervalCurrencyViewModelAlias = "IntervalCurrencyVM";
        public static readonly string NotFoundViewModelAlias = "NotFoundVM";

        private readonly IViewModelsResolver _resolver;

        private ICommand _goToPathCommand;

        private ICommand _goToHomeCommand;

        private ICommand _goToDayCurrencyCommand;

        private ICommand _goToIntervalCurrencyCommand;

        private readonly INotifyPropertyChanged _homeViewModel;
        private readonly INotifyPropertyChanged _dayCurrencyViewModel;
        private readonly INotifyPropertyChanged _intervalCurrencyViewModel;

        public ICommand GoToPathCommand
        {
            get { return _goToPathCommand; }
            set
            {
                _goToPathCommand = value;
                RaisePropertyChanged("GoToPathCommand");
            }
        }

        public ICommand GoToPage1Command
        {
            get
            {
                return _goToHomeCommand;
            }
            set
            {
                _goToHomeCommand = value;
                RaisePropertyChanged("GoToPage1Command");
            }
        }

        public ICommand GoToPage2Command
        {
            get { return _goToDayCurrencyCommand; }
            set
            {
                _goToDayCurrencyCommand = value;
                RaisePropertyChanged("GoToPage2Command");
            }
        }

        public ICommand GoToPage3Command
        {
            get { return _goToIntervalCurrencyCommand; }
            set
            {
                _goToIntervalCurrencyCommand = value;
                RaisePropertyChanged("GoToPage3Command");
            }
        }

        public INotifyPropertyChanged Page1ViewModel
        {
            get { return _homeViewModel; }
        }

        public INotifyPropertyChanged Page2ViewModel
        {
            get { return _dayCurrencyViewModel; }
        }

        public INotifyPropertyChanged Page3ViewModel
        {
            get { return _intervalCurrencyViewModel; }
        }

        public MainViewModel(IViewModelsResolver resolver)
        {
            _resolver = resolver;

            _homeViewModel = _resolver.GetViewModelInstance(HomeViewModelAlias);
            _dayCurrencyViewModel = _resolver.GetViewModelInstance(DayCurrencyViewModelAlias);
            _intervalCurrencyViewModel = _resolver.GetViewModelInstance(IntervalCurrencyViewModelAlias);

            InitializeCommands();
        }


        private void InitializeCommands()
        {

            GoToPathCommand = new RelayCommand<string>(GoToPathCommandExecute);

            GoToPage1Command = new RelayCommand<INotifyPropertyChanged>(GoToHomeCommandExecute);

            GoToPage2Command = new RelayCommand<INotifyPropertyChanged>(GoToDayCurrencyCommandExecute);

            GoToPage3Command = new RelayCommand<INotifyPropertyChanged>(GoToIntervalCurrencyCommandExecute);
        }

        private void GoToPathCommandExecute(string alias)
        {
            if (string.IsNullOrWhiteSpace(alias))
            {
                return;
            }

            Navigation.Navigate(alias);
        }

        private void GoToHomeCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.HomeAlias, Page1ViewModel);
        }

        private void GoToDayCurrencyCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.DayCurrencyAlias, Page1ViewModel);
        }

        private void GoToIntervalCurrencyCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.IntervalCurrencyAlias, Page1ViewModel);
        }
    }
}
