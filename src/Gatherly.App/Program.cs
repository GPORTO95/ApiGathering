using Gatherly.App.Configuration;
using Gatherly.App.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

/// METODO 1
/// É necessário chamar todos os metodos a cada implementação
/// na classe static DependecyInjection
#region :: MÉTODO 1 ::
//builder.Services
//    .AddCaching(builder.Configuration)
//    .AddInfrastructure(builder.Configuration)
//    .AddApplication()
//    .AddBackgroundJobs()
//    .AddPresentation()
//    .AddAuthenticationAndAuthorization();
#endregion

/// MÉTODO 2
/// Uma vez configurado, basta criar suas devidas classes 
builder.Services
    .InstallServices(
        builder.Configuration,
        typeof(IServiceInstaller).Assembly);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
