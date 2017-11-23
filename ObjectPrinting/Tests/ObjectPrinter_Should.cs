using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinter_Should
    {
        private Person person;

        [SetUp]
        public void SetUp()
        {
            person = new Person { Name = "Eduard", Height = 182.5, Age = 18 };
        }

        private static string GenerateExpectedString(string id, string name, string height, string age)
        {
            var sb = new StringBuilder();

            sb.AppendLine("Person");
            if (id != null)
                sb.AppendLine("\tId = " + id);
            if (name != null)
                sb.AppendLine("\tName = " + name);
            if (height != null)
                sb.AppendLine("\tHeight = " + height);
            if (age != null)
                sb.AppendLine("\tAge = " + age);

            return sb.ToString();
        }

        private static string PrintObject<T>(T obj, Func<PrintingConfig<T>, PrintingConfig<T>> configurator)
        {
            var printer = ObjectPrinter.For<T>(configurator);
            return printer.PrintToString(obj);
        }

        [Test]
        public void PrintNull()
        {
            var expected = "null" + Environment.NewLine;

            var printer = ObjectPrinter.For<Person>();
            var actual = printer.PrintToString(null);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithoutConfiguration()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: person.Name,
                height: person.Height.ToString(),
                age: person.Age.ToString());

            var printer = ObjectPrinter.For<Person>();
            var actual = printer.PrintToString(person);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithExcludedType()
        {
            var expected = GenerateExpectedString(
                id: null,
                name: person.Name,
                height: person.Height.ToString(),
                age: person.Age.ToString());

            var actual = PrintObject(person, options => options.ExcludeType<Guid>());
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithExcludedProperty()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: person.Name,
                height: null,
                age: person.Age.ToString());

            var actual = PrintObject(person, options => options.ExcludeProperty(p => p.Height));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithCustomTypeSerialization()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: person.Name,
                height: person.Height.ToString(),
                age: "System.Int32");

            var actual = PrintObject(person, options => options.Print<int>().Using(i => "System.Int32"));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithCustomPropertySerialization()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: $"\"{person.Name}\"",
                height: person.Height.ToString(),
                age: person.Age.ToString());

            var actual = PrintObject(person, options => options.Print(p => p.Name).Using(s => $"\"{s}\""));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithCustomCulture()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: person.Name,
                height: person.Height.ToString(CultureInfo.InvariantCulture),
                age: person.Age.ToString());

            var actual = PrintObject(person, options => options
            .Print<double>().WithCulture(CultureInfo.InvariantCulture));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintWithTrimmedString()
        {
            var expected = GenerateExpectedString(
                id: person.Id.ToString(),
                name: person.Name.Substring(0, 3),
                height: person.Height.ToString(),
                age: person.Age.ToString());

            var actual = PrintObject(person, options => options.Print<string>().TrimmedTo(3));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void PrintNestedObjects()
        {
            var item = new Item { ItemName = "Apple", Owner = person };

            var expected = 
                "Item" + Environment.NewLine
                + "\tItemName = " + item.ItemName + Environment.NewLine
                + "\tOwner = Person" + Environment.NewLine
                + "\t\tId = " + person.Id + Environment.NewLine
                + "\t\tName = " + person.Name + Environment.NewLine
                + "\t\tHeight = " + person.Height + Environment.NewLine
                + "\t\tAge = " + person.Age + Environment.NewLine;

            var printer = ObjectPrinter.For<Item>();
            var actual = printer.PrintToString(item);

            Assert.AreEqual(expected, actual);
        }
    }
}