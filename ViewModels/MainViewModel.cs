using System.Threading.Tasks;

namespace Dental.ViewModels;

public class MainViewModel : ViewModelBase
{
    public DoctorListViewModel DoctorListViewModel { get; }
    public PatientListViewModel PatientListViewModel { get; }
    public VisitListViewModel VisitListViewModel { get; }
    public InterventionListViewModel InterventionListViewModel { get; }

    public MainViewModel()
    {
        DoctorListViewModel = new DoctorListViewModel();
        PatientListViewModel = new PatientListViewModel();
        VisitListViewModel = new VisitListViewModel(DoctorListViewModel,PatientListViewModel);
        InterventionListViewModel = new InterventionListViewModel(VisitListViewModel);
    }

    #region Data load

    public override async Task LoadAsync()
    {
        await Task.WhenAll(DoctorListViewModel.LoadAsync(),
                           PatientListViewModel.LoadAsync(),
                           VisitListViewModel.LoadAsync(),
                           InterventionListViewModel.LoadAsync());
    }

    public override async Task SaveAsync()
    {
        await Task.WhenAll(DoctorListViewModel.SaveAsync(),
                           PatientListViewModel.SaveAsync(),
                           VisitListViewModel.SaveAsync(),
                           InterventionListViewModel.SaveAsync());
    }

    #endregion 
}
