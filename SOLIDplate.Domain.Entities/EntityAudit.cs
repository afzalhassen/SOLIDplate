using System;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(IsReference = true)]
	public class EntityAudit : Entity<EntityAudit>
	{
		[DataMember]
		public DateTime DateTime { get; set; }
		[DataMember]
		public string ActionType { get; set; }
		[DataMember]
		public string ActionedBy { get; set; }
		[DataMember]
		public string TableName { get; set; }
		[DataMember]
		public string EntityId { get; set; }
		[DataMember]
		public string PropertyName { get; set; }
		[DataMember]
		public string OldValue { get; set; }
		[DataMember]
		public string NewValue { get; set; }
	}
}
