using CurrencyWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CurrencyWPF.ViewModels
{
    public class ViewModelsResolver : IViewModelsResolver
    {

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vmResolvers = new Dictionary<string, Func<INotifyPropertyChanged>>();

        public ViewModelsResolver()
        {
            _vmResolvers.Add(MainViewModel.HomeViewModelAlias, () => new HomeViewModel());
            _vmResolvers.Add(MainViewModel.DayCurrencyViewModelAlias, () => new DayCurrencyViewModel());
            _vmResolvers.Add(MainViewModel.IntervalCurrencyViewModelAlias, () => new IntervalCurrencuViewModel());
            _vmResolvers.Add(MainViewModel.NotFoundViewModelAlias, () => new NotFoundViewModel());
        }

        public INotifyPropertyChanged GetViewModelInstance(string alias)
        {
            if (_vmResolvers.ContainsKey(alias))
            {
                return _vmResolvers[alias]();
            }

            return _vmResolvers[MainViewModel.NotFoundViewModelAlias]();
        }
    }
}
