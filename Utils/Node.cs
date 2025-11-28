using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal struct Node
    {
        //these should have been col and row
        public int Row;
        public int Col;

        public Node (int x, int y)
        {
            Row = x;
            Col = y;
        }

        static public (int, int) SelectRandom(Random rand, bool[,] passable)
        {
            int rows = passable.GetLength(0);
            int cols = passable.GetLength(1);

            while (true)
            {
                (int, int) selected = (rand.Next(1, rows - 1), rand.Next(1, cols - 1));
                if (!passable[selected.Item1, selected.Item2])
                {
                    continue;
                }
                return (selected.Item1, selected.Item2);
            }

        }

        //public override bool Equals(object? obj)
        //{
        //    return obj is Node node && node.X == X && node.Y == Y;
        //}
        //public override int GetHashCode()
        //{
        //    return X * 31 * Y;
        //}
    }
}
