using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(Name = "LookupEntity", IsReference = true)]
	public abstract class LookupEntity<TEntity> : Entity<TEntity>
		where TEntity : LookupEntity<TEntity>, new()
	{
		[DataMember]
		public string Value { get; set; }
	}
}