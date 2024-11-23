using System.Windows.Controls;

namespace CurrencyWPF.Interfaces
{
    public interface IPageResolver
    {
        Page GetPageInstance(string alias);
    }
}
