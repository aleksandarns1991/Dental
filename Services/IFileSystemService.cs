using Avalonia.Controls;
using System.Threading.Tasks;

namespace Dental.Services
{
    public interface IFileSystemService<T> where T : class
    {
        string? Title { get; set; }
        Task<T> ShowAsync(Window parent);
    }
}
