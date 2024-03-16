using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace AlifTask.Common.Extensions
{
	public static class SerilogExtensions
	{
		public static IServiceCollection AddSerilog(this IServiceCollection services, string dbConnectionString)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Is(LogEventLevel.Information)
				// TODO __!!__ sometimes MSSqlServer sink throws exception when db file is not created,
				// that is why just create db (example: aliftask.mdf) via MS SSMS, then run migrations
				.WriteTo.MSSqlServer(dbConnectionString, new MSSqlServerSinkOptions() { AutoCreateSqlDatabase = false, AutoCreateSqlTable = true, TableName = "Logs" })
				.WriteTo.File(Path.Combine(Environment.CurrentDirectory, "Logs", "LogFiles/ProgramLog-.txt"), rollingInterval: RollingInterval.Day)
				.CreateLogger();

			services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger));

			return services;
		}
	}
}
