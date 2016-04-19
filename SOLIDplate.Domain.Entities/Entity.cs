using SOLIDplate.Domain.Entities.Interfaces;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
	[DataContract(Name = "Entity", IsReference = true)]
	public abstract class Entity<TEntity> : IEntity<TEntity>
		where TEntity : Entity<TEntity>, new()
	{
		[DataMember]
		public int Id { get; set; }

		/// <summary>
		/// Indicates whether the current TEntity is equal to another TEntity.
		/// </summary>
		/// <param name="other">
		/// A TEntity to compare with this TEntity.</param>
		/// <returns>
		/// True if the current TEntity has the same Id as the <paramref name="other"/> TEntity parameter; otherwise, false.
		/// </returns>
		public bool Equals(TEntity other)
		{
			return other != null && Id == other.Id;
		}
	}
}