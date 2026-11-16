using Microsoft.Extensions.DependencyInjection;
using POS.Infrastructure.Export;
using POS.Infrastructure.Persistence;
using POS.UseCases.Criteria;
using POS.UseCases.Reports;
using POS.UseCases.Students;

namespace POS.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<JsonStorage>();
        services.AddSingleton<IStudentRepository, JsonStudentRepository>();
        services.AddSingleton<IRubricRepository, JsonRubricRepository>();
        services.AddSingleton<IGradeExporter, CsvGradeExporter>();
        return services;
    }
}
