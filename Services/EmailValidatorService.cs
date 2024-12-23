using EmailValidator.Model;
using DnsClient;
using System.Text.RegularExpressions;
using DnsClient.Protocol;

namespace EmailValidator.Services
{
    public class CheckEmailService : ICheckEmailService
    {
        private readonly LookupClient client = new();
        static readonly string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private readonly Regex regex = new(pattern);

        public async Task<bool> CheckSpfRecord(string domain, CancellationToken ct)
        {
            IDnsQueryResponse txtResponse = await client.QueryAsync(domain, QueryType.TXT, QueryClass.IN, ct);

            bool spfRecord = txtResponse.AllRecords
                .TxtRecords()
                .ToArray()
                .Where((record) => record.Text.FirstOrDefault()?.StartsWith("v=spf1") ?? false).Any();

            return spfRecord;
        }

        public async Task<bool> CheckMxRecords(string domain, CancellationToken ct)
        {
            IDnsQueryResponse mxResponse = await client.QueryAsync(domain, QueryType.MX, QueryClass.IN, ct);
            MxRecord[] records = mxResponse.AllRecords.MxRecords().ToArray();

            if (records.Length == 0) return false;

            return true;
        }

        public async ValueTask<ValidationResult> CheckEmail(string email, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

            if(!regex.IsMatch(email))
                throw new BadHttpRequestException("Email validation error via regular expression");

            string domain = email.Split("@")[1];

            bool spfRecord = await CheckSpfRecord(domain, ct);
            bool mxRecords = await CheckMxRecords(domain, ct);

            return new ValidationResult(mxRecords, spfRecord);
        }
    }
}
