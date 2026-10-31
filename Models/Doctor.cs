using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;

namespace Dental.Models
{
    public class Doctor : Person
    {
        [JsonProperty]
        [Reactive]
        public string? Specialty { get; set; }
    }
}
