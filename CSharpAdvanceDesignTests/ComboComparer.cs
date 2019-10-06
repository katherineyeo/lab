using System.Collections.Generic;
using Lab.Entities;

namespace CSharpAdvanceDesignTests
{
    public class ComboComparer : IComparer<Employee>
    {
        public ComboComparer(IComparer<Employee> firstComparer, IComparer<Employee> secondComparer)
        {
            FirstComparer = firstComparer;
            SecondComparer = secondComparer;
        }

        private IComparer<Employee> FirstComparer { get; set; }
        private IComparer<Employee> SecondComparer { get; set; }

        public int Compare(Employee x, Employee y)
        {
            var firstCompareResult = FirstComparer.Compare(x, y);
            if (firstCompareResult != 0)
            {
                return firstCompareResult;
            }
            return SecondComparer.Compare(x, y);
        }
    }
}