using Microsoft.Extensions.DependencyInjection;
using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Evaluations;
using POS.UseCases.Reports;
using POS.UseCases.Students;

namespace POS.UseCases;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddSingleton<EvaluationFactory>();
        services.AddSingleton<GradeCalculationService>();
        services.AddSingleton<IGradeCalculationStrategy, GradedCriteriaStrategy>();
        services.AddSingleton<IGradeCalculationStrategy, MissingAsZeroStrategy>();
        services.AddSingleton<GradeStrategyResolver>();

        services.AddTransient<AddStudentHandler>();
        services.AddTransient<GetStudentsHandler>();
        services.AddTransient<RemoveStudentHandler>();
        services.AddTransient<AddCriterionHandler>();
        services.AddTransient<GetCriteriaHandler>();
        services.AddTransient<RemoveCriterionHandler>();
        services.AddTransient<SetEvaluationHandler>();
        services.AddTransient<UpdateEvaluationHandler>();
        services.AddTransient<RemoveEvaluationHandler>();
        services.AddTransient<GetEvaluationsHandler>();
        services.AddTransient<GetGradeModesHandler>();
        services.AddTransient<CalculateGradeHandler>();
        services.AddTransient<ExportGradesHandler>();

        return services;
    }
}
