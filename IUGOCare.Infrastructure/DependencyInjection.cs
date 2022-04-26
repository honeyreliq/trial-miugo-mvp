using System;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Infrastructure.Identity;
using IUGOCare.Infrastructure.Messaging;
using IUGOCare.Infrastructure.Messaging.Persistence;
using IUGOCare.Infrastructure.Persistence;
using IUGOCare.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SendGrid;

namespace IUGOCare.Infrastructure
{
    public interface IMessagingDbContextFactory
    {
        IMessagingDbContext MessagingDbContext { get; }
    }

    internal sealed class MessagingDbContextFactory : IMessagingDbContextFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public MessagingDbContextFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessagingDbContext MessagingDbContext => _serviceProvider.GetService<IMessagingDbContext>();
    }
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDbContext<MessagingDbContext>(
                options => options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(MessagingDbContext).Assembly.FullName)
                ),
                ServiceLifetime.Transient
            );
            services.AddTransient<IMessagingDbContext>(provider => provider.GetService<MessagingDbContext>());
            services.AddTransient<IMessagingDbContextFactory, MessagingDbContextFactory>();

            // Service bus
            if (bool.Parse(configuration.GetValue<string>("IsServiceBusEnabled")))
            {
                services.AddSingleton<IServiceBusSender, TopicSender>(sp =>
                {
                    var connectionString = configuration.GetValue<string>("ServiceBusConnectionString");
                    var topic = configuration.GetValue<string>("ServiceBusSenderTopic");
                    ILogger<TopicSender> logger = sp.GetService<ILogger<TopicSender>>();
                    IMessagingDbContextFactory contextFactory = sp.GetService<IMessagingDbContextFactory>();
                    IDateTimeOffset dateTimeOffset = sp.GetService<IDateTimeOffset>();

                    return new TopicSender(connectionString, topic, logger, contextFactory, dateTimeOffset);
                });

                services.AddSingleton<IServiceBusListener, TopicListener>(sp =>
                {
                    var connectionString = configuration.GetValue<string>("ServiceBusConnectionString");
                    var topic = configuration.GetValue<string>("ServiceBusListenerTopic");
                    var subscriptionName = configuration.GetValue<string>("ServiceBusSubscriptionName");
                    var subdomain = configuration.GetValue<string>("Subdomain");
                    var systemFailureNotificationRecipient = configuration.GetValue<string>("SystemFailureNotificationRecipient");
                    ILogger<TopicListener> logger = sp.GetService<ILogger<TopicListener>>();
                    IMessageHandler messageHandler = sp.GetService<IMessageHandler>();
                    IServiceBusSender messageSender = sp.GetService<IServiceBusSender>();
                    IMessagingDbContextFactory contextFactory = sp.GetService<IMessagingDbContextFactory>();
                    ISendEmailService sendEmailService = sp.GetService<ISendEmailService>();

                    return new TopicListener(
                        connectionString,
                        topic,
                        subscriptionName,
                        subdomain,
                        systemFailureNotificationRecipient,
                        logger,
                        messageHandler,
                        messageSender,
                        contextFactory,
                        sendEmailService);
                });
            }
            else
            {
                services.AddSingleton<IServiceBusSender, NullSender>();
                services.AddSingleton<IServiceBusListener, NullListener>();
            }

            services.AddHostedService(sp => sp.GetService<IServiceBusSender>());
            services.AddHostedService(sp => sp.GetService<IServiceBusListener>());
            services.AddScoped<IMessageHandler, MessageHandler>();
            services.AddScoped<ICommandFactory, CommandFactory>();

            var sendGridApiKey = configuration.GetValue<string>("SendGridApiKey");

            services.AddScoped<ISendGridClient>(s => new SendGridClient(new SendGridClientOptions
            {
                ApiKey = sendGridApiKey
            }));
            
            services.AddScoped<ISendEmailService, SendEmailService>(provider =>
             {
                 var logger = provider.GetService<ILogger<SendEmailService>>();
                 var client = provider.GetService<ISendGridClient>();
                 return new SendEmailService(logger, client, configuration);
             });

            // TODO - Add authentication
            //services.AddAuthentication().AddIdentityServerJwt();

            services.AddTransient<IDateTimeOffset, DateTimeOffsetService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IActivationCode, ActivationCodeService>();
            services.AddScoped<IAuth0ManagementApiWrapper, Auth0ManagementApiWrapper>(provider => {
                var logger = provider.GetService<ILogger<Auth0ManagementApiWrapper>>();
                var domain = configuration.GetValue<string>("Auth0:Domain");
                var connection = configuration.GetValue<string>("Auth0:Connection");
                var managementClientId = configuration.GetValue<string>("Auth0ManagementClientId");
                var managementSecret = configuration.GetValue<string>("Auth0ManagementSecret");
                return new Auth0ManagementApiWrapper(logger, domain, connection, managementClientId, managementSecret);
            });

            return services;
        }
    }
}
