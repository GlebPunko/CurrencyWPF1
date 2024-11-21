using System.ComponentModel;

namespace CurrencyWPF.Interfaces
{
    public interface IViewModelsResolver
    {
        INotifyPropertyChanged GetViewModelInstance(string alias);
    }
}
