using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System.Diagnostics;
using System;
using Dental.Utility;
using Avalonia.Platform;
using Avalonia.Data;

namespace Dental.Views
{
    public partial class DoctorWindow : ReactiveWindow<DoctorViewModel>
    {
        public DoctorWindow()
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

                d(this.OneWayBind(ViewModel, vm => vm.Specialties, view => view.comboSpecialties.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.Specialty, view => view.comboSpecialties.SelectedItem));

                d(this.Bind(ViewModel, vm => vm.Birthday, view => view.calendarBirth.SelectedDate));
                d(this.OneWayBind(ViewModel, vm => vm.ImagePath, view => view.imagePhoto.Source, value => !string.IsNullOrEmpty(value) ? new Bitmap(value!) : new Bitmap(AssetLoader.Open(new Uri("avares://Dental/Assets/Images/doctor.png")))));

                d(this.BindCommand(ViewModel, vm => vm.OpenFileCmd, view => view.buttonBrowse));
                d(this.BindCommand(ViewModel, vm => vm.SaveDoctorCmd, view => view.buttonSave));

                d(ViewModel!.SaveDoctorCmd.Subscribe(result => Close(result)));

                comboSpecialties.ItemTemplate = null;
                comboSpecialties.DisplayMemberBinding = new Binding();
            });
        }
    }
}