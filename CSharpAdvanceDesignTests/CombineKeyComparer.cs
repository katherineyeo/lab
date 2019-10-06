using System;
using System.Collections.Generic;
using Lab.Entities;

namespace CSharpAdvanceDesignTests
{
    public class CombineKeyComparer<TKey> : IComparer<Employee>
    {
        public CombineKeyComparer(Func<Employee, TKey> keySelector, IComparer<TKey> keyComparer)
        {
            KeySelector = keySelector;
            KeyComparer = keyComparer;
        }

        public Func<Employee, TKey> KeySelector { get; private set; }
        public IComparer<TKey> KeyComparer { get; private set; }

        public int Compare(Employee x, Employee y)
        {
            return KeyComparer.Compare(KeySelector(x), KeySelector(y));
        }
    }
}