namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface IDatabaseContextFactory
    {
        IDatabaseContext CreateDatabaseContext();
    }
}
