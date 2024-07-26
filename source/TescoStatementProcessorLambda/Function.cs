using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Core;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AWS.Lambda.Powertools.Logging;
using TescoStatementHandler.Factories;
using TescoStatementProcessorLambda.Dtos;
using Serilog;
using System.Reflection;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace TescoStatementProcessorLambda;

public class Function
{
    private IStatementProcessor _statementProcessor;
    private readonly ILogger<Function> _logger;

    public Function()
    {
        var config = new ConfigurationBuilder()
            .Build();
        var services= new ServiceCollection();
        services.AddDefaultAWSOptions(config.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();
        services.AddAWSService<IAmazonDynamoDB>();
        services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
        services.AddScoped<IStatementProcessor, StatementProcessor>();
        services.AddScoped<IStatementRespository, StatementRespository>();
        services.AddScoped<IStatementFactory, StatementFactory>();
        services.AddLogging(builder => {
            builder.ClearProviders();
            builder.AddConsole();
            builder.AddSerilog();
        });

        var sp = services.BuildServiceProvider();
        _statementProcessor = sp.GetRequiredService<IStatementProcessor>();
        _logger = sp.GetRequiredService<ILogger<Function>>();
    }

    public Function(IStatementProcessor statementProcessor, ILogger<Function> logger)
    {
        _statementProcessor = statementProcessor;
        _logger = logger;
    }


    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input">The event for the Lambda function handler to process.</param>
    /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
    /// <returns></returns>
    public async Task FunctionHandlerAsync(Event input, ILambdaContext context)
    {
        try
        {
            _logger.LogInformation("we are running {event}", input);
            _logger.LogCritical("we are running {event}", input);
            _logger.LogDebug("we are running {event}", input);
            _logger.LogError("we are running {event}", input);
            _logger.LogTrace("we are running {event}", input);
            _logger.LogWarning("we are running {event}", input);
            Logger.LogInformation("we are running {event}", input);
            Logger.LogCritical("we are running {event}", input);
            Logger.LogDebug("we are running {event}", input);
            Logger.LogError("we are running {event}", input);
            Logger.LogTrace("we are running {event}", input);
            Logger.LogWarning("we are running {event}", input);
            await _statementProcessor.ProcessAsync(input, new CancellationToken());
        }
        catch(Exception ex){
            _logger.LogError(ex.ToString());
        }
    }
}