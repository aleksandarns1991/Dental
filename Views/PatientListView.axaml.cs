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
    public partial class PatientListView : ReactiveUserControl<PatientListViewModel>
    {
        public PatientListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(Interactions.PatientInteraction.RegisterHandler(async interaction =>
                {
                    var topLevel = TopLevel.GetTopLevel(this) as Window;

                    var win = new PatientWindow();
                    win.DataContext = interaction.Input;

                    var output = await win.ShowDialog<Patient>(topLevel!);
                    interaction.SetOutput(output);
                }));

                d(this.Bind(ViewModel, vm => vm.SearchText, view => view.textSearch.Text));

                d(this.OneWayBind(ViewModel, vm => vm.FilteredPatients, view => view.listPatients.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedPatient, view => view.listPatients.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.AddPatientCmd, view => view.btnAdd));
                d(this.BindCommand(ViewModel, vm => vm.EditPatientCmd, view => view.btnEdit));
                d(this.BindCommand(ViewModel, vm => vm.DeletePatientCmd, view => view.btnDelete));

                listPatients.ItemTemplate = null;
                listPatients.DisplayMemberBinding = new Binding("FullName");
            });
        }
    }
}