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

        public ICommand GoToHomeCommand
        {
            get
            {
                return _goToHomeCommand;
            }
            set
            {
                _goToHomeCommand = value;
                RaisePropertyChanged("GoToHomeCommand");
            }
        }

        public ICommand GoToDayCurrencyCommand
        {
            get { return _goToDayCurrencyCommand; }
            set
            {
                _goToDayCurrencyCommand = value;
                RaisePropertyChanged("GoToDayCurrencyCommand");
            }
        }

        public ICommand GoToIntervalCurrencyCommand
        {
            get { return _goToIntervalCurrencyCommand; }
            set
            {
                _goToIntervalCurrencyCommand = value;
                RaisePropertyChanged("GoToIntervalCurrencyCommand");
            }
        }

        public INotifyPropertyChanged HomeViewModel
        {
            get { return _homeViewModel; }
        }

        public INotifyPropertyChanged DayCurrencyViewModel
        {
            get { return _dayCurrencyViewModel; }
        }

        public INotifyPropertyChanged IntervalCurrencyViewModel
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

            GoToHomeCommand = new RelayCommand<INotifyPropertyChanged>(GoToHomeCommandExecute);

            GoToDayCurrencyCommand = new RelayCommand<INotifyPropertyChanged>(GoToDayCurrencyCommandExecute);

            GoToIntervalCurrencyCommand = new RelayCommand<INotifyPropertyChanged>(GoToIntervalCurrencyCommandExecute);
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
            Navigation.Navigate(Navigation.HomeAlias, HomeViewModel);
        }

        private void GoToDayCurrencyCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.DayCurrencyAlias, DayCurrencyViewModel);
        }

        private void GoToIntervalCurrencyCommandExecute(INotifyPropertyChanged viewModel)
        {
            Navigation.Navigate(Navigation.IntervalCurrencyAlias, IntervalCurrencyViewModel);
        }
    }
}
