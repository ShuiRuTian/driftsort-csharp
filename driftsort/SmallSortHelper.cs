using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace driftsort;

internal class SmallSortHelper
{
    public static void small_sort_general_with_scratch<T, C>(Span<T> value, C comparer)
    where C : IComparer<T>
    {
        if (value.Length >= 2)
        {
            InsertionSortShiftLeft(value, 1, comparer);
        }
    }

    public static void small_sort_fallback<T, C>(Span<T> value, C comparer)
    where C : IComparer<T>
    {
        if (value.Length >= 2)
        {
            InsertionSortShiftLeft(value, 1, comparer);
        }
    }

    /// Sort `v` assuming `v[..offset]` is already sorted.
    // TODO(perf): This is a good function to be inlined, check it.
    static void InsertionSortShiftLeft<T, C>(Span<T> values, int offset, C comparer)
    where C : IComparer<T>
    {
        var len = values.Length;
        if (offset == 0 || offset > len)
        {
            throw new InvalidProgramException();
        }

        ref T begin = ref values[0];

        for (int i = offset; i < values.Length; i++)
        {
            insert_tail(ref begin, ref values[i], comparer);
        }
    }

    /// Sorts range [begin, tail] assuming [begin, tail) is already sorted.
    ///
    /// # Safety
    /// begin < tail and p must be valid and initialized for all begin <= p <= tail.
    static void insert_tail<T, C>(ref T begin, ref T tail, C comparer)
        where C : IComparer<T>
    {
        // TODO(perf): Check whether using ref is faster than span[index]
        ref T hole = ref Unsafe.Add(ref tail, -1);

        // Check sorted
        if (comparer.Compare(hole, tail) <= 0)
        {
            return;
        }

        while (Unsafe.AreSame(ref hole, ref begin))
        {
            Unsafe.Add(ref hole, 1) = hole;
            hole = ref Unsafe.Add(ref hole, -1);

            if (comparer.Compare(hole, tail) <= 0)
            {
                break;
            }
        }
        hole = tail;
    }

    static void small_sort_network<T, C>(Span<T> values, C comparer)
        where C : IComparer<T>
    { 

    }

    /// Sorts the first 9 elements of `v` with a fast fixed function.
    ///
    /// Should `is_less` generate substantial amounts of code the compiler can choose to not inline
    /// `swap_if_less`. If the code of a sort impl changes so as to call this function in multiple
    /// places, `#[inline(never)]` is recommended to keep binary-size in check. The current design of
    /// `small_sort_network` makes sure to only call this once.
    static void sort9_optimal<T, C>(Span<T> values, C comparer)
        where C : IComparer<T>
    {
        Debug.Assert(values.Length >= 9);

        // Optimal sorting network see:
        // https://bertdobbelaere.github.io/sorting_networks.html.

        // SAFETY: We checked the len.
        swap_if_less(values, 0, 3, comparer);
        swap_if_less(values, 1, 7, comparer);
        swap_if_less(values, 2, 5, comparer);
        swap_if_less(values, 4, 8, comparer);
        swap_if_less(values, 0, 7, comparer);
        swap_if_less(values, 2, 4, comparer);
        swap_if_less(values, 3, 8, comparer);
        swap_if_less(values, 5, 6, comparer);
        swap_if_less(values, 0, 2, comparer);
        swap_if_less(values, 1, 3, comparer);
        swap_if_less(values, 4, 5, comparer);
        swap_if_less(values, 7, 8, comparer);
        swap_if_less(values, 1, 4, comparer);
        swap_if_less(values, 3, 6, comparer);
        swap_if_less(values, 5, 7, comparer);
        swap_if_less(values, 0, 1, comparer);
        swap_if_less(values, 2, 4, comparer);
        swap_if_less(values, 3, 5, comparer);
        swap_if_less(values, 6, 8, comparer);
        swap_if_less(values, 2, 3, comparer);
        swap_if_less(values, 4, 5, comparer);
        swap_if_less(values, 6, 7, comparer);
        swap_if_less(values, 1, 2, comparer);
        swap_if_less(values, 3, 4, comparer);
        swap_if_less(values, 5, 6, comparer);
    }

