using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
    [DataContract(IsReference = true)]
	public class Person : Entity<Person>
	{
		[DataMember]
		public string FirstNames { get; set; }
		[DataMember]
		public string LastName { get; set; }
	}
}