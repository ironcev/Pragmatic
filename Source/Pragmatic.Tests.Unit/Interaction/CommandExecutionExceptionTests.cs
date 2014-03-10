using NUnit.Framework;
using Pragmatic.Interaction;

namespace Pragmatic.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class CommandExecutionExceptionTests
    {
        [Test]
        public void ToString_ContainsExceptionMessage()
        {
            Assert.That(new CommandExecutionException("Exception message").ToString().Contains("Exception message"));
        }
    }
    // ReSharper restore InconsistentNaming
}
