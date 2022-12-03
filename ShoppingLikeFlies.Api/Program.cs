using ShoppingLikeFlies.Api.Extensions;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();
try
{
    //Using static logger before DI
    Log.Information("Starting WebApi...");

    var app = builder.AddServices();

    await app.UseDatabase();

    app.UseCors(x =>
    {
        x.AllowAnyOrigin();
        x.AllowAnyMethod();
        x.AllowAnyHeader();
    });

    app.UsePipeline();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "WebApi startup failed");
}
finally
{
    Log.CloseAndFlush();
}