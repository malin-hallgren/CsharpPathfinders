using PathfinderTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Algorithm
{
    internal class Astar
    {
        private double _baseCost;
        private double _diagCost;
        private readonly (int dr, int dc)[] Neighbouring =
        {
            (-1, 0), (1, 0), (0, -1), (0, 1),
            (-1, -1), (-1, 1), (1, -1), (1, 1)
        };

        public Astar(double baseCost = 1, double diagCost = 1.414)
        {
            _baseCost = baseCost;
            _diagCost = diagCost;
        }

        public void Algorithm(bool[,] grid, Node src, Node goal, string[] lines)
        {
            Driver.stopWatch.Start();
            int ROW = grid.GetLength(0);
            int COL = grid.GetLength(1);

            if (!InRange(src.Row, src.Col, ROW, COL) || !InRange(goal.Row, goal.Col, ROW, COL))
            {
                Console.WriteLine("Start or destination out of range");
                return;
            }
            if (src.Row == goal.Row && src.Col == goal.Col)
            {
                Console.WriteLine("Destination already reached");
                return;
            }

            bool[,] closed = new bool[ROW, COL];

            Cell[,] cells = new Cell[ROW, COL];

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    cells[i, j].f = double.MaxValue; //we don't know yet
                    cells[i, j].g = double.MaxValue; //we don't know yet
                    cells[i, j].h = double.MaxValue; //we don't know yet
                    cells[i, j].parent_i = -1; //we don't know yet
                    cells[i, j].parent_j = -1; //we don't know yet
                }
            }

            cells[src.Row, src.Col].f = 0;
            cells[src.Row, src.Col].g = 0;
            cells[src.Row, src.Col].h = 0;
            cells[src.Row, src.Col].parent_i = src.Row;
            cells[src.Row, src.Col].parent_j = src.Col;

            //Item 1 is f, Item 2 is coordinate of that cell
            PriorityQueue<Node, double> open = new PriorityQueue<Node, double>();

            open.Enqueue(new Node(src.Row, src.Col), 0);

            bool foundGoal = false;

            while (open.Count > 0)
            {
                //find lowest f, remove it from open, add it to closed as we will visit it
                //Node current = open.Dequeue();

                Node current;

                while(open.TryDequeue(out current, out var fCurrent))
                {
                    int r = current.Row;
                    int c = current.Col;

                    //check if this is stale
                    if(fCurrent > cells[r, c].f)
                    {
                        continue;
                    }

                    //check if current is already closed and skip ahead if it is
                    if (closed[current.Row, current.Col])
                    {
                        continue;
                    }
 
                    closed[r, c] = true;

                    int row = current.Row;
                    int col = current.Col;


                    foreach (var neighbour in Neighbouring)
                    {
                        int newRow = row + neighbour.dr;
                        int newCol = col + neighbour.dc;

                        if (neighbour.dr != 0 && neighbour.dc != 0)
                        {
                            if (!IsWalkable(grid, row + neighbour.dr, col) || !IsWalkable(grid, row, col + neighbour.dc))
                                continue;
                        }

                        if (InRange(newRow, newCol, ROW, COL))
                        {
                            if (IsGoal(newRow, newCol, goal))
                            {
                                cells[newRow, newCol].parent_i = row;
                                cells[newRow, newCol].parent_j = col;
                                Visualizer.Trace(cells, new Node(newRow, newCol), src, lines);
                                foundGoal = true;
                                return;
                            }

                            if (!closed[newRow, newCol] && IsWalkable(grid, newRow, newCol))
                            {
                                double gNew = cells[row, col].g + (neighbour.dr != 0 && neighbour.dc != 0 ? _diagCost : _baseCost);
                                double hNew = CalculateH(newRow, newCol, goal);
                                double fNew = gNew + hNew;

                                if (cells[newRow, newCol].f == double.MaxValue || cells[newRow, newCol].f > fNew)
                                {
                                    open.Enqueue(new Node(newRow, newCol), fNew);

                                    cells[newRow, newCol].f = fNew;
                                    cells[newRow, newCol].g = gNew;
                                    cells[newRow, newCol].h = hNew;
                                    cells[newRow, newCol].parent_i = row;
                                    cells[newRow, newCol].parent_j = col;
                                }

                            }
                        }
                    }
                }

                

                //for (int i = -1; i <= 1; i++)
                //{
                //    for (int j = -1; j <= 1; j++)
                //    {
                //        if (i == 0 && j == 0)
                //        {
                //            continue;
                //        }

                //        int newRow = row + i;
                //        int newCol = col + j;

                //        if (InRange(newRow, newCol, ROW, COL))
                //        {
                //            if (IsGoal(newRow, newCol, goal))
                //            {
                //                cells[newRow, newCol].parent_i = row;
                //                cells[newRow, newCol].parent_j = col;
                //                Console.SetCursorPosition(0, ROW + 2);
                //                Trace(cells, new Node(newRow, newCol));
                //                Console.WriteLine($"The goal has been reached at {newRow}, {newCol}");
                //                foundGoal = true;
                //                return;
                //            }

                //            if (i != 0 && j != 0)
                //            {
                //                if (!IsWalkable(grid, row + i, col) || !IsWalkable(grid, row, col + j))
                //                    continue;
                //            }

                //            if (!closed[newRow, newCol] && IsWalkable(grid, newRow, newCol))
                //            {
                //                double gNew = cells[row, col].g + (i != 0 && j != 0 ? _diagCost : _baseCost);
                //                double hNew = CalculateH(newRow, newCol, goal);
                //                double fNew = gNew + hNew;

                //                if (cells[newRow, newCol].f == double.MaxValue || cells[newRow, newCol].f > fNew)
                //                {
                //                    open.Enqueue(new Node(newRow, newCol), fNew);

                //                    cells[newRow, newCol].f = fNew;
                //                    cells[newRow, newCol].g = gNew;
                //                    cells[newRow, newCol].h = hNew;
                //                    cells[newRow, newCol].parent_i = row;
                //                    cells[newRow, newCol].parent_j = col;
                //                }

                //            }
                //        }
                //    }
                //}
            }

            if (!foundGoal)
            {
                Console.WriteLine("Could not find goal");
            }
        }

        private bool InRange(int row, int col, int ROW, int COL)
        {
            return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
        }

        private bool IsGoal(int row, int col, Node goal)
        {
            return goal.Row == row && goal.Col == col;
        }

        private double CalculateH(int row, int col, Node goal)
        {
            double dx = Math.Abs(row - goal.Row);
            double dy = Math.Abs(col - goal.Col);
            
            return _baseCost * (dx + dy) + (_diagCost - 2 * _baseCost) * Math.Min(dx, dy);
        }

        private bool IsWalkable(bool[,] grid, int row, int col)
        {
            return grid[row, col];
        }
    }
}
