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
    public partial class VisitListView : ReactiveUserControl<VisitListViewModel>
    {
        public VisitListView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(Interactions.VisitInteraction.RegisterHandler(async interaction =>
                {
                    var topLevel = TopLevel.GetTopLevel(this) as Window;

                    var win = new VisitWindow();
                    win.DataContext = interaction.Input;

                    var output = await win.ShowDialog<Visit>(topLevel!);
                    interaction.SetOutput(output);
                }));

                d(this.Bind(ViewModel, vm => vm.Scheduled, view => view.calendarSchedule.SelectedDate));

                d(this.OneWayBind(ViewModel, vm => vm.FilteredVisits, view => view.listVisits.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedVisit, view => view.listVisits.SelectedItem));

                d(this.BindCommand(ViewModel, vm => vm.AddVisitCmd, view => view.btnAdd));
                d(this.BindCommand(ViewModel, vm => vm.EditVisitCmd, view => view.btnEdit)); 
                d(this.BindCommand(ViewModel,vm => vm.DeleteVisitCmd,view => view.btnDelete));

                listVisits.ItemTemplate = null;
                listVisits.DisplayMemberBinding = new Binding("Scheduled");
            });
        }
    }
}