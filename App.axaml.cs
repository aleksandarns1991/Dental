
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dental.Utility;
using Dental.ViewModels;
using Dental.Views;

namespace Dental;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _ = new AppBootStrapper();

            var vm = new MainViewModel();
            _ = vm.LoadAsync();

            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };

            desktop.Exit += async (_, __) => await vm.SaveAsync();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
