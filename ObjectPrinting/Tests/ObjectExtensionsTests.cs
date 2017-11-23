using System;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectExtensionsTests
    {
        private Person person;

        [SetUp]
        public void SetUp()
        {
            person = new Person { Name = "Alex", Age = 19 };
        }


        [Test]
        public void PrintToString_ShouldPrintLikeObjectPrinter_WithoutConfiguration()
        {
            var printer = ObjectPrinter.For<Person>();

            Assert.That(person.PrintToString(), Is.EqualTo(printer.PrintToString(person)));
        }

        [Test]
        public void PrintToString_ShouldPrintLikeObjectPrinter_WithConfiguration()
        {
            Func<PrintingConfig<Person>, PrintingConfig<Person>> config = 
                options => options.ExcludeType<Guid>();

            var printer = ObjectPrinter.For<Person>(config);

            Assert.That(person.PrintToString(config), Is.EqualTo(printer.PrintToString(person)));
        }
    }
}