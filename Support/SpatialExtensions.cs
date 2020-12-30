using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Support
{
    public static class SpatialExtensions
    {
        public static IEnumerable<(int x, int y)> Neighbors(this (int x, int y) source)
        {
            var (x, y) = source;

            yield return (x + 1, y + 1);
            yield return (x + 1, y    );
            yield return (x + 1, y - 1);

            yield return (x    , y + 1);
            yield return (x    , y - 1);

            yield return (x - 1, y + 1);
            yield return (x - 1, y    );
            yield return (x - 1, y - 1);
        }

        public static IEnumerable<(int x, int y)> CardinalNeighbors(this (int x, int y) source)
        {
            var (x, y) = source;

            yield return (x + 1, y    );
            yield return (x - 1, y    );

            yield return (x    , y + 1);
            yield return (x    , y - 1);
        }

        public static IEnumerable<(int x, int y, int z)> Neighbors(this (int x, int y, int z) source)
        {
            var (x, y, z) = source;

            yield return (x + 1, y + 1, z + 1);
            yield return (x + 1, y + 1, z    );
            yield return (x + 1, y + 1, z - 1);

            yield return (x + 1, y    , z + 1);
            yield return (x + 1, y    , z    );
            yield return (x + 1, y    , z - 1);

            yield return (x + 1, y - 1, z + 1);
            yield return (x + 1, y - 1, z    );
            yield return (x + 1, y - 1, z - 1);

            yield return (x    , y + 1, z + 1);
            yield return (x    , y + 1, z    );
            yield return (x    , y + 1, z - 1);

            yield return (x    , y    , z + 1);
            yield return (x    , y    , z - 1);

            yield return (x    , y - 1, z    );
            yield return (x    , y - 1, z + 1);
            yield return (x    , y - 1, z - 1);

            yield return (x - 1, y + 1, z + 1);
            yield return (x - 1, y + 1, z    );
            yield return (x - 1, y + 1, z - 1);

            yield return (x - 1, y    , z + 1);
            yield return (x - 1, y    , z    );
            yield return (x - 1, y    , z - 1);

            yield return (x - 1, y - 1, z + 1);
            yield return (x - 1, y - 1, z    );
            yield return (x - 1, y - 1, z - 1);
        }

        public static IEnumerable<(int x, int y, int z)> CardinalNeighbors(this (int x, int y, int z) source)
        {
            var (x, y, z) = source;

            yield return (x + 1, y    , z    );
            yield return (x - 1, y    , z    );

            yield return (x    , y + 1, z    );
            yield return (x    , y - 1, z    );

            yield return (x    , y    , z + 1);
            yield return (x    , y    , z - 1);
        }

        public static IEnumerable<(int x, int y, int z, int w)> Neighbors(this (int x, int y, int z, int w) source)
        {
            var (x, y, z, w) = source;

            yield return (x + 1, y + 1, z + 1, w + 1);
            yield return (x + 1, y + 1, z + 1, w    );
            yield return (x + 1, y + 1, z + 1, w - 1);

            yield return (x + 1, y + 1, z    , w + 1);
            yield return (x + 1, y + 1, z    , w    );
            yield return (x + 1, y + 1, z    , w - 1);

            yield return (x + 1, y + 1, z - 1, w + 1);
            yield return (x + 1, y + 1, z - 1, w    );
            yield return (x + 1, y + 1, z - 1, w - 1);

            yield return (x + 1, y    , z + 1, w + 1);
            yield return (x + 1, y    , z + 1, w    );
            yield return (x + 1, y    , z + 1, w - 1);

            yield return (x + 1, y    , z    , w + 1);
            yield return (x + 1, y    , z    , w    );
            yield return (x + 1, y    , z    , w - 1);

            yield return (x + 1, y    , z - 1, w + 1);
            yield return (x + 1, y    , z - 1, w    );
            yield return (x + 1, y    , z - 1, w - 1);

            yield return (x + 1, y - 1, z + 1, w + 1);
            yield return (x + 1, y - 1, z + 1, w    );
            yield return (x + 1, y - 1, z + 1, w - 1);

            yield return (x + 1, y - 1, z    , w + 1);
            yield return (x + 1, y - 1, z    , w    );
            yield return (x + 1, y - 1, z    , w - 1);

            yield return (x + 1, y - 1, z - 1, w + 1);
            yield return (x + 1, y - 1, z - 1, w    );
            yield return (x + 1, y - 1, z - 1, w - 1);

            yield return (x    , y + 1, z + 1, w + 1);
            yield return (x    , y + 1, z + 1, w    );
            yield return (x    , y + 1, z + 1, w - 1);

            yield return (x    , y + 1, z    , w + 1);
            yield return (x    , y + 1, z    , w    );
            yield return (x    , y + 1, z    , w - 1);

            yield return (x    , y + 1, z - 1, w + 1);
            yield return (x    , y + 1, z - 1, w    );
            yield return (x    , y + 1, z - 1, w - 1);

            yield return (x    , y    , z + 1, w + 1);
            yield return (x    , y    , z + 1, w    );
            yield return (x    , y    , z + 1, w - 1);

            yield return (x    , y    , z    , w + 1);
            yield return (x    , y    , z    , w - 1);

            yield return (x    , y    , z - 1, w + 1);
            yield return (x    , y    , z - 1, w    );
            yield return (x    , y    , z - 1, w - 1);

            yield return (x    , y - 1, z + 1, w + 1);
            yield return (x    , y - 1, z + 1, w    );
            yield return (x    , y - 1, z + 1, w - 1);

            yield return (x    , y - 1, z    , w + 1);
            yield return (x    , y - 1, z    , w    );
            yield return (x    , y - 1, z    , w - 1);

            yield return (x    , y - 1, z - 1, w + 1);
            yield return (x    , y - 1, z - 1, w    );
            yield return (x    , y - 1, z - 1, w - 1);

            yield return (x - 1, y + 1, z + 1, w + 1);
            yield return (x - 1, y + 1, z + 1, w    );
            yield return (x - 1, y + 1, z + 1, w - 1);

            yield return (x - 1, y + 1, z    , w + 1);
            yield return (x - 1, y + 1, z    , w    );
            yield return (x - 1, y + 1, z    , w - 1);

            yield return (x - 1, y + 1, z - 1, w + 1);
            yield return (x - 1, y + 1, z - 1, w    );
            yield return (x - 1, y + 1, z - 1, w - 1);

            yield return (x - 1, y    , z + 1, w + 1);
            yield return (x - 1, y    , z + 1, w    );
            yield return (x - 1, y    , z + 1, w - 1);

            yield return (x - 1, y    , z    , w + 1);
            yield return (x - 1, y    , z    , w    );
            yield return (x - 1, y    , z    , w - 1);

            yield return (x - 1, y    , z - 1, w + 1);
            yield return (x - 1, y    , z - 1, w    );
            yield return (x - 1, y    , z - 1, w - 1);

            yield return (x - 1, y - 1, z + 1, w + 1);
            yield return (x - 1, y - 1, z + 1, w    );
            yield return (x - 1, y - 1, z + 1, w - 1);

            yield return (x - 1, y - 1, z    , w + 1);
            yield return (x - 1, y - 1, z    , w    );
            yield return (x - 1, y - 1, z    , w - 1);

            yield return (x - 1, y - 1, z - 1, w + 1);
            yield return (x - 1, y - 1, z - 1, w    );
            yield return (x - 1, y - 1, z - 1, w - 1);
        }

        public static IEnumerable<(int x, int y, int z, int w)> CardinalNeighbors(this (int x, int y, int z, int w) source)
        {
            var (x, y, z, w) = source;

            yield return (x + 1, y    , z    , w   );
            yield return (x - 1, y    , z    , w   );

            yield return (x    , y + 1, z    , w   );
            yield return (x    , y - 1, z    , w   );

            yield return (x    , y    , z + 1, w   );
            yield return (x    , y    , z - 1, w   );

            yield return (x    , y    , z    , w + 1);
            yield return (x    , y    , z    , w - 1);
        }
    }
}
