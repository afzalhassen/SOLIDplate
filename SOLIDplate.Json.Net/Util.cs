using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SOLIDplate.Json.Net
{
    public static class Util
	{
		public static JsonSerializerSettings CreateJsonSerializerSettings(IContractResolver contractResolver)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
										 {
											 ContractResolver = contractResolver,
											 DateFormatHandling = DateFormatHandling.IsoDateFormat,
											 PreserveReferencesHandling = PreserveReferencesHandling.Objects,
											 Formatting = Formatting.None
										 };

			jsonSerializerSettings.Converters.Add(new StringEnumConverter
												  {
													  CamelCaseText = false
												  });
			return jsonSerializerSettings;
		}
	}
}
