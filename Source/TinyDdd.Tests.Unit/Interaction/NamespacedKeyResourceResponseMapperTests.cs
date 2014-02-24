using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using SwissKnife;
using TinyDdd.Interaction;

namespace TinyDdd.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class NamespacedKeyResourceResponseMapperTests
    {
        private readonly Assembly _resourceAssembly = typeof (NamespacedKeyResourceResponseMapperTests).Assembly;
        private readonly string _resourceBaseName = typeof(NamespacedKeyResourceResponseMapperTests).Assembly.GetName().Name + ".Interaction.ClientResource";

        private NamespacedKeyResourceResponseMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new NamespacedKeyResourceResponseMapper(_resourceAssembly, _resourceBaseName);
        }

        [Test]
        public void Map_KeyIsOverloaded_OverloadsMessage()
        {
            // The original response is returned from one layer, e.g. from the domain code.
            var originalResponse = new Response();
            originalResponse.AddInformation(DomainResource.TestEntityResource.FirstKey, GetNamespacedKey(() => DomainResource.TestEntityResource.FirstKey));
            Assert.That(originalResponse.Informations.First().Key, Is.EqualTo("TestEntityResource.FirstKey"));
            Assert.That(originalResponse.Informations.First().Message, Is.EqualTo(DomainResource.TestEntityResource.FirstKey));
            
            // The other layer, e.g. the client code, overrides the messages in its own resource.
            var mappedResponse = _mapper.Map(originalResponse);
            Assert.That(mappedResponse.Informations.First().Key, Is.EqualTo("TestEntityResource.FirstKey")); // The key stays the same.
            Assert.That(mappedResponse.Informations.First().Message, Is.EqualTo(ClientResource.TestEntityResource.FirstKey)); // The message is translated.
        }

        [Test]
        public void Map_KeyIsNotOverloaded_KeepsOriginalMessage()
        {
            // The original response is returned from one layer, e.g. from the domain code.
            var originalResponse = new Response();
            originalResponse.AddInformation(DomainResource.TestEntityResource.SecondKey, GetNamespacedKey(() => DomainResource.TestEntityResource.SecondKey));
            Assert.That(originalResponse.Informations.First().Key, Is.EqualTo("TestEntityResource.SecondKey"));
            Assert.That(originalResponse.Informations.First().Message, Is.EqualTo(DomainResource.TestEntityResource.SecondKey));

            // The other layer, e.g. the client code, does not have the message overridden in its own resource.
            var mappedResponse = _mapper.Map(originalResponse);
            Assert.That(mappedResponse.Informations.First().Key, Is.EqualTo("TestEntityResource.SecondKey")); // The key stays the same.
            Assert.That(mappedResponse.Informations.First().Message, Is.EqualTo(DomainResource.TestEntityResource.SecondKey)); // The message is translated.
        }

        private static string GetNamespacedKey(Expression<Func<object>> identifierExpression)
        {
            return Identifier.ToString(identifierExpression, new Identifier.ConversionOptions { StaticMemberConversion = Identifier.StaticMemberConversion.ParentTypeName });
        }
    }
    // ReSharper restore InconsistentNaming
}
