using System.Globalization;
using Contacts.Domain;
using Contacts.GraphQL;
using Contacts.Interfaces;
using Contacts.Mediatr.Validators.Contacts;
using Contacts.Repositories;
using FluentValidation;
using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.ClientApi;

public static class WebAppExtension
{
    public static async Task UpdateDatabaseAsync(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices
                   .GetRequiredService<IServiceScopeFactory>()
                   .CreateScope())
        {
            using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
            {
                await context.Database.MigrateAsync();
            }
        }
    }
    
    public static void AddFluentValidations(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<CreateOrUpdateContactCommandValidator>();
    }

    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped(typeof(IFilterProvider<>), typeof(FilterProvider<>));
        builder.Services.AddScoped<IEntityValidator, EntityValidator>();
        builder.Services.AddScoped(typeof(IReadGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
    }

    public static void AddGraphQL(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISchema, GraphQLSchema>(services =>
            new GraphQLSchema(new SelfActivatingServiceProvider(services)));

        builder.Services.AddGraphQL(options =>
            options.ConfigureExecution((opt, next) =>
            {
                opt.EnableMetrics = true;
                opt.ThrowOnUnhandledException = true;
                return next(opt);
            }).AddSystemTextJson()
        );
    }

    public static void ConfigureApplicationLocalization(this IApplicationBuilder app)
    {
        var options = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture),
            SupportedCultures = new[] { CultureInfo.InvariantCulture },
            SupportedUICultures = new[] { CultureInfo.InvariantCulture }
        };

        // Remove any existing culture providers to avoid conflicts
        options.RequestCultureProviders.Clear();

        // Add your preferred culture provider (e.g., AcceptLanguageHeaderRequestCultureProvider or CookieRequestCultureProvider)
        options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());

        // UseRequestLocalization with the configured options
        app.UseRequestLocalization(options);
    }
}