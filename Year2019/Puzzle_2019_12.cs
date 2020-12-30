using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;
using static System.Math;

namespace AoC.Year2019
{
    internal sealed class Puzzle_2019_12 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(8362, 478373365921244);
        }

        private Moon[] Moons { get; }

        public Puzzle_2019_12(string input)
        {
            Moons = input
                .ParseLines(line => line.Int32Matches().Parse(matches => new Moon(matches[0], matches[1], matches[2])));
        }

        protected override object Part1()
        {
            for (var step = 0; step < 1000; step++)
            {

                foreach (var moon in Moons)
                    foreach (var otherMoon in Moons)
                    {
                        moon.Affect(otherMoon);
                    }

                foreach (var moon in Moons)
                {
                    moon.Move();
                }
            }

            var totalEnergy = Moons.Sum(m => m.Energy);

            return totalEnergy;
        }

        protected override object Part2()
        {
            var xPeriod = 0;
            var yPeriod = 0;
            var zPeriod = 0;
            for (var step = 0; step < int.MaxValue; step++)
            {

                foreach (var moon in Moons)
                    foreach (var otherMoon in Moons)
                    {
                        moon.Affect(otherMoon);
                    }

                foreach (var moon in Moons)
                {
                    moon.Move();
                }

                if (xPeriod == 0 && Moons.All(m => m.AtStartX)) xPeriod = step + 1;
                if (yPeriod == 0 && Moons.All(m => m.AtStartY)) yPeriod = step + 1;
                if (zPeriod == 0 && Moons.All(m => m.AtStartZ)) zPeriod = step + 1;

                if (xPeriod != 0 && yPeriod != 0 && zPeriod != 0) break;

            }

            var period = Lcm(xPeriod, yPeriod, zPeriod);
            return period;
        }

        class Moon
        {
            private long _posX;
            private long _posY;
            private long _posZ;

            private readonly long _startX;
            private readonly long _startY;
            private readonly long _startZ;

            private long _velX;
            private long _velY;
            private long _velZ;


            public long Energy => (Abs(_posX) + Abs(_posY) + Abs(_posZ)) * (Abs(_velX) + Abs(_velY) + Abs(_velZ));
            public bool AtStartX => _startX == _posX && _velX == 0;
            public bool AtStartY => _startY == _posY && _velY == 0;
            public bool AtStartZ => _startZ == _posZ && _velZ == 0;

            public Moon(int posX, int posY, int posZ)
            {
                _startX = _posX = posX;
                _startY = _posY = posY;
                _startZ = _posZ = posZ;
            }

            public void Affect(Moon other)
            {
                _velX += _posX > other._posX ? -1 : (_posX < other._posX ? 1 : 0);
                _velY += _posY > other._posY ? -1 : (_posY < other._posY ? 1 : 0);
                _velZ += _posZ > other._posZ ? -1 : (_posZ < other._posZ ? 1 : 0);
            }

            public void Move()
            {
                _posX += _velX;
                _posY += _velY;
                _posZ += _velZ;
            }
        }
    }
}