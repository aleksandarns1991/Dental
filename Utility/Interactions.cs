using Dental.Models;
using Dental.Services;
using Dental.ViewModels;
using ReactiveUI;

namespace Dental.Utility
{
    public static class Interactions
    {
        public static Interaction<DoctorViewModel, Doctor> DoctorInteraction { get; } = new();
        public static Interaction<PatientViewModel, Patient> PatientInteraction { get; } = new();
        public static Interaction<VisitViewModel, Visit> VisitInteraction { get; } = new();
        public static Interaction<InterventionViewModel, Intervention> InterventionInteraction { get; } = new();
        public static Interaction<OpenFileDialogService, string[]> OpenFileDialogInteraction { get; } = new();
    }
}
