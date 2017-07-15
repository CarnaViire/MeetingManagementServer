using MeetingManagementServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetingManagementServer.Services.EntityFramework
{
    /// <summary>
    /// Entity Framework data store
    /// </summary>
    public class EfDataStore : DbContext
    {
        public DbSet<Partner> Partners { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<AvailableDate> AvailableDates { get; set; }

        public EfDataStore(DbContextOptions<EfDataStore> options)
            : base(options)
        { }
    }
}
