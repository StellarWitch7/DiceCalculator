using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StellarDiceCalculator
{
    public class CustomReverseComparer : IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return ((int)y).CompareTo((int)x);
        }
    }
}
