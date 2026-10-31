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
    public class PatientViewModel : ReactiveObject
    {
        [Reactive]
        public string? FirstName { get; set; }
        [Reactive]
        public string? LastName { get; set; }
        [Reactive]
        public DateTime? Birthday { get; set; }
        [Reactive]
        public string? ImagePath { get; set; }

        public ReactiveCommand<Unit, Patient> SavePatientCmd { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCmd { get; }

        public PatientViewModel(Patient patient) : this()
        {
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Birthday = patient.Birthday;
            ImagePath = patient.ImagePath;
        }

        public PatientViewModel()
        {
            var canSave = this.WhenAnyValue(x => x.FirstName, x => x.LastName, x => x.Birthday, (firstName, lastName, birthday) =>
                                           !string.IsNullOrEmpty(firstName)
                                        && !string.IsNullOrEmpty(lastName)
                                        && birthday != null);

            SavePatientCmd = ReactiveCommand.Create(SavePatient,canSave);
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

        private Patient SavePatient()
        {
            return new Patient
            {
                FirstName = FirstName,
                LastName = LastName,
                Birthday = Birthday,
                ImagePath = ImagePath
            };
        }
    }
}
