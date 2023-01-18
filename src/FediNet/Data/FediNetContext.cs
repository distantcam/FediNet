using Microsoft.EntityFrameworkCore;

namespace FediNet.Data;

// https://medium.com/analytics-vidhya/entityframework-core-dont-get-burnt-in-production-335ddfcfdfda
// https://github.com/NMillard/EFCoreLearnings

public class FediNetContext : DbContext
{
    public FediNetContext(DbContextOptions options) : base(options)
    {
    }
}
