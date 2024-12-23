namespace EmailValidator.Model
{
    public interface ICheckEmailService
    {
        ValueTask<bool> CheckSpfRecord(string hostString, CancellationToken ct);
        ValueTask<bool> CheckMxRecords(string hostString, CancellationToken ct);
        ValueTask<bool> CheckEmail(string email, CancellationToken ct);
    }
}
