using Microsoft.AspNet.SignalR.Infrastructure;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace SOLIDplate.Application.Services.SignalR
{
    public class SignalRContractResolver : IContractResolver
    {
        private readonly Assembly _assembly;
        private readonly IContractResolver _camelCaseContractResolver;
        private readonly IContractResolver _defaultContractSerializer;

        public SignalRContractResolver()
        {
            _defaultContractSerializer = new DefaultContractResolver();
            _camelCaseContractResolver = new CamelCasePropertyNamesContractResolver();
            _assembly = typeof(Connection).Assembly;
        }

        public JsonContract ResolveContract(Type type)
        {
            var isSignalRType = type.Assembly.Equals(_assembly);
            return isSignalRType ?
                   _defaultContractSerializer.ResolveContract(type) :
                   _camelCaseContractResolver.ResolveContract(type);
        }
    }
}
