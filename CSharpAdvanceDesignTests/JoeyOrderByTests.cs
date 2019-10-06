using ExpectedObjects;
using Lab.Entities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CSharpAdvanceDesignTests
{
    [TestFixture]
    public class JoeyOrderByTests
    {
        [Test]
        public void orderBy_lastName_and_firstName()
        {
            var employees = new[]
            {
                new Employee {FirstName = "Joey", LastName = "Wang"},
                new Employee {FirstName = "Tom", LastName = "Li"},
                new Employee {FirstName = "Joseph", LastName = "Chen"},
                new Employee {FirstName = "Joey", LastName = "Chen"},
            };

            var firstComparer = new CombineKeyComparer<string>(employee => employee.LastName, Comparer<string>.Default);
            var secondComparer = new CombineKeyComparer<string>(employee => employee.FirstName, Comparer<string>.Default);

            var actual = JoeyOrderByLastNameAndFirstName(
                employees,
                new ComboComparer(firstComparer, secondComparer));

            var expected = new[]
            {
                new Employee {FirstName = "Joey", LastName = "Chen"},
                new Employee {FirstName = "Joseph", LastName = "Chen"},
                new Employee {FirstName = "Tom", LastName = "Li"},
                new Employee {FirstName = "Joey", LastName = "Wang"},
            };

            expected.ToExpectedObject().ShouldMatch(actual);
        }

        private IEnumerable<Employee> JoeyOrderByLastNameAndFirstName(
            IEnumerable<Employee> employees,
            ComboComparer comboComparer)
        {
            //bubble sort
            var elements = employees.ToList();
            while (elements.Any())
            {
                var minElement = elements[0];
                var index = 0;
                var finalCompareResult = 0;
                for (int i = 1; i < elements.Count; i++)
                {
                    var employee = elements[i];
                    finalCompareResult = comboComparer.Compare(employee, minElement);

                    if (finalCompareResult < 0)
                    {
                        minElement = employee;
                        index = i;
                    }

                }

                elements.RemoveAt(index);
                yield return minElement;
            }
        }
    }
}