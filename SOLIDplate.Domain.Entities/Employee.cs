using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
	[DataContract(IsReference = true)]
	public class Employee : IntegrationEntity<Employee, int>
	{
		[DataMember]
		public string FirstNames { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string EmailAddress { get; set; }
		[DataMember]
		public virtual ICollection<ContactNumber> ContactNumbers { get; set; }
		[DataMember]
		public string Position { get; set; }
		[DataMember]
		public string AccountName { get; set; }
	}
}