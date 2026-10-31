using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Dental.Models;
using Dental.Utility;
using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Dental.Views
{
    public partial class DoctorListView : ReactiveUserControl<DoctorListViewModel>
    {
        public DoctorListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(Interactions.DoctorInteraction.RegisterHandler(async interaction =>
                {
                    var topLevel = TopLevel.GetTopLevel(this) as Window;

                    var win = new DoctorWindow();
                    win.DataContext = interaction.Input;

                    var output = await win.ShowDialog<Doctor>(topLevel!);
                    interaction.SetOutput(output);
                }));

                d(this.Bind(ViewModel, vm => vm.SearchText, view => view.textSearch.Text));

                d(this.OneWayBind(ViewModel, vm => vm.FilteredDoctors, view => view.listDoctors.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedDoctor, view => view.listDoctors.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.AddDoctorCmd, view => view.btnAdd));
                d(this.BindCommand(ViewModel,vm => vm.EditDoctorCmd, view => view.btnEdit));
                d(this.BindCommand(ViewModel,vm => vm.DeleteDoctorCmd,view => view.btnDelete));

                listDoctors.ItemTemplate = null;
                listDoctors.DisplayMemberBinding = new Binding("FullName");
            });
        }
    }
}