using ShoppingLikeFlies.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.AddServices();

await app.UseDatabase();

app.UsePipeline();

app.Run();
