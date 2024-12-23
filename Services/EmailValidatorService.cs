using EmailValidator.Model;
using DnsClient;
using System.Text.RegularExpressions;

namespace EmailValidator.Services
{
    public class CheckEmailService: ICheckEmailService
    {
        private readonly LookupClient client = new();
        static readonly string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private readonly Regex regex = new(pattern);

        public async ValueTask<bool> CheckSpfRecord(string hostString, CancellationToken ct)
        {
            IDnsQueryResponse txtResponse = await client.QueryAsync(hostString, QueryType.TXT, QueryClass.IN, ct);
            var records = txtResponse.AllRecords.TxtRecords().ToArray();

            var isSpf = records.Select((record) => record.Text.FirstOrDefault()?.StartsWith("v=spf1")).FirstOrDefault() ?? false;
            return isSpf;
        }

        public async ValueTask<bool> CheckMxRecords(string hostString, CancellationToken ct)
        {
            IDnsQueryResponse txtResponse = await client.QueryAsync(hostString, QueryType.MX, QueryClass.IN, ct);
            var records = txtResponse.AllRecords.MxRecords().ToArray();

            if (records.Length > 0) return true;
            
            return false;
        }

        public async ValueTask<bool> CheckEmail(string email, CancellationToken ct)
        {
            ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

            if(!regex.IsMatch(email)) return false;

            string domain = email.Split("@")[1];

            bool spf = await CheckSpfRecord(domain, ct);
            bool mxRecords = await CheckMxRecords(domain, ct);

            if(!mxRecords && !spf) return false;

            return true;
        }
    }
}
