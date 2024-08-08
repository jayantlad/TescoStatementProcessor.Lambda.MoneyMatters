using Amazon.DynamoDBv2.DataModel;

namespace TescoStatementProcessorLambda;

internal sealed class TransactionRespository(IDynamoDBContext dynamoDBContext) : ITransactionRespository
{
    public async Task SaveTransactionsAsync(List<Transaction> transactions, CancellationToken cancellationToken)
    {
        List<Task> tasks = new();
        
        transactions.ForEach(t => { tasks.Add(dynamoDBContext.SaveAsync(t, cancellationToken)); });

        await Task.WhenAll(tasks.ToArray());
    }
}

internal interface ITransactionRespository
{
    Task SaveTransactionsAsync(List<Transaction> transactions, CancellationToken cancellationToken);
}