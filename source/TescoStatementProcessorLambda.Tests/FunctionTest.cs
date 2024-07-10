using Xunit;
using Amazon.Lambda.TestUtilities;
using System.Text.Json;
using FluentAssertions;
using TescoStatementProcessorLambda.Dtos;
using Moq;

namespace TescoStatementProcessorLambda.Tests;

public class FunctionTest
{
    [Fact(Skip = "Used for setup")]
    public async Task TestToUpperFunctionAsync()
    {
        // Invoke the lambda function and confirm the string was upper cased.
        CancellationToken cancellationToken = new();
        var statementProcessor = new Mock<IStatementProcessor>();
        statementProcessor.Setup(sp => sp.ProcessAsync(It.IsAny<Event>(), cancellationToken));
        var function = new Function();
        var context = new TestLambdaContext();

        var @event = await JsonSerializer.DeserializeAsync<Event>(File.OpenRead(Path.Join(Environment.CurrentDirectory, "event.json")), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        var echo = function.FunctionHandlerAsync(statementProcessor.Object, @event, context, cancellationToken);

        echo.Should().BeEquivalentTo(@event);
    }
}
