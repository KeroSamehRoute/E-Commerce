using E_Commerce.API.Extensions;
using E_Commerce.Application;
using E_Commerce.Application.Profiles;
using E_Commerce.Application.Services;
using E_Commerce.Infrastructure;
using E_Commerce.Infrastructure.Identity.Services;
using Microsoft.Extensions.FileProviders;

namespace E_Commerce.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Services

        builder.Services.AddControllers();

        builder.Services.AddInfrastructureServices(builder.Configuration);

        builder.Services.AddApplicationServices();

        builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));

        builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

        builder.Services.Configure<PaymentGatewaySettings>(builder.Configuration.GetSection("Stripe"));

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen();


        var app = builder.Build();

        await app.SeedAndMigrateDataAsync();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "Files")),
            RequestPath = "/Files"
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }

}
