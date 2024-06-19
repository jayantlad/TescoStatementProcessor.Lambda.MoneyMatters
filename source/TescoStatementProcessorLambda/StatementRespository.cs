using Amazon.DynamoDBv2.DataModel;

namespace TescoStatementProcessorLambda;

internal sealed class StatementRespository(IDynamoDBContext dynamoDBContext) : IStatementRespository
{
    public async Task SaveStatementAsync(Statement statement, CancellationToken cancellationToken)
    {
        await dynamoDBContext.SaveAsync(statement, cancellationToken);
    }
}

internal interface IStatementRespository
{
    Task SaveStatementAsync(Statement statement, CancellationToken cancellationToken);
}