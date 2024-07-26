using Amazon.S3.Model;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using TescoStatementHandler.Factories;
using Xunit;

namespace TescoStatementProcessorLambda.Tests;

public class StatementFactoryTests
{
    private readonly Mock<ILogger<StatementFactory>> _logger;
    private StatementFactory _sut;

    public StatementFactoryTests()
    {
        _logger = new Mock<ILogger<StatementFactory>>();
        _sut = new StatementFactory(_logger.Object);
    }

    [Fact]
    public async Task CreateAsync_SucceedsAsync()
    {
        CancellationToken cancellationToken = new();
        var aStatement = Directory.EnumerateFiles(Path.GetFullPath("./statements")).First();

        var lines = await File.ReadAllLinesAsync(aStatement, cancellationToken);
        using var fileStream = File.OpenRead(aStatement);

        GetObjectResponse getObjectResponse = new()
        {
            ResponseStream = fileStream
        };

        var statement = await _sut.CreateAsync(getObjectResponse, cancellationToken);

        statement.Product.Should().Be(Constants.MasterCard);
        statement.Provider.Should().Be(Constants.Tesco);
        statement.Transactions.Count().Should().Be(lines.Count() - 1);
    }
}
