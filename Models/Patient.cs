using ReactiveUI;

namespace Dental.Models
{
    public class Patient : Person
    {
        // Create a template for patient: 
        private readonly ObservableAsPropertyHelper<string> patientTemplate; 
        public string PatientTemplate => patientTemplate.Value;

        public Patient()
        {
            patientTemplate = this.WhenAnyValue(x => x.FullName, x => x.Birthday, (fullName, birthday) => 
                                               $"{fullName},{birthday:dd/MM/yyyy}")
                                               .ToProperty(this, vm => vm.PatientTemplate);
        }
    }
}