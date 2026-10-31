using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dental.Services
{
    public class OpenFileDialogService : IFileSystemService<string[]>
    {
        public string? Title { get; set; }
        public string? Extensions { get; set; }
        public bool AllowMultiple { get; set; }

        public OpenFileDialogService()
        {
            Title = "Open file";
            Extensions = "All files:*.*";
            AllowMultiple = false;
        }

        public async Task<string[]> ShowAsync(Window parent)
        {
            var topLevel = TopLevel.GetTopLevel(parent);

            var openFile = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = Title,
                AllowMultiple = AllowMultiple,
                FileTypeFilter = GetFilePickerFileTypes()
            });

            var files = new List<string>();

            foreach (var file in openFile)
            {
                var filePath = file.TryGetLocalPath()!;
                files.Add(filePath);
            }

            return files.ToArray();
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