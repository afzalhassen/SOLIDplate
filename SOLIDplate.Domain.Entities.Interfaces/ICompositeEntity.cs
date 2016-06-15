namespace SOLIDplate.Domain.Entities.Interfaces
{
    public interface ICompositeEntity<TIdentityEntity1, TIdentityEntity2>
		where TIdentityEntity1 : class, new()
		where TIdentityEntity2 : class, new()
	{
	}

	public interface ICompositeEntity<TIdentityEntity1, TIdentityEntity2, TIdentityEntity3>
		where TIdentityEntity1 : class, new()
		where TIdentityEntity2 : class, new()
		where TIdentityEntity3 : class, new()
	{
	}
}