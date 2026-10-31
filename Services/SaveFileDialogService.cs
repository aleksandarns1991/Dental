using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dental.Services
{
    public class SaveFileDialogService : IFileSystemService<string>
    {
        public string? Title { get; set; }
        public string? Extensions { get; set; }

        public SaveFileDialogService()
        {
            Title = "Open file";
            Extensions = "All files:*.*";
        }

        public async Task<string> ShowAsync(Window parent)
        {
            var topLevel = TopLevel.GetTopLevel(parent);

            var saveFile = await topLevel!.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = Title,
                FileTypeChoices = GetFilePickerFileTypes()
            });

            return saveFile?.TryGetLocalPath()!;  
        }

        private IReadOnlyList<FilePickerFileType> GetFilePickerFileTypes()
        {
            var fileTypes = new List<FilePickerFileType>();
            var extensions = Extensions!.Split('|');

            foreach (var extension in extensions)
            {
                var arr = extension.Split(':');
                var name = arr[0];
                var fileType = arr[1].Split(',');

                fileTypes.Add(new FilePickerFileType(name)
                {
                    Patterns = fileType
                });
            }

            return fileTypes;
        }
    }
}
