using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ProductsApi.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var contentRoot = FindProjectDirectoryWithAppSettings();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(contentRoot)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        var rawConnectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                $"Design-time config missing ConnectionStrings:DefaultConnection. Content root: {contentRoot}");
        var connectionString = SqliteConnectionResolver.Resolve(rawConnectionString, contentRoot);
        optionsBuilder.UseSqlite(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    /// <summary>
    /// Design-time cwd is not always the project folder; locate the directory that contains appsettings.json.
    /// </summary>
    private static string FindProjectDirectoryWithAppSettings()
    {
        var candidates = new[]
        {
            Directory.GetCurrentDirectory(),
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "",
            AppContext.BaseDirectory,
        };

        foreach (var start in candidates)
        {
            if (string.IsNullOrWhiteSpace(start))
                continue;

            var dir = new DirectoryInfo(Path.GetFullPath(start));
            while (dir is not null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "appsettings.json")))
                    return dir.FullName;

                dir = dir.Parent;
            }
        }

        return Directory.GetCurrentDirectory();
    }
}
