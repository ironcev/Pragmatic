using NUnit.Framework;
using TinyDdd.Interaction;

namespace TinyDdd.Tests.Unit.Interaction
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
