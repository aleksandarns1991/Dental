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
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Dental.ViewModels
{
    public class InterventionListViewModel : ViewModelBase
    {
        public ObservableCollection<Intervention> Interventions { get; } = new();

        [Reactive]
        public Intervention? SelectedIntervention { get; set; }
        [Reactive]
        public string? SearchText { get; set; } = string.Empty;

        private readonly ObservableAsPropertyHelper<IEnumerable<Intervention>> filteredInterventions;
        public IEnumerable<Intervention> FilteredInterventions => filteredInterventions.Value;

        public ReactiveCommand<Unit, Unit> AddInterventionCmd { get; }
        public ReactiveCommand<Unit, Unit> EditInterventionCmd { get; }
        public ReactiveCommand<Unit, Unit> DeleteInterventionCmd { get; }

        private readonly VisitListViewModel visitList;

        private readonly IDataPersistence<Intervention> interventionPersistence;

        public InterventionListViewModel(VisitListViewModel visitList,IDataPersistence<Intervention>? interventionPersistence = null)
        {
            this.visitList = visitList;
            this.interventionPersistence = interventionPersistence ?? Locator.Current.GetService<IDataPersistence<Intervention>>()!;

            filteredInterventions = this.WhenAnyValue(x => x.SearchText, x => x.visitList.SelectedVisit, x => x.Interventions, x => x.Interventions.Count, (text, visit, interventions, _) =>
                                                     interventions.Where(i => i.VisitId == visit?.Id && i.Title!.StartsWith(text!, StringComparison.OrdinalIgnoreCase)))
                                                     .ToProperty(this, vm => vm.FilteredInterventions);

            var canAddIntervention = this.WhenAnyValue(x => x.visitList.SelectedVisit).Select(x => x != null); 
            var canHandleIntervention = this.WhenAnyValue(x => x.SelectedIntervention).Select(x => x != null); 

            AddInterventionCmd = ReactiveCommand.CreateFromTask(AddInterventionAsync,canAddIntervention);
            EditInterventionCmd = ReactiveCommand.CreateFromTask(EditInterventionAsync, canHandleIntervention);
            DeleteInterventionCmd = ReactiveCommand.CreateFromTask(DeleteInterventionAsync, canHandleIntervention);
        }

        #region Data loader

        public override async Task LoadAsync()
        {
            await interventionPersistence!.LoadAsync(Interventions);
            Debug.WriteLine(Interventions.Count);
        }

        public override async Task SaveAsync()
        {
            await interventionPersistence!.SaveAsync(Interventions);
        }

        #endregion

        #region Methods

        private async Task AddInterventionAsync()
        {
            var vm = new InterventionViewModel();
            var intervention = await Interactions.InterventionInteraction.Handle(vm); 

            if (intervention != null)
            {
                intervention.VisitId = visitList.SelectedVisit!.Id; 
                Interventions.Add(intervention);    
            }
        }

        private async Task EditInterventionAsync()
        {
            if (SelectedIntervention == null)
            {
                return;
            }

            var vm = new InterventionViewModel(SelectedIntervention);
            var intervention = await Interactions.InterventionInteraction.Handle(vm); 

            if (intervention != null)
            {
                SelectedIntervention.Title = intervention.Title; 
                SelectedIntervention.Price = intervention.Price;
                SelectedIntervention.Description = intervention.Description;
            }
        }

        private async Task DeleteInterventionAsync()
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Question", "Do you want to remove this intervention?", MsBox.Avalonia.Enums.ButtonEnum.YesNo,Icon.Info);
            var result = await box.ShowAsync();

            if (SelectedIntervention != null && result == ButtonResult.Yes)
            {
                Interventions.Remove(SelectedIntervention);                
            }
        }

        #endregion 
    }
}