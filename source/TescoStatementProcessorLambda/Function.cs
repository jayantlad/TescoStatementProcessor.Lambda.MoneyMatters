using Amazon.Lambda.Core;
using TescoStatementProcessorLambda.Dtos;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TescoStatementProcessorLambda;

internal class Function(IStatementProcessor statementProcessor)
{
    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandlerAsync(Event input, ILambdaContext context, CancellationToken cancellationToken)
    {
        await statementProcessor.ProcessAsync(input, cancellationToken);
    }
}