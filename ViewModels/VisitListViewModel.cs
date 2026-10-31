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
    public class VisitListViewModel : ViewModelBase
    {
        public ObservableCollection<Visit> Visits { get; } = new();

        [Reactive]
        public Visit? SelectedVisit { get; set; }

        [Reactive]
        public DateTime? Scheduled { get; set; } = DateTime.Now;

        private readonly ObservableAsPropertyHelper<IEnumerable<Visit>> filteredVisits; 
        public IEnumerable<Visit> FilteredVisits => filteredVisits.Value;

        public ReactiveCommand<Unit, Unit> AddVisitCmd { get; }
        public ReactiveCommand<Unit, Unit> EditVisitCmd { get; }
        public ReactiveCommand<Unit, Unit> DeleteVisitCmd { get; }

        private readonly DoctorListViewModel doctorList;
        private readonly PatientListViewModel patientList;

        private readonly IDataPersistence<Visit> visitPersistence;

        public VisitListViewModel(DoctorListViewModel doctorList,PatientListViewModel patientList,IDataPersistence<Visit>? visitPersistence = null)
        {
            this.doctorList = doctorList;
            this.patientList = patientList;

            this.visitPersistence = visitPersistence ?? Locator.Current.GetService<IDataPersistence<Visit>>()!;

            filteredVisits = this.WhenAnyValue(x => x.Scheduled,x => x.Visits,x => x.Visits.Count,(scheduled,visits,_) =>
                                              visits.Where(v => v.Scheduled!.Value.Date == scheduled!.Value.Date))
                                             .ToProperty(this, vm => vm.FilteredVisits);
                                              
            var canHandleVisit = this.WhenAnyValue(x => x.SelectedVisit).Select(x => x != null);

            AddVisitCmd = ReactiveCommand.CreateFromTask(AddVisitAsync); 
            EditVisitCmd = ReactiveCommand.CreateFromTask(EditVisitAsync,canHandleVisit);
            DeleteVisitCmd = ReactiveCommand.CreateFromTask(DeleteVisitAsync,canHandleVisit);    
        }

        #region Data loader
        
        public override async Task LoadAsync()
        {
            await visitPersistence.LoadAsync(Visits);
        }
        
        public override async Task SaveAsync()
        {
            await visitPersistence.SaveAsync(Visits);
        }

        #endregion

        #region Methods

        private async Task AddVisitAsync()
        {
            var doctors = doctorList.Doctors;
            var patients = patientList.Patients; 

            var vm = new VisitViewModel(doctors, patients);
            var visit = await Interactions.VisitInteraction.Handle(vm); 

            if (visit != null)
            {
                Visits.Add(visit);
                SelectedVisit = visit;
            }
        }

        private async Task EditVisitAsync()
        {
            if (SelectedVisit == null)
            {
                return;
            }

            var doctors = doctorList.Doctors;
            var patients = patientList.Patients;

            var vm = new VisitViewModel(doctors, patients, SelectedVisit);
            var visit = await Interactions.VisitInteraction.Handle(vm); 

            if (visit != null)
            {
                SelectedVisit.DoctorId = visit.DoctorId;
                SelectedVisit.PatientId = visit.PatientId;
                SelectedVisit.Scheduled = visit.Scheduled;
            }
        }

        private async Task DeleteVisitAsync()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Question", "Do you want to remove this visit?", ButtonEnum.YesNo, Icon.Info);
            var result = await box.ShowAsync();

            if (SelectedVisit != null && result == ButtonResult.Yes)
            {
                Visits.Remove(SelectedVisit);
            }
        }

        #endregion 
    }
}
