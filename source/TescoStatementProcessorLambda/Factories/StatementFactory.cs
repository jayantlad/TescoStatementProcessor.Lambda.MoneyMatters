using Amazon.Runtime.SharedInterfaces;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using TescoStatementProcessorLambda;

namespace TescoStatementHandler.Factories;

public class StatementFactory(ILogger<StatementFactory> logger) : IStatementFactory
{
    public async Task<Statement> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken)
    {
        List<Transaction> transactionLines = new List<Transaction>();
        
        using var stream = getObjectResponse.ResponseStream;
        var sr = new StreamReader(stream);

        if (sr.EndOfStream)
        {
            logger.LogInformation("Statement empty {@getObjectResponse}", getObjectResponse);
            return new Statement { StatementId = Guid.NewGuid(), Transactions = Enumerable.Empty<Transaction>().ToList(), FileName = string.Empty, Provider = string.Empty, Product = string.Empty };
        }
            

        sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync(cancellationToken);
            transactionLines.Add(TransactionFactory.Create(line));
        }
        return new Statement { StatementId = Guid.NewGuid(), Transactions = transactionLines, FileName = getObjectResponse.Key, Provider = Constants.Tesco, Product = Constants.MasterCard };
    }
}

public interface IStatementFactory
{
    Task<Statement> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken);
}