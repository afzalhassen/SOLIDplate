using System.Runtime.Serialization;

namespace SOLIDplate.Domain.Entities
{
	[DataContract(IsReference = true)]
	public class Address : Entity<Address>
	{
		[DataMember]
		public string UnitNumber { get; set; }
		[DataMember]
		public string BuildingName { get; set; }
		[DataMember]
		public string StreetName { get; set; }
		[DataMember]
		public string Suburb { get; set; }
		[DataMember]
		public string City { get; set; }
		[DataMember]
		public string PostalCode { get; set; }
		[DataMember]
		public string CountryCode { get; set; }
		[DataMember]
		public string ProvinceCode { get; set; }
	}
}