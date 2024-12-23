namespace EmailValidator.Model
{
    public interface ICheckEmailService
    {
        Task<bool> CheckSpfRecord(string domain, CancellationToken ct);
        Task<bool> CheckMxRecords(string domain, CancellationToken ct);
        ValueTask<ValidationResult> CheckEmail(string email, CancellationToken ct);
    }

    public class ValidationResult(bool mxRecords, bool spfRecords)
    {
        public bool MxRecords { get; private set; } = mxRecords;
        public bool SpfRecords { get; private set; } = spfRecords;
    }
}
