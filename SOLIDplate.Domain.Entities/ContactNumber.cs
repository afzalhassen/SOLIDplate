using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
	[DataContract(IsReference = true)]
	public class ContactNumber : Entity<ContactNumber>
	{
		[DataMember]
		public string Number { get; set; }
		[DataMember]
		public TypeOfContactNumber TypeOfContactNumber { get; set; }
	}
}