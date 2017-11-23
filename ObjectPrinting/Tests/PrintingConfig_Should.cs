using System;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class PrintingConfig_Should
    {
        [Test]
        public void ThrowOnInvalidPropertySelector()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PrintingConfig<Person>().ExcludeProperty(p => 8));

            Assert.That(ex.Message, Does.StartWith("Selector should be an expression that is " +
                                                   "simple property access"));
        }
    }
}