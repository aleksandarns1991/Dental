using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Dental.Utility;
using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;

namespace Dental.Views
{
    public partial class PatientWindow : ReactiveWindow<PatientViewModel>
    {
        public PatientWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(Interactions.OpenFileDialogInteraction.RegisterHandler(async interaction =>
                {
                    var output = await interaction.Input.ShowAsync(this);
                    interaction.SetOutput(output);
                }));

                d(this.Bind(ViewModel, vm => vm.FirstName, view => view.textFirstName.Text));
                d(this.Bind(ViewModel, vm => vm.LastName, view => view.textLastName.Text));
                d(this.Bind(ViewModel, vm => vm.Birthday, view => view.calendarBirth.SelectedDate));

                d(this.OneWayBind(ViewModel, vm => vm.ImagePath, view => view.imagePhoto.Source, value => !string.IsNullOrEmpty(value) ? new Bitmap(value) : new Bitmap(AssetLoader.Open(new System.Uri("avares://Dental/Assets/Images/patient.png")))));

                d(this.BindCommand(ViewModel, vm => vm.OpenFileCmd, view => view.btnBrowse));

                d(this.BindCommand(ViewModel, vm => vm.SavePatientCmd, view => view.btnSave));
                d(ViewModel!.SavePatientCmd.Subscribe(result => Close(result)));
            });
        }
    }
}