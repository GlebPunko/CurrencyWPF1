using CurrencyWPF.CustomNavigation;
using CurrencyWPF.ViewModels;
using System.Windows;

namespace CurrencyWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Navigation.Service = MainFrame.NavigationService;

            DataContext = new MainViewModel(new ViewModelsResolver());
        }
    }
}
