using FluentAssertions;
using TescoStatementHandler.Factories;
using TescoStatementProcessorLambda.Dtos;
using Xunit;

namespace TescoStatementProcessorLambda.Tests;

public class TransactionFactoryTests
{
    [Fact]
    public async Task Create_SucceedsAsync()
    {
        CancellationToken cancellationToken = new();

        var aStatement = Directory.EnumerateFiles(Path.GetFullPath("./statements")).First();

        var lines = (await File.ReadAllLinesAsync(aStatement, cancellationToken)).Skip(1);

        foreach (var line in lines)
        {
            var transaction = TransactionFactory.Create(line, Guid.NewGuid());

            var values = line.Split(',');

            transaction.ReferenceNumber.Should().Be(values[(int)TransactionLineColumns.ReferenceNumber]);
            transaction.TransactionId.Should().NotBeEmpty();
            transaction.TransactionDate.DateTimeStr.Should().Be(values[(int)TransactionLineColumns.TransactionDate]);
            transaction.PostingDate.DateTimeStr.Should().Be(values[(int)TransactionLineColumns.PostingDate]);
            transaction.BillingAmount.BillingAmountString.Should().Be(values[(int)TransactionLineColumns.BillingAmount]);
            transaction.Merchant.Merchant.Should().Be(values[(int)TransactionLineColumns.Merchant].Replace(@"""", ""));
            transaction.Merchant.MerchantCity.Should().Be(values[(int)TransactionLineColumns.MerchantCity]);
            transaction.Merchant.MerchantState.Should().Be(values[(int)TransactionLineColumns.MerchantState]);
            transaction.Merchant.MerchantZip.Should().Be(values[(int)TransactionLineColumns.MerchantZip]);
            transaction.DebitCreditFlag.DebitCreditFlag.Should().Be(values[(int)TransactionLineColumns.DebitCreditFlag]);
            transaction.SICMCCCode.Should().Be(values[(int)TransactionLineColumns.SICMCCCode]);
            transaction.EncodedRawData.Should().Be(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(line)));
        }
    }
    
    [Fact]
    public async Task Create_EncodedRawDataUnique_SucceedsAsync()
    {
        CancellationToken cancellationToken = new();

        var aStatement = Directory.EnumerateFiles(Path.GetFullPath("./statements")).First();

        var lines = (await File.ReadAllLinesAsync(aStatement, cancellationToken)).Skip(1);

        var transactions = lines.Select(l => TransactionFactory.Create(l, Guid.NewGuid()));

        foreach (var transaction in transactions)
        {
            transactions.Where(t => t.EncodedRawData != transaction.EncodedRawData)
                .FirstOrDefault(t => t.EncodedRawData == transaction.EncodedRawData).Should().BeNull();

            transactions.Count(t => t.EncodedRawData == transaction.EncodedRawData).Should().Be(1);
        }
    }
}
