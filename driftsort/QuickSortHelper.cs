using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace driftsort
{
    // This module contains a stable quicksort and partition implementation.
    internal class QuickSortHelper
    {
        public static void quicksort<T, C>(Span<T> values, C comparer)
            where C : IComparer<T>
        {
            while(true){
                var length = values.Length;

                if(length <= ){
                    
                }
            }
        }
    }
}
