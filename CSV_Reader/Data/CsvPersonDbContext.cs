using Microsoft.EntityFrameworkCore;
using CSV_Reader.Models;

namespace CSV_Reader.Data;

public class CsvPersonDbContext:DbContext
{
    public CsvPersonDbContext(DbContextOptions<CsvPersonDbContext> options) : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>()
            .Property(e => e.Salary)
            .HasPrecision(7, 2);
    }
}
