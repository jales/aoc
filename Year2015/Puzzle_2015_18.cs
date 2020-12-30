using System;
using System.Linq;
using AoC.Infrastructure.Puzzles;

namespace AoC.Year2015
{
    internal sealed class Puzzle_2015_18 : Puzzle
    {
        public static void Configure()
        {
            SetSolution(768, 781);
        }

        private bool[][] Grid { get; set; }
        private bool[][] TempGrid { get; set; }

        public Puzzle_2015_18(string input)
        {
            Grid = input
               .ParseLines(line => line.Select(c => c =='#').ToArray());

            TempGrid = Enumerable.Range(0, 100).Select(_ => new bool[100]).ToArray();
        }

        protected override object Part1()
        {
            for (var i = 0; i < 100; i++)
            {
                MoveGridToNextState();
            }

            return Grid.SelectMany(r => r).Count(l => l);
        }

        protected override object Part2()
        {

            for (var i = 0; i < 100; i++)
            {
                Grid[00][00] = true;
                Grid[00][99] = true;
                Grid[99][00] = true;
                Grid[99][99] = true;
                MoveGridToNextState();
            }

            Grid[00][00] = true;
            Grid[00][99] = true;
            Grid[99][00] = true;
            Grid[99][99] = true;

            return Grid.SelectMany(r => r).Count(l => l);
        }

        private void MoveGridToNextState(int maxI = 100, int maxJ = 100)
        {
            for (var i = 0; i < maxI; i++)
            for (var j = 0; j < maxJ; j++)
            {
                TempGrid[i][j] = GetLightNextState(i, j, maxI, maxJ);
            }

            var tmp = Grid;
            Grid = TempGrid;
            TempGrid = tmp;
        }

        private bool GetLightNextState(int i, int j, int maxI, int maxJ)
        {
            var onNeighbors = 0;
            maxI -= 1;
            maxJ -= 1;

            if (i > 0    && j > 0    && Grid[i-1][j-1]) onNeighbors++;
            if (i > 0                && Grid[i-1][j  ]) onNeighbors++;
            if (i > 0    && j < maxJ && Grid[i-1][j+1]) onNeighbors++;
            if (            j < maxJ && Grid[i  ][j+1]) onNeighbors++;
            if (i < maxI && j < maxJ && Grid[i+1][j+1]) onNeighbors++;
            if (i < maxI             && Grid[i+1][j  ]) onNeighbors++;
            if (i < maxI && j > 0    && Grid[i+1][j-1]) onNeighbors++;
            if (            j > 0    && Grid[i  ][j-1]) onNeighbors++;

            return Grid[i][j] ? onNeighbors == 2 || onNeighbors == 3 : onNeighbors == 3;
        }
    }
}