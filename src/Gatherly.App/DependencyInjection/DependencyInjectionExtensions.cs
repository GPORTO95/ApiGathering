﻿using FluentValidation;
using Gatherly.App.OptionsSetup;
using Gatherly.Application.Abstractions;
using Gatherly.Application.Behaviors;
using Gatherly.Domain.Repositories;
using Gatherly.Domain.Shared;
using Gatherly.Infrastructure.Authentication;
using Gatherly.Infrastructure.BackgroundJobs;
using Gatherly.Persistence;
using Gatherly.Persistence.Interceptors;
using Gatherly.Persistence.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Gatherly.Infrastructure.Services;

namespace Gatherly.App.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services)
    {

        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddAuthorization();

        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        return services;
    }

    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Gatherly.Presentation.AssemblyReference.Assembly);

        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(100)
                                        .RepeatForever()));
        });

        services.AddQuartzHostedService();

        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Gatherly.Application.AssemblyReference.Assembly);

            config.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        services.AddValidatorsFromAssembly(
            Gatherly.Application.AssemblyReference.Assembly,
            includeInternalTypes: true);

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //services.AddScoped<IMemberRepository, MemberRepository>();
        //services.Decorate<IMemberRepository, CachingMemberRepository>();

        //services.Scan(selector => selector.FromAssemblies(
        //        Infrastructure.AssemblyReference.Assembly,
        //        Persistence.AssemblyReference.Assembly)
        //    .AddClasses(false)
        //    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        //    .AsMatchingInterface()
        //    .WithScopedLifetime());

        //return services;

         services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IAttendeeRepository, AttendeeRepository>();
        services.AddScoped<IGatheringRepository, GatheringRepository>();
        services.AddScoped<IInvitationRepository, InvitationRepository>();

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ISystemTimeProvider, SystemTimeProvider>();

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database");

        Ensure.NotNullOrWithSpace(connectionString);

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        services.AddDbContext<ApplicationDbContext>(
            (sp, optionsBuilder) => optionsBuilder.UseSqlServer(connectionString));

        services.AddMemoryCache();

        return services;
    }
}
