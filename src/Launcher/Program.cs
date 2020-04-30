namespace Launcher {
	using System;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Quilt;

	class Program {
		static void Main(string[] args) {
			var services = new ServiceCollection();
			services.AddLogging(builder => {
				builder.SetMinimumLevel(LogLevel.Debug);
				builder.AddConsole();
			});

			services.AddSingleton(p => new ModuleManager(p.GetRequiredService<ILogger<ModuleManager>>(), "."));

			var serviceProvider = services.BuildServiceProvider();

			var moduleManager = serviceProvider.GetService<ModuleManager>();

			Console.WriteLine("Hello World!");
		}
	}
}
