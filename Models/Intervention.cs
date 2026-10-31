using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Dental.Models
{
    public class Intervention : ReactiveObject
    {
        [JsonProperty]
        public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty]
        [Reactive]
        public Guid VisitId { get; set; }
        [JsonProperty]
        [Reactive]
        public string? Title { get; set; }
        [JsonProperty]
        [Reactive]
        public decimal Price { get; set; }
        [JsonProperty]
        [Reactive]
        public string? Description { get; set; }    
    }
}