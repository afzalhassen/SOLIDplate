using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(IsReference = true)]
	public class EntityQueryCategory : LookupEntity<EntityQueryCategory>
	{
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public int DisplayIndex { get; set; }
	}
}