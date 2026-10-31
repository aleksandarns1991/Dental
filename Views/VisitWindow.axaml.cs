using Avalonia;
using Avalonia.Data;
using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;

namespace Dental.Views
{
    public partial class VisitWindow : ReactiveWindow<VisitViewModel>
    {
        public VisitWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.Doctors, view => view.comboDoctors.ItemsSource));
                d(this.Bind(ViewModel, vm => vm.SelectedDoctor, view => view.comboDoctors.SelectedItem));

                d(this.OneWayBind(ViewModel, vm => vm.Patients, view => view.comboPatients.ItemsSource)); 
                d(this.Bind(ViewModel,vm => vm.SelectedPatient,view => view.comboPatients.SelectedItem));

                d(this.Bind(ViewModel, vm => vm.Scheduled, view => view.calendarSchedule.SelectedDate));

                d(this.BindCommand(ViewModel, vm => vm.SaveVisitCmd, view => view.btnSave));
                d(ViewModel!.SaveVisitCmd.Subscribe(result => Close(result)));

                comboDoctors.ItemTemplate = null;
                comboDoctors.DisplayMemberBinding = new Binding("FullName");

                comboPatients.ItemTemplate = null;
                comboPatients.DisplayMemberBinding = new Binding("PatientTemplate");
            });
        }
    }
}