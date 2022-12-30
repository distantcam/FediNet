using Microsoft.EntityFrameworkCore;

namespace FediNet;

public sealed class DatabaseUpgrader
{
    private readonly string _connectionString;

    public DatabaseUpgrader(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void PerformUpgrade(bool create = false)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(_connectionString);

        if (create)
        {
            new DbContext(optionsBuilder.Options).Database.EnsureCreated();
        }

        var db = new FediNetContext(optionsBuilder.Options);
        db.Database.Migrate();
    }
}
