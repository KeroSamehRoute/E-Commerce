using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace E_Commerce.Infrastructure.DataSeeding;

internal class CatalogDataSeeder(StoreDbContext dbContext, ILogger<CatalogDataSeeder> logger) : IDataSeeder
{

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task SeedDataAsync(CancellationToken ct = default)
    {
        try
        {
            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync(ct);

            var pendingMigrationsCollection = (PendingMigrations as ICollection<string>) ?? [.. PendingMigrations];

            if (pendingMigrationsCollection.Count > 0)
            {
                await dbContext.Database.MigrateAsync(ct);
            }

            var seedRoot = Path.Combine(AppContext.BaseDirectory, "DataSeed");

            await SeedIfEmptyAsync<ProductBrand, int>(seedRoot, "brands.json", ct);

            await SeedIfEmptyAsync<ProductType, int>(seedRoot, "types.json", ct);

            await SeedIfEmptyAsync<Product, int>(seedRoot, "products.json", ct);

            await SeedIfEmptyAsync<DeliveryMethod, int>(seedRoot, "delivery.json", ct);

            int result = await dbContext.SaveChangesAsync(ct);

            if (result > 0)
                logger.LogInformation("{Count} Rows Added", result);
            else
                logger.LogInformation("Database Already Seeded");

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding data");
        }

    }


    private async Task SeedIfEmptyAsync<T, Tkey>(string rootPath, string fileName, CancellationToken ct) where T : BaseEntity<Tkey>
    {
        if (await dbContext.Set<T>().AnyAsync(ct))
        {
            logger.LogInformation("Table Already Has Data");
            return;
        }

        var filePath = Path.Combine(rootPath, fileName);

        if (!File.Exists(filePath))
        {
            logger.LogWarning("File {FileName} does not exist", fileName);
            return;
        }

        using var fileStream = File.OpenRead(filePath);

        var items = await JsonSerializer.DeserializeAsync<List<T>>(fileStream, s_jsonOptions, ct);

        if (items is not null && items.Count > 0)
        {
            dbContext.Set<T>().AddRange(items);
        }

    }

}
