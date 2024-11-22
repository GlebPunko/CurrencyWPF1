using CurrencyWPF.Interfaces;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CurrencyWPF.CustomNavigation
{
    public class Navigation
    {
        public static readonly string HomeAlias = "Home";
        public static readonly string DayCurrencyAlias = "DayCurrency";
        public static readonly string IntervalCurrencyAlias = "IntervalCurrency";
        public static readonly string NotFoundAlias = "NotFound";

        private NavigationService _navService;
        private readonly IPageResolver _resolver;

        public static NavigationService Service
        {
            get { return Instance._navService; }
            set
            {
                if (Instance._navService != null)
                {
                    Instance._navService.Navigated -= Instance.NavService_Navigated;
                }

                Instance._navService = value;
                Instance._navService.Navigated += Instance.NavService_Navigated;
            }
        }

        public static void Navigate(Page page, object context)
        {
            if (Instance._navService == null || page == null)
            {
                return;
            }

            Instance._navService.Navigate(page, context);
        }

        public static void Navigate(Page page)
        {
            Navigate(page, null);
        }

        public static void Navigate(string uri, object context)
        {
            if (Instance._navService == null || uri == null)
            {
                return;
            }

            var page = Instance._resolver.GetPageInstance(uri);

            Navigate(page, context);
        }

        public static void Navigate(string uri)
        {
            Navigate(uri, null);
        }

        void NavService_Navigated(object sender, NavigationEventArgs e)
        {
            var page = e.Content as Page;

            if (page == null)
            {
                return;
            }

            page.DataContext = e.ExtraData;
        }


        private static volatile Navigation _instance;
        private static readonly object SyncRoot = new Object();

        private Navigation()
        {
            _resolver = new PagesResolver();
        }

        private static Navigation Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new Navigation();
                    }
                }

                return _instance;
            }
        }
    }
}
