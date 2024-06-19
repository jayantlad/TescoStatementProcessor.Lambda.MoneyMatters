using Amazon.S3.Model;
using TescoStatementProcessorLambda;

namespace TescoStatementHandler.Factories;

public class StatementFactory : IStatementFactory
{
    public async Task<Statement> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken)
    {
        IList<Transaction> transactionLines = new List<Transaction>();
        
        using var stream = getObjectResponse.ResponseStream;
        var sr = new StreamReader(stream);

        if (sr.EndOfStream)
            return new(Guid.NewGuid(), Enumerable.Empty<Transaction>(), string.Empty, string.Empty, string.Empty);

        sr.ReadLine();

        while (!sr.EndOfStream)
        {
            var line = await sr.ReadLineAsync(cancellationToken);
            transactionLines.Add(TransactionFactory.Create(line));
        }

        return new Statement(Guid.NewGuid(), transactionLines, getObjectResponse.Key, Constants.Tesco, Constants.MasterCard);
    }
}

public interface IStatementFactory
{
    Task<Statement> CreateAsync(GetObjectResponse getObjectResponse, CancellationToken cancellationToken);
}