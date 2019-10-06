using ExpectedObjects;
using Lab.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpAdvanceDesignTests
{
    public class ComboComparer
    {
        public ComboComparer(IComparer<Employee> combineKeyComparer, IComparer<Employee> secondCombinedKeyComparer)
        {
            CombineKeyComparer = combineKeyComparer;
            SecondCombinedKeyComparer = secondCombinedKeyComparer;
        }

        public IComparer<Employee> CombineKeyComparer { get; private set; }
        public IComparer<Employee> SecondCombinedKeyComparer { get; private set; }
    }

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


            Func<Employee, string> secondKeySelector = employee => employee.FirstName;
            IComparer<string> secondKeyComparer = Comparer<string>.Default;
            var actual = JoeyOrderByLastNameAndFirstName(employees, new ComboComparer(new CombineKeyComparer(employee => employee.LastName, Comparer<string>.Default), new CombineKeyComparer(secondKeySelector, secondKeyComparer)));

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
                for (int i = 1; i < elements.Count; i++)
                {
                    var employee = elements[i];
                    if (comboComparer.CombineKeyComparer.Compare(employee, minElement) < 0)
                    {
                        minElement = employee;
                        index = i;
                    }
                    else if (comboComparer.CombineKeyComparer.Compare(employee, minElement) == 0)
                    {
                        if (comboComparer.SecondCombinedKeyComparer.Compare(employee, minElement) < 0)
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