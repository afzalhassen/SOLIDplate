using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
	[DataContract(IsReference = true)]
	public class User : IntegrationEntity<User, string>
	{
	    [DataMember]
		public string FirstNames { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string EmailAddress { get; set; }
		[DataMember]
		public virtual ICollection<string> RoleMemberships { get; set; } = new HashSet<string>();
	}
}