using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(IsReference = true)]
	public class EntityQuery : Entity<EntityQuery>
	{
	    [DataMember]
		public string Name { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public virtual EntityQueryCategory EntityQueryCategory { get; set; }
		[DataMember]
		public virtual ICollection<EntityPropertyFilter> EntityPropertyFilters { get; set; } = new HashSet<EntityPropertyFilter>();

	    // This property must never be serialised as it exists merely for EntityFramewWork persistence
		[IgnoreDataMember]
		public virtual string EntityTypeSerialised { get; set; }
		[DataMember]
		public virtual Type EntityType
		{
			get { return Type.GetType(EntityTypeSerialised); }
			set { EntityTypeSerialised = value.ToString(); }
		}

		public int EntityQueryCategoryId { get; set; }
	}
}