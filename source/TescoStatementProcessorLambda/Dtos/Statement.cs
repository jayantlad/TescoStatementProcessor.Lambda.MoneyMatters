﻿
using System.Globalization;

namespace TescoStatementProcessorLambda;

public record class Statement(Guid id, IEnumerable<Transaction>? Transactions, string FileName, string Provider, string Product);

public record class Transaction
{
    public Guid TransactionId { get; init; }
    public DateTimeValue TransactionDate { get; init; }
    public DateTimeValue PostingDate { get; init; }
    public BillingAmountValue BillingAmount { get; init; }
    public MerchantValue Merchant { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public DebitCreditFlagValue DebitCreditFlag { get; init; }
    public string SICMCCCode { get; init; } = string.Empty;
    public string EncodedRawData { get; init; } = string.Empty;
}

public record class DateTimeValue()
{
    public string DateTimeStr { get; init; } = string.Empty;
    public DateTime DateTime => DateTime.Parse(DateTimeStr);
}

public record class MerchantValue(string Merchant, string MerchantCity, string MerchantState, string MerchantZip);

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