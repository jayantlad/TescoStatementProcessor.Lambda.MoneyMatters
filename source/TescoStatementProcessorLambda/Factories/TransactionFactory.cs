using TescoStatementProcessorLambda;
using TescoStatementProcessorLambda.Dtos;

namespace TescoStatementHandler.Factories;

public class TransactionFactory
{
    public static Transaction Create(
        string rawData, Guid statementId)
    {
        var columns = rawData.Split(',');

        return Transaction.Create(Guid.NewGuid(), statementId, new DateTimeValue { DateTimeStr = columns[(int)TransactionLineColumns.TransactionDate] },
            new DateTimeValue { DateTimeStr = columns[(int)TransactionLineColumns.PostingDate] }, new BillingAmountValue { BillingAmountString = columns[(int)TransactionLineColumns.BillingAmount] },
            new MerchantValue
            {
                Merchant = columns[(int)TransactionLineColumns.Merchant].Replace(@"""", ""),
                MerchantCity = columns[(int)TransactionLineColumns.MerchantCity],
                MerchantState = columns[(int)TransactionLineColumns.MerchantState],
                MerchantZip = columns[(int)TransactionLineColumns.MerchantZip]
            },
            columns[(int)TransactionLineColumns.ReferenceNumber], new DebitCreditFlagValue { DebitCreditFlag = columns[(int)TransactionLineColumns.DebitCreditFlag] },
            columns[(int)TransactionLineColumns.SICMCCCode], Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawData)));
    }
}