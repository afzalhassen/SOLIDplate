using System;

namespace SOLIDplate.Domain.Entities.Interfaces
{
	public interface IEntity<TEntity> : IEquatable<TEntity>
		   where TEntity : class, IEntity<TEntity>, new()
	{
		/// <summary>
		/// Gets or sets the Id of the entity.
		/// </summary>
		/// <value>
		/// The id.
		/// </value>
		int Id { get; set; }
	}
}
