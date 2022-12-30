using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace FediNet;

// https://medium.com/analytics-vidhya/entityframework-core-dont-get-burnt-in-production-335ddfcfdfda
// https://github.com/NMillard/EFCoreLearnings

public class FediNetContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FediNetContext" /> class using the specified options.
    /// The <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" /> method will still be called to allow further
    /// configuration of the options.
    /// </summary>
    /// <remarks>
    /// See <see href="https://aka.ms/efcore-docs-dbcontext">FediNetContext lifetime, configuration, and initialization</see> and
    /// <see href="https://aka.ms/efcore-docs-dbcontext-options">Using DbContextOptions</see> for more information and examples.
    /// </remarks>
    /// <param name="options">The options for this context.</param>
    [RequiresUnreferencedCode("EF Core isn't fully compatible with trimming, and running the application may generate unexpected runtime failures. Some specific coding pattern are usually required to make trimming work properly, see https://aka.ms/efcore-docs-trimming for more details.")]
    public FediNetContext(DbContextOptions options) : base(options)
    {
    }
}
