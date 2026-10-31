using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace Dental.Views;

public partial class MainWindow : ReactiveWindow<MainViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
       
        this.WhenActivated(d =>
        {
            d(this.OneWayBind(ViewModel, vm => vm.DoctorListViewModel, view => view.contentDoctors.Content));
            d(this.OneWayBind(ViewModel, vm => vm.PatientListViewModel, view => view.contentPatients.Content));
            d(this.OneWayBind(ViewModel,vm => vm.VisitListViewModel,view => view.contentVisits.Content));
            d(this.OneWayBind(ViewModel, vm => vm.InterventionListViewModel, view => view.contentInterventions.Content));
        });
    }
}