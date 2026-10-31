using Dental.Models;
using Dental.Services;
using Dental.Utility;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dental.ViewModels
{
    public class DoctorViewModel : ReactiveObject
    {
        [Reactive]
        public string? FirstName { get; set; }
        [Reactive]
        public string? LastName { get; set; }
        [Reactive]
        public string? Specialty { get; set; }
        [Reactive]
        public DateTime? Birthday { get; set; }
        [Reactive]
        public string? ImagePath { get; set; }

        public string[] Specialties => new string[]
        {
            "Nurse",
            "Stomatologist",
            "Orthodontist",
            "Endodontist",
            "Oral Surgeon"
        };

        public ReactiveCommand<Unit, Doctor> SaveDoctorCmd { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCmd { get; }

        public DoctorViewModel(Doctor doctor) : this()
        {
            FirstName = doctor.FirstName;
            LastName = doctor.LastName;
            Specialty = doctor.Specialty;
            Birthday = doctor.Birthday;
            ImagePath = doctor.ImagePath;
        }

        public DoctorViewModel()
        {
            var canSave = this.WhenAnyValue(x => x.FirstName, x => x.LastName, x => x.Specialty, x => x.Birthday, (firstName, lastName, specialty, birthday) =>
                                           !string.IsNullOrEmpty(firstName)
                                        && !string.IsNullOrEmpty(lastName)
                                        && specialty != null
                                        && birthday != null);

            SaveDoctorCmd = ReactiveCommand.Create(SaveDoctor,canSave);
            OpenFileCmd = ReactiveCommand.CreateFromTask(OpenFileAsync);
        }

        private async Task OpenFileAsync()
        {
            var openFile = new OpenFileDialogService
            {
                AllowMultiple = false,
                Extensions = "Images:*.jpg,*.bmp,*.tiff,*.png"
            };

            var filePath = await Interactions.OpenFileDialogInteraction.Handle(openFile);

            if (filePath != null && filePath.Length > 0)
            {
                ImagePath = filePath.FirstOrDefault();
            }
        }

        private Doctor SaveDoctor()
        {
            return new Doctor
            {
                FirstName = FirstName,
                LastName = LastName,
                Specialty = Specialty,
                Birthday = Birthday,
                ImagePath = ImagePath
            };
        }
    }
}
