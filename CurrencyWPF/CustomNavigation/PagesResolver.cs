using CurrencyWPF.Interfaces;
using CurrencyWPF.Views;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace CurrencyWPF.CustomNavigation
{
    public class PagesResolver : IPageResolver
    {

        private readonly Dictionary<string, Func<Page>> _pagesResolvers = new Dictionary<string, Func<Page>>();

        public PagesResolver()
        {
            _pagesResolvers.Add(Navigation.HomeAlias, () => new HomeView());
            _pagesResolvers.Add(Navigation.DayCurrencyAlias, () => new DayCurrencyView());
            _pagesResolvers.Add(Navigation.IntervalCurrencyAlias, () => new IntervalCurrencyView());
            _pagesResolvers.Add(Navigation.NotFoundAlias, () => new NotFoundView());
        }

        public Page GetPageInstance(string alias)
        {
            if (_pagesResolvers.ContainsKey(alias))
            {
                return _pagesResolvers[alias]();
            }

            return _pagesResolvers[Navigation.NotFoundAlias]();
        }
    }
}
