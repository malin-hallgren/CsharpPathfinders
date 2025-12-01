using PathfinderTest.Algorithm;
using System.Diagnostics;

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
            Node start = new Node(1, 1);
            //(startCoord.Item1, startCoord.Item2);

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

            Node goal = new Node(9, 12);
                
            //(goalCoord.Item1, goalCoord.Item2);

            var astar = new Astar();

            // Measure only the A* algorithm wall-clock time
            var algSw = Stopwatch.StartNew();
            astar.Algorithm(grid, start, goal, lines);
            algSw.Stop();

            Console.WriteLine(FormattableString.Invariant($"A* algorithm elapsed: {algSw.Elapsed.TotalMilliseconds} ms ({algSw.Elapsed.Ticks} ticks)"));

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
