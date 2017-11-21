using System;
using System.Globalization;
using System.Text;
using NUnit.Framework;

namespace ObjectPrinting.Tests
{
    [TestFixture]
    public class ObjectPrinter_Should
    {
        private readonly Person person = new Person { Name = "Eduard", Height = 182.5, Age = 18 };

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

        [Test]
        public void PrintNull()
        {
            var printer = ObjectPrinter.For<Person>();

            var expected = "null" + Environment.NewLine;
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

            var actual = person.PrintToString();
            
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

            var actual = person.PrintToString(options => options.ExcludeType<Guid>());
            
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

            var actual = person.PrintToString(options => options.ExcludeProperty(p => p.Height));
            
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

            var actual = person.PrintToString(options => options.Print<int>().Using(i => "System.Int32"));
            
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

            var actual = person.PrintToString(options => options.Print<string>().Using(s => $"\"{s}\""));
            
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

            var actual = person.PrintToString(options => options
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

            var actual = person.PrintToString(options => options.Print<string>().TrimmedTo(3));
            
            Assert.AreEqual(expected, actual);
        }
    }
}