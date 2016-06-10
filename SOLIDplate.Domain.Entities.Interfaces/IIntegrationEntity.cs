namespace SOLIDplate.Domain.Entities.Interfaces
{
	public interface IIntegrationEntity<TEntity, TKeyType>
		where TEntity : class, IIntegrationEntity<TEntity, TKeyType>, new()
	{
		/// <summary>
		/// Gets or sets the Key of the entity.
		/// </summary>
		/// <value>
		/// The value of the Key of type TKeyType.
		/// </value>
		TKeyType Key { get; set; }
	}
}