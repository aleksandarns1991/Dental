using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dental.Services
{
    public interface IDataPersistence<T> where T : class
    {
        Task LoadAsync(ICollection<T> collection);
        Task SaveAsync(ICollection<T> collection);
    }
}
