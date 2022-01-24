using System;
using Microsoft.EntityFrameworkCore;

namespace br.com.waltercoan.azfuncisolated;
public class FuncDbContext : DbContext
{
    public virtual DbSet<PhotoItem> PhotoItens { get; set; }
    public FuncDbContext(DbContextOptions<FuncDbContext> options)
        : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseCosmos(accountEndpoint: Environment.GetEnvironmentVariable("COSMOSDB_ACCOUNTENDPOINT"),
                                    accountKey: Environment.GetEnvironmentVariable("COSMOSDB_ACCOUNTKEY"),
                                    databaseName: Environment.GetEnvironmentVariable("COSMOSDB_DATABASENAME")
                                    );
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhotoItem>()
            .ToContainer("PhotoItem");
        modelBuilder.Entity<PhotoItem>()
            .HasKey(o => o.RowKey);
        modelBuilder.Entity<PhotoItem>()
            .HasPartitionKey(o => o.RowKey);
    }
}

