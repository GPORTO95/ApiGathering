using FluentValidation;
using Gatherly.Application.Behaviors;
using Gatherly.Infrastructure.BackgroundJobs;
using Gatherly.Infrastructure.Idempotence;
using Gatherly.Persistence;
using Gatherly.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Scan(
        selector => selector
        .FromAssemblies(
            Gatherly.Infrastructure.AssemblyReference.Assembly,
            Gatherly.Persistence.AssemblyReference.Assembly)
        .AddClasses(false)
        .AsImplementedInterfaces()
        .WithScopedLifetime());

builder.Services.AddMediatR(Gatherly.Application.AssemblyReference.Assembly);

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.AddValidatorsFromAssembly(Gatherly.Application.AssemblyReference.Assembly,
    includeInternalTypes: true);

builder.Services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

string connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, optionsBuilder) =>
    {
        var interceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

        optionsBuilder.UseSqlServer(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
            .AddInterceptors(interceptor);
    });

builder.Services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(
            trigger =>
                trigger.ForJob(jobKey)
                    .WithSimpleSchedule(
                        schedule =>
                            schedule.WithIntervalInSeconds(10)
                                .RepeatForever()));

    configure.UseMicrosoftDependencyInjectionJobFactory();
});

builder.Services.AddQuartzHostedService();

builder
    .Services
    .AddControllers()
    .AddApplicationPart(Gatherly.Presentation.AssemblyReference.Assembly);

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();