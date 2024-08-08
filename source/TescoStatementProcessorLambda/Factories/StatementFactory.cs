using Amazon.Runtime.SharedInterfaces;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using TescoStatementProcessorLambda;

namespace TescoStatementHandler.Factories;

public class StatementFactory(ILogger<StatementFactory> logger) : IStatementFactory
{
    public async Task<(Statement Statement, List<Transaction> Transactions)> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken)
    {
        List<Transaction> transactionLines = new List<Transaction>();
        var statementId = Guid.NewGuid();

        using var stream = getObjectResponse.ResponseStream;
        var sr = new StreamReader(stream);

        if (sr.EndOfStream)
        {
            logger.LogInformation("Statement empty {@getObjectResponse}", getObjectResponse);

            return (Statement.Create(statementId, Enumerable.Empty<Guid>().ToList(), string.Empty, string.Empty, string.Empty), Enumerable.Empty<Transaction>().ToList());
        }

        await sr.ReadLineAsync();

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync(cancellationToken);
            transactionLines.Add(TransactionFactory.Create(line!, statementId));
        }
        return (Statement.Create(statementId, transactionLines.Select(t => t.TransactionId).ToList(), getObjectResponse.Key, Constants.Tesco, Constants.MasterCard), transactionLines);
    }
}

public interface IStatementFactory
{
    Task<(Statement Statement, List<Transaction> Transactions)> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken);
}