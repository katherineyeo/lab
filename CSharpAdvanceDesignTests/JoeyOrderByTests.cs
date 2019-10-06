using ExpectedObjects;
using Lab.Entities;
using NUnit.Framework;
using System;
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


            var actual = JoeyOrderByLastNameAndFirstName(
                employees,
                employee => employee.LastName,
                Comparer<string>.Default,
                employee => employee.FirstName,
                Comparer<string>.Default);

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
            Func<Employee, string> firstKeySelector, 
            IComparer<string> firstKeyComparer, 
            Func<Employee, string> secondKeySelector, 
            IComparer<string> secondKeyComparer)
        {
            //bubble sort
            var elements = employees.ToList();
            while (elements.Any())
            {
                var minElement = elements[0];
                var index = 0;
                for (int i = 1; i < elements.Count; i++)
                {
                    var employee = elements[i];
                    if (firstKeyComparer.Compare(firstKeySelector(employee), firstKeySelector(minElement)) < 0)
                    {
                        minElement = employee;
                        index = i;
                    }
                    else if (firstKeyComparer.Compare(firstKeySelector(employee), firstKeySelector(minElement)) == 0)
                    {
                        if (secondKeyComparer.Compare(secondKeySelector(employee), secondKeySelector(minElement)) < 0)
                        {
                            minElement = employee;
                            index = i;
                        }
                    }
                }

                elements.RemoveAt(index);
                yield return minElement;
            }
        }
    }
}