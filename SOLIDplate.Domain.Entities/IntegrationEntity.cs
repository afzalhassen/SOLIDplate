using SOLIDplate.Domain.Entities.Interfaces;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    /// <summary>
    /// Any type the implements this class must be a LookupEntity that comes from a remote 
    /// service that does not exist in the context of this applications problem domain.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type that is implementing this class
    /// </typeparam>
    /// <typeparam name="TKeyType">
    /// The type of the Unique/Primary Key that can be used 
    /// to lookup the details of <typeparamref name="TEntity"/>.
    /// </typeparam>
    [DataContract(Name = "IntegrationEntity", IsReference = true)]
	public abstract class IntegrationEntity<TEntity, TKeyType> : IIntegrationEntity<TEntity, TKeyType>
		where TEntity : IntegrationEntity<TEntity, TKeyType>, new()
	{
		[DataMember]
		public TKeyType Key { get; set; }
	}
}