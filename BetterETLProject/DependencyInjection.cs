using System.Data;
using BetterETLProject.Connection;
using BetterETLProject.DTO;
using BetterETLProject.Extract.CreateTableAdaptor;
using BetterETLProject.Extract.DataConverterAdaptor;
using BetterETLProject.Extract.Factories;
using BetterETLProject.Extract.ImportTableAdaptor;
using BetterETLProject.QueryGeneration;
using BetterETLProject.Transform;
using BetterETLProject.Validation.DTOValidation;
using FluentValidation;
using Npgsql;

namespace BetterETLProject;

public class DependencyInjection
{
    public static void InjectDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IValidator<AggregationDto>, AggregationDtoValidator>();
        serviceCollection.AddTransient<IAggregation, Aggregation>();
        
        serviceCollection.AddSingleton<IValidator<ConditionDto>, ConditionDtoValidator>();
        serviceCollection.AddTransient<ICondition, Condition>();
        
        serviceCollection.AddSingleton<IValidator<ImportDataDto>, ImportDataDtoValidator>();
        serviceCollection.AddSingleton<IFactory<IDataConverter>, DataConverterFactory>();

        serviceCollection.AddTransient<IDbCommand, NpgsqlCommand>();

        serviceCollection.AddTransient<ICreatorConnection, CreatorConnection>();
        serviceCollection.AddSingleton<IFactory<ICreatorTable> , CreateTableFactory>();
        serviceCollection.AddSingleton<IFactory<IImporterTable>, ImportTableFactory>();
        serviceCollection.AddSingleton<IQueryGenerator, QueryGenerator>();

        serviceCollection.AddTransient<IDbDataAdapter, NpgsqlDataAdapter>();

        serviceCollection.AddTransient<CsvDataConverter>();
    }
}