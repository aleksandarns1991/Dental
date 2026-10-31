using Dental.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace Dental.ViewModels
{
    public class InterventionViewModel : ReactiveObject
    {
        [Reactive]
        public string? Title { get; set; }
        [Reactive]
        public decimal Price { get; set; }
        [Reactive]
        public string? Description { get; set; } 

        public ReactiveCommand<Unit, Intervention> SaveInterventionCmd { get; }

        public InterventionViewModel(Intervention intervention) : this()
        {
            Title = intervention.Title; 
            Price = intervention.Price;
            Description = intervention.Description;
        }

        public InterventionViewModel()
        {
            var canSave = this.WhenAnyValue(x => x.Title, x => x.Price, (title, price) =>
                                           !string.IsNullOrEmpty(title)
                                        && price > 0);

            SaveInterventionCmd = ReactiveCommand.Create(SaveIntervention, canSave);
        }

        private Intervention SaveIntervention()
        {
            return new Intervention
            {
                Title = Title,
                Price = Price,
                Description = Description
            };
        }
    }
}
