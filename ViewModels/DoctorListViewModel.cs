using Dental.Models;
using Dental.Services;
using Dental.Utility;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dental.ViewModels
{
    public class DoctorListViewModel : ViewModelBase
    {
        public ObservableCollection<Doctor> Doctors { get; } = new();

        [Reactive]
        public string? SearchText { get; set; } = string.Empty;
        [Reactive]
        public Doctor? SelectedDoctor { get; set; }

        private readonly ObservableAsPropertyHelper<IEnumerable<Doctor>> filteredDoctors;
        public IEnumerable<Doctor> FilteredDoctors => filteredDoctors.Value; 

        public ReactiveCommand<Unit, Unit>? AddDoctorCmd { get; }
        public ReactiveCommand<Unit, Unit>? EditDoctorCmd { get; }
        public ReactiveCommand<Unit, Unit>? DeleteDoctorCmd { get; }

        private readonly IDataPersistence<Doctor> doctorPersistence;

        public DoctorListViewModel(IDataPersistence<Doctor>? doctorPersistence = null)
        {
            this.doctorPersistence = doctorPersistence ?? Locator.Current.GetService<IDataPersistence<Doctor>>()!;

            filteredDoctors = this.WhenAnyValue(x => x.Doctors, x => x.SearchText, x => x.Doctors.Count, (doctors, text, _) =>
                                               doctors.Where(d => d.FullName!.StartsWith(text!, System.StringComparison.OrdinalIgnoreCase)))
                                               .ToProperty(this, vm => vm.FilteredDoctors);
                                               
            var canHandleDoctor = this.WhenAnyValue(x => x.SelectedDoctor).Select(x => x != null);

            AddDoctorCmd = ReactiveCommand.CreateFromTask(AddDoctorAsync); 
            EditDoctorCmd = ReactiveCommand.CreateFromTask(EditDoctorAsync, canHandleDoctor);   
            DeleteDoctorCmd = ReactiveCommand.CreateFromTask(DeleteDoctorAsync, canHandleDoctor);   
        }

        #region Data loader

        public override async Task LoadAsync()
        {
            await doctorPersistence.LoadAsync(Doctors);
        }

        public override async Task SaveAsync()
        {
            await doctorPersistence.SaveAsync(Doctors);
        }

        #endregion

        #region Methods 

        private async Task AddDoctorAsync()
        {
            var vm = new DoctorViewModel();
            var doctor = await Interactions.DoctorInteraction.Handle(vm); 

            if (doctor != null)
            {
                Doctors!.Add(doctor);
                SelectedDoctor = doctor;
            }
        }

        private async Task EditDoctorAsync()
        {
            if (SelectedDoctor == null)
            {
                return;
            }

            var vm = new DoctorViewModel(SelectedDoctor);
            var doctor = await Interactions.DoctorInteraction.Handle(vm);

            if (doctor != null)
            {
                SelectedDoctor.FirstName = doctor.FirstName;
                SelectedDoctor.LastName = doctor.LastName;
                SelectedDoctor.Specialty = doctor.Specialty;
                SelectedDoctor.Birthday = doctor.Birthday;
                SelectedDoctor.ImagePath = doctor.ImagePath;
            }
        }

        private async Task DeleteDoctorAsync()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Question", "Do you want to remove this doctor?", MsBox.Avalonia.Enums.ButtonEnum.YesNo, Icon.Info);
            var result = await box.ShowAsync();

            if (SelectedDoctor != null && result == ButtonResult.Yes)
            {
                Doctors!.Remove(SelectedDoctor);
            }
        }

        #endregion
    }
}