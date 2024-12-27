namespace EmailValidator.Model
{
    public interface IEmailValidatorService
    {
        ValueTask<bool> CheckSpfRecord(string domain, CancellationToken ct);
        ValueTask<bool> CheckMxRecord(string domain, CancellationToken ct);
        ValueTask<ValidationResult> ValidateEmail(string email, CancellationToken ct);
    }

    public class ValidationResult(bool mxRecords, bool spfRecords)
    {
        public bool MxRecords { get; private set; } = mxRecords;
        public bool SpfRecords { get; private set; } = spfRecords;
    }
}
