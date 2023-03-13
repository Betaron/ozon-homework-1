using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services;
using Route256.Week1.Homework.PriceCalculator.Api.Bll.Services.Interfaces;
using Route256.Week1.Homework.PriceCalculator.Api.Conventions;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories;
using Route256.Week1.Homework.PriceCalculator.Api.Dal.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers(options =>
{
	options.Conventions.Add(
		new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
})
	.AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(o =>
{
	o.CustomSchemaIds(x => x.FullName);
});
services.AddScoped<IPriceCalculatorService, PriceCalculatorService>();
services.AddScoped<IAnalyticsService, AnalyticsService>();
services.AddSingleton<IStorageRepository, StorageRepository>();


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
