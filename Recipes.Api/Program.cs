using Recipes.Api.Middlewares;
using Recipes.Application;
using Recipes.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.

    builder.Services.AddApplication().
        AddInfrastructure(builder.Configuration);
    var presentationAssembly = typeof(Recipes.Presentation.AssemblyReference).Assembly;
    builder.Services.AddControllers().AddApplicationPart(presentationAssembly);
    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
    builder.Services.AddMemoryCache();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
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
}
