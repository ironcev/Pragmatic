using System.Linq;
using NUnit.Framework;
using Pragmatic.Interaction;

namespace Pragmatic.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class ResponseTests
    {
        private Response _response; 

        [SetUp]
        public void SetUp()
        {
            _response = new Response();
        }

        #region AddSuccess(string message)
        [Test]
        public void AddSuccess_AddsSingleSuccessMessage()
        {
            _response.AddSuccess("SuccessMessage");
            Assert.That(_response.Information, Is.Empty);
            Assert.That(_response.Warnings, Is.Empty);
            Assert.That(_response.Errors, Is.Empty);
            Assert.That(_response.Successes.Count(), Is.EqualTo(1));
            Assert.That(_response.Successes.First().Message, Is.EqualTo("SuccessMessage"));
            Assert.That(_response.Successes.First().MessageType, Is.EqualTo(MessageType.Success));
        }
        #endregion
    }
    // ReSharper restore InconsistentNaming
}
