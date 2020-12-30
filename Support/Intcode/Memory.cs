using System.Collections.Generic;

namespace AoC.Support.Intcode
{

    public class Memory
    {
        private const int PageSize = 4096;
        private readonly List<long[]> _pages = new();

        public Memory()
        {
        }

        private Memory(Memory other)
        {
            foreach (var page in other._pages)
            {
                _pages.Add((long[])page.Clone());
            }
        }

        public void WriteTo(long address, long value)
        {
            GetPointerToAddress(address) = value;
        }

        public void WriteRangeTo(long address, IEnumerable<long> values)
        {
            foreach (var value in values)
            {
                GetPointerToAddress(address++) = value;
            }
        }

        public long ReadFrom(long address)
        {
            return GetPointerToAddress(address);
        }

        public long[] ReadRangeFrom(long address, int count)
        {
            var range = new long[count];

            for (var i = 0; i < count; i++)
            {
                range[i] = GetPointerToAddress(address + i);
            }

            return range;
        }

        private ref long GetPointerToAddress(long address)
        {
            var pageIndex = (int)address / PageSize;
            var pageOffset = (int)address % PageSize;

            while (_pages.Count <= pageIndex)
            {
                _pages.Add(new long[PageSize]);
            }

            return ref _pages[pageIndex][pageOffset];
        }

        public Memory Snapshot() => new Memory(this);
    }
}
