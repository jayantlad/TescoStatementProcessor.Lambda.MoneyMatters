using TescoStatementProcessorLambda;
using TescoStatementProcessorLambda.Dtos;

namespace TescoStatementHandler.Factories;

public class TransactionFactory
{
    public static Transaction Create(
        string rawData)
    {
        var columns = rawData.Split(',');

        return new Transaction
        {
            TransactionId = Guid.NewGuid(),
            TransactionDate = new DateTimeValue { DateTimeStr = columns[(int)TransactionLineColumns.TransactionDate] },
            PostingDate = new DateTimeValue { DateTimeStr = columns[(int)TransactionLineColumns.PostingDate] },
            BillingAmount = new BillingAmountValue { BillingAmountString = columns[(int)TransactionLineColumns.BillingAmount] },
            Merchant = new MerchantValue
            (
                Merchant: columns[(int)TransactionLineColumns.Merchant].Replace(@"""", ""),
                MerchantCity: columns[(int)TransactionLineColumns.MerchantCity],
                MerchantState: columns[(int)TransactionLineColumns.MerchantState],
                MerchantZip: columns[(int)TransactionLineColumns.MerchantZip]
            ),
            ReferenceNumber = columns[(int)TransactionLineColumns.ReferenceNumber],
            DebitCreditFlag = new DebitCreditFlagValue { DebitCreditFlag = columns[(int)TransactionLineColumns.DebitCreditFlag] },
            SICMCCCode = columns[(int)TransactionLineColumns.SICMCCCode],
            EncodedRawData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(rawData))
        };
    }
}