    /// Sorts the first 13 elements of `v` with a fast fixed function.
    ///
    /// Should `is_less` generate substantial amounts of code the compiler can choose to not inline
    /// `swap_if_less`. If the code of a sort impl changes so as to call this function in multiple
    /// places, `#[inline(never)]` is recommended to keep binary-size in check. The current design of
    /// `small_sort_network` makes sure to only call this once.
    static void sort13_optimal<T, C>(Span<T> values, C comparer)
        where C : IComparer<T>
    {
        Debug.Assert(values.Length >= 13);

        // Optimal sorting network see:
        // https://bertdobbelaere.github.io/sorting_networks.html.

        // SAFETY: We checked the len.
        swap_if_less(values, 0, 12, comparer);
        swap_if_less(values, 1, 10, comparer);
        swap_if_less(values, 2, 9, comparer);
        swap_if_less(values, 3, 7, comparer);
        swap_if_less(values, 5, 11, comparer);
        swap_if_less(values, 6, 8, comparer);
        swap_if_less(values, 1, 6, comparer);
        swap_if_less(values, 2, 3, comparer);
        swap_if_less(values, 4, 11, comparer);
        swap_if_less(values, 7, 9, comparer);
        swap_if_less(values, 8, 10, comparer);
        swap_if_less(values, 0, 4, comparer);
        swap_if_less(values, 1, 2, comparer);
        swap_if_less(values, 3, 6, comparer);
        swap_if_less(values, 7, 8, comparer);
        swap_if_less(values, 9, 10, comparer);
        swap_if_less(values, 11, 12, comparer);
        swap_if_less(values, 4, 6, comparer);
        swap_if_less(values, 5, 9, comparer);
        swap_if_less(values, 8, 11, comparer);
        swap_if_less(values, 10, 12, comparer);
        swap_if_less(values, 0, 5, comparer);
        swap_if_less(values, 3, 8, comparer);
        swap_if_less(values, 4, 7, comparer);
        swap_if_less(values, 6, 11, comparer);
        swap_if_less(values, 9, 10, comparer);
        swap_if_less(values, 0, 1, comparer);
        swap_if_less(values, 2, 5, comparer);
        swap_if_less(values, 6, 9, comparer);
        swap_if_less(values, 7, 8, comparer);
        swap_if_less(values, 10, 11, comparer);
        swap_if_less(values, 1, 3, comparer);
        swap_if_less(values, 2, 4, comparer);
        swap_if_less(values, 5, 6, comparer);
        swap_if_less(values, 9, 10, comparer);
        swap_if_less(values, 1, 2, comparer);
        swap_if_less(values, 3, 4, comparer);
        swap_if_less(values, 5, 7, comparer);
        swap_if_less(values, 6, 8, comparer);
        swap_if_less(values, 2, 3, comparer);
        swap_if_less(values, 4, 5, comparer);
        swap_if_less(values, 6, 7, comparer);
        swap_if_less(values, 8, 9, comparer);
        swap_if_less(values, 3, 4, comparer);
        swap_if_less(values, 5, 6, comparer);
    }

    /// Swap two values in the slice pointed to by `v_base` at the position `a_pos` and `b_pos` if the
    /// value at position `b_pos` is less than the one at position `a_pos`.
    static void swap_if_less<T, C>(Span<T> values, int a_pos, int b_pos, C comparer)
    where C : IComparer<T>
    {
        Debug.Assert(a_pos < values.Length);
        Debug.Assert(b_pos < values.Length);
        ref var v_base = ref values[0];

        ref var v_a = ref Unsafe.Add(ref v_base, a_pos);
        ref var v_b = ref Unsafe.Add(ref v_base, b_pos);

        var should_swap = comparer.Compare(v_a, v_b) > 0;

        // TODO(perf): The rust implementation mentioned that it's intentional to generate cmove instructions.
        //             But seems C# could not.
        if (should_swap)
        {
            var tmp = v_b;
            v_b = v_a;
            v_a = tmp;
        }
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
