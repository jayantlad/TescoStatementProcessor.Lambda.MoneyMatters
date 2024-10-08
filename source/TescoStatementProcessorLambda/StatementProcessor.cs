﻿using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using TescoStatementHandler.Factories;
using TescoStatementProcessorLambda.Dtos;

namespace TescoStatementProcessorLambda;

internal sealed class StatementProcessor(IStatementFactory statementFactory,
        ILogger<StatementProcessor> logger,
        IAmazonS3 amazonS3Client,
        IStatementRespository statementRespository,
        ITransactionRespository transactionRespository) : IStatementProcessor
{
    public async Task ProcessAsync(Event @event, CancellationToken cancellationToken)
    {
        GetObjectRequest getObjectRequest = new()
        {
            BucketName = @event.Detail.Bucket.Name,
            Key = @event.Detail.Object.Key
        };

        var getObjectResponse = await amazonS3Client.GetObjectAsync(getObjectRequest, cancellationToken);

        var statement = await statementFactory.CreateAsync(getObjectResponse, cancellationToken);
        await statementRespository.SaveStatementAsync(statement.Statement, cancellationToken);
        await transactionRespository.SaveTransactionsAsync(statement.Transactions, cancellationToken);
    }
}

public interface IStatementProcessor
{
    Task ProcessAsync(Event @event, CancellationToken cancellationToken);
}