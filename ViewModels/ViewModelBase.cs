using ReactiveUI;
using System.Threading.Tasks;

namespace Dental.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public abstract Task LoadAsync();
    public abstract Task SaveAsync();
}