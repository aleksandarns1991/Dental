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
    public partial class InterventionListView : ReactiveUserControl<InterventionListViewModel>
    {
        public InterventionListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(Interactions.InterventionInteraction.RegisterHandler(async interaction =>
                {
                    var topLevel = TopLevel.GetTopLevel(this) as Window;

                    var win = new InterventionWindow();
                    win.DataContext = interaction.Input;

                    var output = await win.ShowDialog<Intervention>(topLevel!);
                    interaction.SetOutput(output);
                }));

                d(this.Bind(ViewModel, vm => vm.SearchText, view => view.textSearch.Text));

                d(this.OneWayBind(ViewModel, vm => vm.FilteredInterventions, view => view.listInterventions.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedIntervention, view => view.listInterventions.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.AddInterventionCmd, view => view.btnAdd));
                d(this.BindCommand(ViewModel, vm => vm.EditInterventionCmd, view => view.btnEdit)); 
                d(this.BindCommand(ViewModel,vm => vm.DeleteInterventionCmd, view => view.btnDelete));

                listInterventions.ItemTemplate = null;
                listInterventions.DisplayMemberBinding = new Binding("Title");
            });
        }
    }
}