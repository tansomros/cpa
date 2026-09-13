namespace Cpa.Application.Features.Patients.ViewModels
{
   public class PatientListViewModel
    {
        public ICollection<PatientViewModel> Patients { get; set; }

        public PatientListViewModel()
        {
            Patients = new HashSet<PatientViewModel>();
        }
    }
}
