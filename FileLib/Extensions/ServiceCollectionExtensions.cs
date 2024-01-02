using Amazon.S3;
using Filio.FileLib.Settings.Aws;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Filio.FileLib.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register FileLib services and settings
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddFileLib(this IServiceCollection services, IConfiguration configuration)
    {
        var awsSettingsSection = configuration.GetSection(nameof(AwsSettings));
        var awsSettings = awsSettingsSection.Get<AwsSettings>();
        services.Configure<AwsSettings>(awsSettingsSection);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<AwsSettings>>().Value);

        var s3ClientConfig = new AmazonS3Config
        {
            ServiceURL = awsSettings.ServiceUrl,
            MaxErrorRetry = awsSettings.MaxErrorRetry,
            ForcePathStyle = true,
        };

        services.AddSingleton(
                new AmazonS3Client(awsAccessKeyId: awsSettings.AccessKey,
                 awsSecretAccessKey: awsSettings.SecretKey,
                  s3ClientConfig));

        services.AddScoped<IFileService, S3FileService>();

        return services;
    }
}