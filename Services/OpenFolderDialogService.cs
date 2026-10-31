using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace Dental.Services
{
    public class OpenFolderDialogService : IFileSystemService<string>
    {
        public string? Title { get; set; }

        public async Task<string> ShowAsync(Window parent)
        {
            var topLevel = TopLevel.GetTopLevel(parent);

            var openFolder = await topLevel!.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = Title
            });

            return openFolder.Count > 0 ? openFolder[0].TryGetLocalPath()! : string.Empty;
        }
    }
}
