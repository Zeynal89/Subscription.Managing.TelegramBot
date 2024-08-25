public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        // Configure your connection string or other options here
        optionsBuilder.UseSqlite("Data Source=app.db");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
