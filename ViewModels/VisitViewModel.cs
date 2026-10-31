using Dental.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace Dental.ViewModels
{
    public class VisitViewModel : ReactiveObject
    {
        public IEnumerable<Doctor>? Doctors { get; } 
        public IEnumerable<Patient>? Patients { get; }

        [Reactive]
        public Doctor? SelectedDoctor { get; set; }
        [Reactive]
        public Patient? SelectedPatient { get; set; }
        [Reactive]
        public DateTime? Scheduled { get; set; } = DateTime.Now;

        public ReactiveCommand<Unit, Visit> SaveVisitCmd { get; }

        public VisitViewModel(IEnumerable<Doctor> doctors,IEnumerable<Patient> patients,Visit visit) : this()
        {
            Doctors = doctors;
            Patients = patients;

            SelectedDoctor = Doctors.FirstOrDefault(d => d.Id == visit.DoctorId);
            SelectedPatient = Patients.FirstOrDefault(p => p.Id == visit.PatientId);

            Scheduled = visit.Scheduled;
        }

        public VisitViewModel(IEnumerable<Doctor> doctors,IEnumerable<Patient> patients) : this()
        {
            Doctors = doctors;
            Patients = patients; 
        }

        public VisitViewModel()
        {
            var canSave = this.WhenAnyValue(x => x.SelectedDoctor, x => x.SelectedPatient, x => x.Scheduled, (doctor, patient, scheduled) =>
                                           doctor != null
                                        && patient != null
                                        && scheduled != null);

            SaveVisitCmd = ReactiveCommand.Create(SaveVisit, canSave);
        }

        private Visit SaveVisit()
        {
            return new Visit
            {
                DoctorId = SelectedDoctor!.Id,
                PatientId = SelectedPatient!.Id,
                Scheduled = Scheduled
            };
        }
    }
}