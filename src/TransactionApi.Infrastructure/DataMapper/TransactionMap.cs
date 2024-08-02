using System.Globalization;
using CsvHelper.Configuration;
using TransactionApi.Domain.Entities;

namespace TransactionApi.Infrastructure.DataMapper;

public sealed class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
        {
            Map(m => m.TransactionId).Name("transaction_id");
            Map(m => m.Name).Name("name");
            Map(m => m.Email).Name("email");
            Map(m => m.Amount)
                .Name("amount")
                .Convert(args => decimal.Parse(args.Row.GetField("amount").Replace("$", ""), CultureInfo.InvariantCulture));
            Map(m => m.TransactionDate)
                .Name("transaction_date")
                .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");
            Map(m => m.ClientLocation).Name("client_location");
        }
}