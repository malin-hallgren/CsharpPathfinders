using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest
{
    internal struct Cell
    {
        public int parent_i, parent_j;
        public double g, h, f;

    }
}
