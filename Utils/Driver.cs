using PathfinderTest.Algorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics;

//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Driver
    {
        public static Stopwatch stopWatch = new Stopwatch();

        static public void Run()
        {
            var random = new Random();
            string[] lines = Layout.GetLines(random);
            bool[,] grid = Layout.GetGrid(lines);


            (int, int) startCoord = Node.SelectRandom(random, grid);
            Node start = new Node(startCoord.Item1, startCoord.Item2);

            (int, int) goalCoord = (-1, -1);

            bool startGoalDistinct = false;
            do
            {
                goalCoord = Node.SelectRandom(random, grid);

                if (startCoord != goalCoord)
                {
                    break;
                }
            } while (!startGoalDistinct);

            Node goal = new Node(goalCoord.Item1, goalCoord.Item2);

            var astar = new Astar();

            astar.Algorithm(grid, start, goal, lines);

            //foreach (var line in lines)
            //{
            //    Console.WriteLine(line);
            //}

            //foreach (var coordinate in grid)
            //{
            //    Console.WriteLine(coordinate);
            //}

        }
    }
}
