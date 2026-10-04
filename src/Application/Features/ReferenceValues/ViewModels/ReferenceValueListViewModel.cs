#pragma warning disable CS0618
namespace BigLion.CPA.Application.Features.ReferenceValues.ViewModels
{
   [Obsolete("ใช้ SmartEnum จาก Domain.Enums แทน — ดู LookupRegistry.cs")]
   public class ReferenceValueListViewModel
    {
        public ICollection<ReferenceValueViewModel> ReferenceValues { get; set; }

        public ReferenceValueListViewModel()
        {
            ReferenceValues = new HashSet<ReferenceValueViewModel>();
        }
    }
}
