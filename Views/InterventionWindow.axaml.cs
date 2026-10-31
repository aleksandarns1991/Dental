using Avalonia;
using Dental.ViewModels;
using ReactiveUI;
using ReactiveUI.Avalonia;
using System;

namespace Dental.Views
{
    public partial class InterventionWindow : ReactiveWindow<InterventionViewModel>
    {
        public InterventionWindow()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.Bind(ViewModel, vm => vm.Title, view => view.textTitle.Text));
                d(this.Bind(ViewModel, vm => vm.Price, view => view.textPrice.Text)); 
                d(this.Bind(ViewModel, vm => vm.Description,view => view.textDescription.Text));

                d(this.BindCommand(ViewModel, vm => vm.SaveInterventionCmd, view => view.btnSave));
                d(ViewModel!.SaveInterventionCmd.Subscribe(result => Close(result)));
            });
        }
    }
}