using Amazon.DynamoDBv2;
using Amazon.Lambda.Annotations;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TescoStatementHandler.Factories;

namespace TescoStatementProcessorLambda;

[LambdaStartup]
public class StartUp(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddDefaultAWSOptions(configuration.GetAWSOptions());
        serviceCollection.AddAWSService<IAmazonS3>();
        serviceCollection.AddAWSService<IAmazonDynamoDB>();
        serviceCollection.AddScoped<IStatementProcessor, StatementProcessor>();
        serviceCollection.AddScoped<IStatementFactory, StatementFactory>();
    }
}
