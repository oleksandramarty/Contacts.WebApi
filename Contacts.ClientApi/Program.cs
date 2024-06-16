using Autofac;
using Autofac.Extensions.DependencyInjection;
using Contacts.ClientApi;
using Contacts.Domain;
using Contacts.Mapper;
using Contacts.Mediatr;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Contacts")));

        var allowedSpecificOriginsPolicy = "_AllowedSpecificOriginsPolicy";

        var origin = builder.Configuration.GetValue<string>("Origin");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(allowedSpecificOriginsPolicy, builder =>
            {
                builder.WithOrigins(origin.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services.AddHealthChecks();
        builder.Services.AddResponseCompression();
        builder.Services.AddResponseCaching();

        builder.Services.AddAutoMapper(config => config.AddProfile(new MappingProfile()));

        builder.Services.AddMvc(mvcOptions =>
            {
                mvcOptions.CacheProfiles.Add("OneHour",
                    new CacheProfile
                    {
                        Duration = 3600,
                        Location = ResponseCacheLocation.Any
                    });
                mvcOptions.CacheProfiles.Add("FiveMinutes",
                    new CacheProfile
                    {
                        Duration = 300,
                        Location = ResponseCacheLocation.Any
                    });
                mvcOptions.CacheProfiles.Add("Week",
                    new CacheProfile
                    {
                        Duration = 604800,
                        Location = ResponseCacheLocation.Any
                    });
                mvcOptions.CacheProfiles.Add("Month",
                    new CacheProfile
                    {
                        Duration = 2419200,
                        Location = ResponseCacheLocation.Any
                    });
                mvcOptions.AllowEmptyInputInBodyModelBinding = false;
            }).AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters();

        builder.Services.AddRouting(option => option.LowercaseUrls = true);

        builder.AddFluentValidations();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.AddDependencyInjection();

        builder.AddGraphQL();

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        builder.Host.ConfigureContainer<ContainerBuilder>(opts => { opts.RegisterModule(new MediatorModule()); });

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.UseGraphQLPlayground("/graphql/playground");
        else
            app.UseHsts();
        
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        app.UpdateDatabaseAsync().Wait(cancellationToken);

        app.UseHttpsRedirection();
        app.UseResponseCompression();
        app.UseResponseCaching();

        app.UseRouting();

        app.ConfigureApplicationLocalization();

        app.UseCors(allowedSpecificOriginsPolicy);

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.UseGraphQL();

        app.Run();
    }
}