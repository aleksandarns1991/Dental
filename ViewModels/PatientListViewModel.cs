using Dental.Models;
using Dental.Services;
using Dental.Utility;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dental.ViewModels
{
    public class PatientListViewModel : ViewModelBase
    {
        public ObservableCollection<Patient> Patients { get; } = new();

        [Reactive]
        public Patient? SelectedPatient { get; set; }

        [Reactive]
        public string? SearchText { get; set; } = string.Empty;

        private readonly ObservableAsPropertyHelper<IEnumerable<Patient>> filteredPatients;
        public IEnumerable<Patient> FilteredPatients => filteredPatients.Value;

        public ReactiveCommand<Unit, Unit> AddPatientCmd { get; }
        public ReactiveCommand<Unit, Unit> EditPatientCmd { get; }
        public ReactiveCommand<Unit, Unit> DeletePatientCmd { get; }

        private readonly IDataPersistence<Patient> patientPeristence;

        public PatientListViewModel(IDataPersistence<Patient>? patientPersistence = null)
        {
            this.patientPeristence = patientPersistence ?? Locator.Current.GetService<IDataPersistence<Patient>>()!;

            filteredPatients = this.WhenAnyValue(x => x.Patients,x => x.SearchText,x => x.Patients.Count,(patients, text,_) =>
                                                patients.Where(p => p.FullName.StartsWith(text!, StringComparison.OrdinalIgnoreCase)))
                                               .ToProperty(this, vm => vm.FilteredPatients);

            var canHandlePatient = this.WhenAnyValue(x => x.SelectedPatient).Select(x => x != null);

            AddPatientCmd = ReactiveCommand.CreateFromTask(AddPatientAsync);
            EditPatientCmd = ReactiveCommand.CreateFromTask(EditPatientAsync, canHandlePatient); 
            DeletePatientCmd = ReactiveCommand.CreateFromTask(DeletePatientAsync, canHandlePatient); 
        }

        #region Data loader

        public override async Task LoadAsync()
        {
            await patientPeristence.LoadAsync(Patients);
        }

        public override async Task SaveAsync()
        {
            await patientPeristence.SaveAsync(Patients);
        }

        #endregion


        #region Methods 

        private async Task AddPatientAsync()
        {
            var vm = new PatientViewModel();
            var patient = await Interactions.PatientInteraction.Handle(vm);

            if (patient != null)
            {
                Patients.Add(patient);
                SelectedPatient = patient; 
            }
        }

        private async Task EditPatientAsync()
        {
            if (SelectedPatient == null)
            {
                return; 
            }

            var vm = new PatientViewModel(SelectedPatient);
            var patient = await Interactions.PatientInteraction.Handle(vm); 

            if (patient != null)
            {
                SelectedPatient.FirstName = patient.FirstName;
                SelectedPatient.LastName = patient.LastName;
                SelectedPatient.Birthday = patient.Birthday;
                SelectedPatient.ImagePath = patient.ImagePath;
            }
        }

        private async Task DeletePatientAsync()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Question", "Do you want to remove this patient?", ButtonEnum.YesNo,Icon.Info);
            var result = await box.ShowAsync();

            if (SelectedPatient != null && result == ButtonResult.Yes)
            {
                Patients.Remove(SelectedPatient);
            }
        }

        #endregion
    }
}