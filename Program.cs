using AlifTask.Common.Extensions.CustomAuthenticator;
using AlifTask.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(opt =>
{
	opt.DefaultScheme = ConstValues.CustomXDigestAuthenticationSchemeName;
})
	.AddXDigest(ConstValues.CustomXDigestAuthenticationSchemeName);

var connectionString = builder.Configuration["DefaultDbConnection"]
								?.Replace("[DataDirectory]", Directory.GetCurrentDirectory())
								?? throw new InvalidOperationException("Connection string to Db is required");
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));

var app = builder.Build();

InitializeAppAsync(app).Wait();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


static async Task InitializeAppAsync(WebApplication app)
{
	var db = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

	await DbInitializer.InitializeAsync(db);
}