using FluentValidation.Results;

namespace BigLion.CPA.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }
        public IEnumerable<ValidationFailure> ValidationErrors { get; }
        public ValidationException()
            : base("โปรดป้อนข้อมูลให้ถูกต้องตามเงื่อนไขที่กำหนด")
        {
            Errors = new Dictionary<string, string[]>();
            ValidationErrors = new List<ValidationFailure>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            ValidationErrors = failures;
        }
    }
}
