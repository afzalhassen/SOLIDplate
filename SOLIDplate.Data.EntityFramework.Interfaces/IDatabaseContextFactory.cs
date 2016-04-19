namespace SOLIDplate.Data.EntityFramework.Interfaces
{
	public interface IDatabaseContextFactory
	{
		IDatabaseContext CreateDatabaseContext();
	}
}
