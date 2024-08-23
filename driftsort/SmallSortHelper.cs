using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace driftsort;

internal class SmallSortHelper
{
    public static void small_sort_fallback<T, C>(Span<T> value, C comparer)
    where C : IComparer<T>
    {
        if (value.Length >= 2)
        {
            InsertionSortShiftLeft(value, 1, comparer);
        }
    }

    /// Sort `v` assuming `v[..offset]` is already sorted.
    static void InsertionSortShiftLeft<T, C>(Span<T> value, int offset, C comparer)
    where C : IComparer<T>
    {
        var len = value.Length;
        if (offset == 0 || offset > len)
        {
            throw new InvalidProgramException();
        }

        foreach (T item in value)
        {

        }
    }

    /// Sorts range [begin, tail] assuming [begin, tail) is already sorted.
    ///
    /// # Safety
    /// begin < tail and p must be valid and initialized for all begin <= p <= tail.
    static void insert_tail<T, C>(Span<T> values, int beginIndex, int tailIndex, C comparer)
    where C : IComparer<T>
    {
        Debug.Assert(beginIndex < tailIndex);
        Debug.Assert(tailIndex < values.Length);

        // TODO(perf): Check whether using ref is faster than span[index]
        ref T hole = ref values[tailIndex - 1];
        T insertValue = values[tailIndex];

        // Check sorted
        if (comparer.Compare(hole, insertValue) <= 0)
        {
            return;
        }

        while (Unsafe.AsPointer(ref hole) != Unsafe.AsPointer(ref values[beginIndex]))
        {
            Unsafe.Add(ref hole, 1) = hole;
            hole = ref Unsafe.Add(ref hole, -1);

            if (comparer.Compare(hole, insertValue) <= 0)
            {
                break;
            }
        }
        hole = insertValue;
    }

    public static readonly nuint SMALL_SORT_FALLBACK_THRESHOLD = 16;

    public static readonly nuint SMALL_SORT_GENERAL_THRESHOLD = 32;

    public static readonly nuint SMALL_SORT_GENERAL_SCRATCH_LEN = SMALL_SORT_GENERAL_THRESHOLD + 16;

    public static readonly nuint SMALL_SORT_NETWORK_THRESHOLD = 32;
    public static readonly nuint SMALL_SORT_NETWORK_SCRATCH_LEN = SMALL_SORT_NETWORK_THRESHOLD;

}

internal class StableSmallSortTypeUnfreezeHelper<T>
{
    public static nuint SmallSortThreshold() =>
        SmallSortHelper.SMALL_SORT_GENERAL_THRESHOLD;
}

// In rust, there is an internal marker called FreezeMarker. The implement in rust use specialization, which is lacked in C#.
// So create 2 helpers to do the similar things.
// Note: I do not quite understand what "FreezeMarker" means in rust, so 2 helpers are also more flexiable to adapt for furture changes.
internal class StableSmallSortTypeFreezeHelper<T>
{
    public static nuint SmallSortThreshold() =>
    SmallSortHelper.SMALL_SORT_GENERAL_THRESHOLD;
}
