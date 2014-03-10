using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    // FUTURE: Support more than one resource assembly or maybe provide CompoundResponseMapper.
    //         In the second case, the IResponseMapper should have the CanMappe() method.
    public class NamespacedKeyResourceResponseMapper : IResponseMapper // TODO-IG: Test this class properly and mybe add some more error handling.
    {
        private readonly Dictionary<string, ResourceManager> _resourceManagers = new Dictionary<string, ResourceManager>();
        private readonly Assembly _resourceAssembly;
        private readonly string _resourceBaseName;

        public NamespacedKeyResourceResponseMapper(Assembly resourceAssembly, string resourceBaseName)
        {
            Argument.IsNotNull(resourceAssembly, "resourceAssembly");
            Argument.IsNotNull(resourceBaseName, "resourceBaseName");

            _resourceAssembly = resourceAssembly;
            _resourceBaseName = resourceBaseName;
        }

        public Response Map(Response originalResponse)
        {
            Argument.IsNotNull(originalResponse, "originalResponse");

            Response result = new Response();
            originalResponse.AllMessages.ForEach(message => result.Add(MapSingleResponseMessage(message)));
            return result;
        }

        private ResponseMessage MapSingleResponseMessage(ResponseMessage responseMessage)
        {
            System.Diagnostics.Debug.Assert(responseMessage != null);

            if (!responseMessage.HasKey)
                return responseMessage; // ResponseMessage class is deep immutable, so returning the same object is perfectly fine.

            return new ResponseMessage(responseMessage.MessageType, MapMessage(responseMessage), responseMessage.Key);
        }
        
        private string MapMessage(ResponseMessage responseMessage)
        {
            System.Diagnostics.Debug.Assert(responseMessage.HasKey);

            // Dot is used as the namespace separator. The last dot represents the end of the namespace and the beginning of the key.
            int indexOfTheLastDot = responseMessage.Key.LastIndexOf('.');

            // The namespace is not mandatory.
            // In case it is not defined, the resource name is the same as the resource base name and the key is the whole content of the Key.
            string resourceName = _resourceBaseName;
            string keyWithoutNamespace = responseMessage.Key;
            if (indexOfTheLastDot > 0)
            {
                // Add the namespace to the resource base name.
                resourceName += ("." + responseMessage.Key.Substring(0, indexOfTheLastDot));

                if (indexOfTheLastDot == responseMessage.Key.Length - 1)
                    throw new InvalidOperationException(string.Format("Invalid namespaced key. A namespaced key cannot end with a dot (.).{0}" +
                                                                      "The namespaced key is: {1}",
                                                                      Environment.NewLine,
                                                                      responseMessage.Key));
                keyWithoutNamespace = responseMessage.Key.Substring(indexOfTheLastDot + 1);
            }

            // ResourceManager.GetString() returns null if the key is not found.
            return GetResourceManagerFor(resourceName).GetString(keyWithoutNamespace) ?? responseMessage.Message;
        }

        private ResourceManager GetResourceManagerFor(string resourceName)
        {
            return _resourceManagers.GetValue(resourceName, () => new ResourceManager(resourceName, _resourceAssembly));
        }
    }
}
