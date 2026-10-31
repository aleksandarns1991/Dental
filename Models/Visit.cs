using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace Dental.Models
{
    public class Visit : ReactiveObject
    {
        [JsonProperty]
        public Guid Id { get; private set; } = Guid.NewGuid();
        [JsonProperty]
        [Reactive]
        public Guid DoctorId { get; set; }
        [JsonProperty]
        [Reactive]
        public Guid PatientId { get; set; }
        [JsonProperty]
        [Reactive]
        public DateTime? Scheduled { get; set; }
    }
}