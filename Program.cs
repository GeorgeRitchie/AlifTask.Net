using AlifTask.Common.Extensions;
using AlifTask.Common.Extensions.CustomAuthenticator;
using AlifTask.Persistence;
using AlifTask.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Custom X-UserId and X-Digest authentication
builder.Services.AddAuthentication(opt =>
{
	opt.DefaultScheme = ConstValues.CustomXDigestAuthenticationSchemeName;
})
	.AddXDigest(ConstValues.CustomXDigestAuthenticationSchemeName);

// Adding Db
var connectionString = builder.Configuration["DefaultDbConnection"]
								?.Replace("[DataDirectory]", Directory.GetCurrentDirectory())
								?? throw new InvalidOperationException("Connection string to Db is required");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

// Adding Serilog
// WARNING see SerilogExtensions.cs before running db migrations
builder.Services.AddSerilog(connectionString);

// Adding FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

#region Adding Custom Services

builder.Services.AddScoped<WalletOperationService>();

#endregion

var app = builder.Build();

InitializeAppAsync(app).Wait();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task InitializeAppAsync(WebApplication app)
{
	var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

	await DbInitializer.InitializeAsync(db);
}