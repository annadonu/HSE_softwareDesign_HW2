using Microsoft.Extensions.DependencyInjection;
using POS.Infrastructure;
using POS.Presentation;
using POS.UseCases;

var services = new ServiceCollection();

services.AddUseCases();
services.AddInfrastructure();
services.AddSingleton<POSFacade>();
services.AddSingleton<DemoScenario>();
services.AddSingleton<ConsoleMenu>();

using var serviceProvider = services.BuildServiceProvider();
var menu = serviceProvider.GetRequiredService<ConsoleMenu>();
menu.Run();
