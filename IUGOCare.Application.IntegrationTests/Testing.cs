using Auth0.ManagementApi.Models;
using IUGOCare.API;
using IUGOCare.Application.Common.Interfaces;
using IUGOCare.Domain.Entities;
using IUGOCare.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Respawn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

[SetUpFixture]
public class Testing
{
    public static IConfiguration Configuration { get; private set; }
    private static IServiceScopeFactory _scopeFactory;
    private static Checkpoint _checkpoint;
    private static Mock<IIdentityService> _iis;
    private static string _currentUserId;
    private const string _emailAddress = "testUser@example.com";

    public static readonly Guid AllOnesGuid = Guid.Parse("11111111-1111-1111-1111-111111111111");

    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", true, true)
            .AddJsonFile("appsettings.IntegrationTests.json", true, true)
            .AddUserSecrets(typeof(Testing).Assembly)
            .AddEnvironmentVariables();

        Configuration = builder.Build();

        var startup = new Startup(Configuration);

        var services = new ServiceCollection();

        services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
            w.EnvironmentName == "Development" &&
            w.ApplicationName == "IUGOCare.API"));

        services.AddSingleton(Configuration);

        services.AddLogging();

        startup.ConfigureServices(services);

        // Replace service registration for ICurrentUserService
        // Remove existing registration
        var currentUserServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(ICurrentUserService));

        services.Remove(currentUserServiceDescriptor);

        // Register testing version
        services.AddTransient(provider =>
            Mock.Of<ICurrentUserService>(s => s.UserId == _currentUserId));

        var identityServiceDescriptor = services.FirstOrDefault(d =>
            d.ServiceType == typeof(IIdentityService));

        services.Remove(identityServiceDescriptor);

        services.AddTransient(provider => Mock.Of<ISendEmailService>());

        // Register testing version
        _iis = new Mock<IIdentityService>();
        _iis.Setup(i => i.GetUserNameAsync(It.IsAny<string>())).Returns(Task.FromResult($"auth0|{_currentUserId}"));
        _iis.Setup(i => i.GetUserAsync(It.IsAny<string>())).Returns(Task.FromResult(new User { UserId = _currentUserId, Email = _emailAddress }));
        _iis.Setup(i => i.GetUserAsync("-99")).Returns(Task.FromResult((User)null));
        _iis.Setup(i => i.UpdateUserAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
        _iis.Setup(i => i.ValidateUserAndPassword(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
        services.AddTransient(_ => _iis.Object);

        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

        _checkpoint = new Checkpoint
        {
            TablesToIgnore = new[] { "__EFMigrationsHistory", "CareManagementPrograms" },
        };

        EnsureDatabase();

        AddCareManagementPrograms();
    }

    private void AddCareManagementPrograms()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

        var ccm = context.CareManagementPrograms.FirstOrDefault(cmp => cmp.ShortName == "CCM");
        var rpm = context.CareManagementPrograms.FirstOrDefault(cmp => cmp.ShortName == "RPM");

        if (ccm is null)
        {
            context.Add(new CareManagementProgram { Id = Guid.NewGuid(), Name = "Chronic Care Management", ShortName = "CCM" });
        }

        if (rpm is null)
        {
            context.Add(new CareManagementProgram { Id = Guid.NewGuid(), Name = "Remote Patient Monitoring", ShortName = "RPM" });
        }

        context.SaveChanges();
    }

    private static void EnsureDatabase()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();
        }
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.Add(entity);
            await context.SaveChangesAsync();
        }
    }

    public static async Task<TEntity> FindAsync<TEntity>(Guid id)
        where TEntity : class
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            return await context.FindAsync<TEntity>(id);
        }
    }

    public static async Task<ICollection<TEntity>> GetAll<TEntity>()
        where TEntity : class
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            return await context.Set<TEntity>().ToListAsync();
        }
    }

    public static async Task<TEntity> FindAsync<TEntity>(Guid id, Guid secondId)
        where TEntity : class
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            return await context.FindAsync<TEntity>(id, secondId);
        }
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator.Send(request);
        }
    }

    public static string RunAsDefaultUser()
    {
        return RunAsUser(Guid.NewGuid());
    }

    public static string RunAsUser(Guid patientId)
    {
        _currentUserId = patientId.ToString();
        _iis.Setup(i => i.GetCurrentPatientId()).Returns(Task.FromResult(patientId));
        return _currentUserId;
    }

    public static async Task ResetState()
    {
        await _checkpoint.Reset(Configuration.GetConnectionString("DefaultConnection"));
        _currentUserId = null;
    }
}
