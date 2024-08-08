
using Amazon.DynamoDBv2.DataModel;
using System.Globalization;

namespace TescoStatementProcessorLambda;

[DynamoDBTable("Statements")]
public record class Statement
{
    [DynamoDBHashKey]
    public Guid StatementId { get; init; }

    public List<Guid>? Transactions { get; init; }
    public string FileName { get; init; } = string.Empty;
    public string Provider { get; init; } = string.Empty;
    public string Product { get; init; } = string.Empty;

    internal static Statement Create(Guid guid, List<Guid> transactionIds, string fileName, string provider, string product) =>
        new Statement { StatementId = guid, Transactions = transactionIds, FileName = fileName, Provider = provider, Product = product };
}

[DynamoDBTable("Transactions")]
public record class Transaction
{
    [DynamoDBHashKey]
    public Guid TransactionId { get; init; }
    public DateTimeValue TransactionDate { get; init; } 
    public DateTimeValue PostingDate { get; init; }
    public BillingAmountValue BillingAmount { get; init; }
    public MerchantValue Merchant { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public DebitCreditFlagValue DebitCreditFlag { get; init; }
    public string SICMCCCode { get; init; } = string.Empty;
    public string EncodedRawData { get; init; } = string.Empty;
    public Guid StatementId { get; init; }

    internal static Transaction Create(Guid guid, Guid statementId, DateTimeValue transactionDate, DateTimeValue postingDate, BillingAmountValue billingAmountValue,
        MerchantValue merchantValue, string referenceNumber, DebitCreditFlagValue debitCreditFlagValue, string sICMCCCode, string encodedRawData) =>
        new Transaction
        {
            TransactionId = guid,
            StatementId = statementId,
            TransactionDate = transactionDate,
            PostingDate = postingDate,
            BillingAmount = billingAmountValue,
            Merchant = merchantValue,
            ReferenceNumber = referenceNumber,
            DebitCreditFlag = debitCreditFlagValue,
            SICMCCCode = sICMCCCode,
            EncodedRawData = encodedRawData
        };
}

public record class DateTimeValue()
{
    public string DateTimeStr { get; init; } = string.Empty;
    public DateTime DateTime => DateTime.Parse(DateTimeStr, CultureInfo.InvariantCulture);
}

public record class MerchantValue
{
    public string Merchant { get; set; } = string.Empty;
    public string MerchantCity { get; set; } = string.Empty;
    public string MerchantState { get; set; } = string.Empty;
    public string MerchantZip { get; set; } = string.Empty;
}

public record class BillingAmountValue()
{
    public string BillingAmountString { get; init; } = string.Empty;
    public decimal BillingAmountFormatted => decimal.Parse(BillingAmountString.Replace(@"""", ""), NumberStyles.AllowCurrencySymbol | NumberStyles.Float);
}

public record class DebitCreditFlagValue()
{
    public string DebitCreditFlag { get; init; } = string.Empty;
    public string TransactionType => DebitCreditFlag.Equals("D") ? Constants.Debit : Constants.Credit;
}