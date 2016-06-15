using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(IsReference = true)]
	public class EntityPropertyFilter : Entity<EntityPropertyFilter>
	{
		public EntityPropertyFilter()
		{
			Values = new HashSet<string>();
		}
		[DataMember]
		public virtual string Name { get; set; }
		[DataMember]
		public virtual LogicalOperator LogicalOperator { get; set; }
		[DataMember]
		public virtual ComparisonOperator ComparisonOperator { get; set; }
		// This property must never be serialised as it exists merely for EntityFramewWork persistence
		[IgnoreDataMember]
		public virtual string ValueTypeSerialised { get; set; }
		[DataMember]
		public virtual Type ValueType
		{
			get { return Type.GetType(ValueTypeSerialised); }
			set { ValueTypeSerialised = value.ToString(); }
		}
		// This property must never be serialised as it exists merely for EntityFramewWork persistence
		[IgnoreDataMember]
		public virtual string ValuesSerialised { get; set; }
		[DataMember]
		public virtual ICollection<string> Values
		{
			get { return JsonConvert.DeserializeObject<ICollection<string>>(ValuesSerialised); }
			set { ValuesSerialised = JsonConvert.SerializeObject(value); }
		}

		public int EntityQueryId { get; set; }
	}
}