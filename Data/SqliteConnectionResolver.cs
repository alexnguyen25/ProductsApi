namespace ProductsApi.Data;

public static class SqliteConnectionResolver
{
    /// <summary>
    /// Resolves a relative SQLite file in <paramref name="rootDirectory"/> to an absolute path
    /// so the database file location does not depend on the process working directory.
    /// </summary>
    public static string Resolve(string? connectionString, string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        const string prefix = "Data Source=";
        if (!connectionString.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            return connectionString;

        var path = connectionString[prefix.Length..].Trim();
        if (Path.IsPathRooted(path))
            return connectionString;

        var absolute = Path.GetFullPath(Path.Combine(rootDirectory, path));
        return $"{prefix}{absolute}";
    }
}
