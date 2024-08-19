using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace driftsort;

internal class SmallSortHelper
{
    /// Sort `v` assuming `v[..offset]` is already sorted.
    public void insertion_sort_shift_left<T, U>(Span<T> value, int offset, U comparer)
    where U : IComparer<T>
    {
        var len = value.Length;
        if (offset == 0 || offset > len)
        {
            throw new InvalidProgramException();
        }

        // TODO(perf): use Unsafe to avoid bound check?
        
    }
}
