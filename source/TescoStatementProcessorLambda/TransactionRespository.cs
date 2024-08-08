using Amazon.DynamoDBv2.DataModel;

namespace TescoStatementProcessorLambda;

internal sealed class TransactionRespository(IDynamoDBContext dynamoDBContext) : ITransactionRespository
{
    public async Task SaveStatementAsync(List<Transaction> transactions, CancellationToken cancellationToken)
    {
        await dynamoDBContext.SaveAsync(transactions, cancellationToken);
    }
}

internal interface ITransactionRespository
{
    Task SaveStatementAsync(List<Transaction> transactions, CancellationToken cancellationToken);
}