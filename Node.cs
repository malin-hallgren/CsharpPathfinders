using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest
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
