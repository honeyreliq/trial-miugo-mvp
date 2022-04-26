using System;
using IUGOCare.Audit.Infrastructure;
using IUGOCare.Audit.Infrastructure.Wrappers;
using IUGOCare.Audit.Persistence;
using IUGOCare.Audit.Repositories;
using IUGOCare.Audit.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IUGOCare.Audit
{
    public interface IAuditDbContextFactory
    {
        IAuditDbContext AuditDbContext { get; }
    }

    internal sealed class AuditDbContextFactory : IAuditDbContextFactory
    {
        private readonly IServiceProvider _provider;

        public AuditDbContextFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IAuditDbContext AuditDbContext => _provider.GetService<IAuditDbContext>();
    }

    public static class DependencyInjection
    {
        public static IServiceCollection AddAudit(this IServiceCollection services, IConfiguration configuration)
        {
            string apiAuditAzureStorageConnectionString = configuration.GetSection("ApiAuditAzureStorageConnectionString").Get<string>();
            string webUriAuthority = configuration.GetSection("WebUriAuthority").Get<string>();

            services.AddDbContext<AuditDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AuditDbContext).Assembly.FullName)
                ),
                ServiceLifetime.Transient
            );

            services.AddTransient<IAuditDbContext>(provider => provider.GetService<AuditDbContext>());
            services.AddTransient<IAuditDbContextFactory, AuditDbContextFactory>();

            services.AddSingleton<IAuditService, AuditService>();

            if (!string.IsNullOrWhiteSpace(apiAuditAzureStorageConnectionString))
            {
                services.AddSingleton<IApiAuditRepository, AzureBlobStorageApiAuditRepository>();

                var clientIdentifier = new Uri(webUriAuthority).Host;
                var containerPrefix = clientIdentifier.Split('.')[0];
                var container = GetCloudBlobContainer(apiAuditAzureStorageConnectionString, containerPrefix);

                services.AddSingleton<ICloudBlobContainer>(new CloudBlobContainerWrapper(container));
                services.AddSingleton<IAzureBlobStorageUtilities, AzureBlobStorageUtilities>();
                services.AddSingleton<IAzureBlobNameGenerator, ApiAuditAzureBlobNameGenerator>();
                services.AddSingleton<IApiAuditFailoverRepository, EFApiAuditRepository>();

                services.AddSingleton<IApiAuditAzureBlobStorageStrategy>(provider =>
                    new ApiAuditAppendBlobAzureBlobStorageStrategy(
                        provider.GetService<IAzureBlobNameGenerator>(),
                        provider.GetService<IAzureBlobStorageUtilities>(),
                        provider.GetService<IApiAuditFailoverRepository>(),
                        clientIdentifier));
            }
            else
            {
                services.AddTransient<IApiAuditRepository, EFApiAuditRepository>();
            }

            return services;
        }

        private static CloudBlobContainer GetCloudBlobContainer(string connectionString, string containerPrefix)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var containerName = $"{containerPrefix}-api-audit-logs";

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            if (!container.Exists())
                container.CreateIfNotExists(BlobContainerPublicAccessType.Off); // Off = Private (no anonymous access)

            return container;
        }
    }
}
