using CsvHelper;
using Npgsql;
using PostgreSQLCopyHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleCsvProcessor
{
    public interface ICsvService
    {
        Task ProcessAsync();
    }

    public class CsvService : ICsvService
    {
        private readonly IDbConnectionStringProvider dbConnectionStringProvider;

        public CsvService(IDbConnectionStringProvider dbConnectionStringProvider)
        {
            this.dbConnectionStringProvider = dbConnectionStringProvider;
        }

        public async Task ProcessAsync()
        {
            await Task.CompletedTask;
            var data = GetData();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await SaveDataAsync(data);
            stopwatch.Stop();
            var timeElapsed = stopwatch.Elapsed;
            Console.WriteLine($"{data.Count()} records processed in {timeElapsed.TotalMinutes} minutes");
        }

        public IEnumerable<TblCsv> GetData()
        {
            var result = new List<TblCsv>();
            using var reader = new StreamReader("majestic_million.csv");
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Read();
            csv.ReadHeader();
            while(csv.Read())
            {
                var record = csv.GetRecords<TblCsv>();
                result = record.ToList();
            }
            return result;
        }

        public async Task SaveDataAsync(IEnumerable<TblCsv> data)
        {
            using var connection = new NpgsqlConnection(await dbConnectionStringProvider.GetConnectionStringAsync());
            connection.Open();
            var copyHelper = GetHelper();
            var result = copyHelper.SaveAll(connection, data);
        }

        private PostgreSQLCopyHelper<TblCsv> GetHelper()
        {
            var copyHelper = new PostgreSQLCopyHelper<TblCsv>("tbl_csv")
                .MapInteger("GlobalRank", x => x.GlobalRank)
                .MapInteger("TldRank", x => x.TldRank)
                .MapText("Domain", x => x.Domain)
                .MapText("TLD", x => x.TLD)
                .MapText("RefSubNets", x => x.RefSubNets)
                .MapText("RefIPs", x => x.RefIPs)
                .MapText("IDN_Domain", x => x.IDN_Domain)
                .MapText("IDN_TLD", x => x.IDN_TLD)
                .MapInteger("PrevGlobalRank", x => x.PrevGlobalRank)
                .MapInteger("PrevTldRank", x => x.PrevTldRank)
                .MapText("PrevRefSubNets", x => x.PrevRefSubNets)
                .MapText("PrevRefIPs", x => x.PrevRefIPs);
            return copyHelper;
        }
    }
}
