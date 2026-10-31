using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Dental.Models
{
    public class Person : ReactiveObject
    {
        [JsonProperty]
        public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty]
        [Reactive]
        public string? FirstName { get; set; }
        [JsonProperty]
        [Reactive]
        public string? LastName { get; set; }
        [JsonProperty]
        [Reactive]
        public DateTime? Birthday { get; set; }
        [JsonProperty]
        [Reactive]
        public string? ImagePath { get; set; }

        // Track changes in first and last name to update full name:
        private readonly ObservableAsPropertyHelper<string> fullName;
        public string FullName => fullName.Value;

        public Person()
        {
            fullName = this.WhenAnyValue(x => x.FirstName, x => x.LastName, (firstName, lastName) => $"{firstName} {lastName}").ToProperty(this, vm => vm.FullName);
        }
    }
}
