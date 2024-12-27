using EmailValidator.Model;
using DnsClient;
using System.Text.RegularExpressions;

namespace EmailValidator.Services
{
    public class EmailValidatorService : IEmailValidatorService
    {
        private readonly LookupClient client = new();
        private static readonly string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private readonly Regex regex = new(pattern);

        public async ValueTask<bool> CheckSpfRecord(string domain, CancellationToken ct)
        {
            IDnsQueryResponse response = await client.QueryAsync(domain, QueryType.TXT, QueryClass.IN, ct);

            var spfRecord = response.AllRecords
                .TxtRecords()
                .FirstOrDefault(x => x.Text
                .FirstOrDefault(str => str.StartsWith("v=spf1")) is not null);

            return spfRecord is not null;
        }

        public async ValueTask<bool> CheckMxRecord(string domain, CancellationToken ct)
        {
            IDnsQueryResponse response = await client.QueryAsync(domain, QueryType.MX, QueryClass.IN, ct);
            var records = response.AllRecords.MxRecords();

            if (records.Any()) return true;

            return false;
        }

        public async ValueTask<ValidationResult> ValidateEmail(string email, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

            if(!regex.IsMatch(email))
                throw new BadHttpRequestException("Email validation error via regular expression");

            string domain = email.Split("@")[1];

            bool spfRecord = await CheckSpfRecord(domain, ct);
            bool mxRecords = await CheckMxRecord(domain, ct);

            return new ValidationResult(mxRecords, spfRecord);
        }
    }
}