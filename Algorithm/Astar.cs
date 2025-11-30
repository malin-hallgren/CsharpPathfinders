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


        bool shouldVisualize = true;

        public Astar(double baseCost = 1, double diagCost = 1.414)
        {
            _baseCost = baseCost;
            _diagCost = diagCost;
        }

        /// <summary>
        /// A' algorithm, squared Euclidean movement
        /// </summary>
        /// <param name="grid">The generated grid denominating walkable or blocked squares</param>
        /// <param name="src">the starting node</param>
        /// <param name="goal">the goal node</param>
        /// <param name="lines">all lines of the maze in a string array</param>
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


            //creates 2D array of cells for the grid
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

            //sets the source cell to be 0'ed out with no parent
            cells[src.Row, src.Col].f = 0;
            cells[src.Row, src.Col].g = 0;
            cells[src.Row, src.Col].h = 0;
            cells[src.Row, src.Col].parent_i = src.Row;
            cells[src.Row, src.Col].parent_j = src.Col;

            //Item 1 is is the node, Item 2 is f value of the cell, order updates as value updates(?)
            PriorityQueue<Node, double> open = new PriorityQueue<Node, double>();

            open.Enqueue(new Node(src.Row, src.Col), 0);

            bool foundGoal = false;

            while (open.Count > 0)
            {
                Node current;

                //create a node from the current top post in the Priority queue and extract values to individual variables
                while(open.TryDequeue(out current, out var fCurrent))
                {
                    int r = current.Row;
                    int c = current.Col;

                    //check current this is stale or irrelevant, higher f means not a good option/exists with better f value
                    if(fCurrent > cells[r, c].f)
                    {
                        continue;
                    }

                    //check if current is already closed and skip ahead if it is, closes if it isn't
                    if (closed[current.Row, current.Col])
                    {
                        continue;
                    }
 
                    closed[r, c] = true;


                    //Checks all the neighbours
                    foreach (var neighbour in Neighbouring)
                    {
                        int newRow = r + neighbour.dr;
                        int newCol = c + neighbour.dc;

                        //If we can't walk on a specific neighbour, continue to next itteration
                        if (neighbour.dr != 0 && neighbour.dc != 0)
                        {
                            if (!IsWalkable(grid, r + neighbour.dr, c) || !IsWalkable(grid, r, c + neighbour.dc))
                                continue;
                        }

                        //Checks if this neighbour is in range
                        if (InRange(newRow, newCol, ROW, COL))
                        {
                            //checks if this neighbour is the goal
                            if (IsGoal(newRow, newCol, goal))
                            {
                                cells[newRow, newCol].parent_i = r;
                                cells[newRow, newCol].parent_j = c;
                                
                                //Visualize only if the setting is true
                                if (shouldVisualize)
                                {
                                    Visualizer.Trace(cells, new Node(newRow, newCol), src, lines);
                                }
                                
                                foundGoal = true;
                                return;
                            }

                            //If the neighbour is not the goal and is walkable, get new g, calculate new h and f, set them to this cell, add to cell list
                            //enqueue to the open list (it will dequeue on next loop itteration)
                            if (!closed[newRow, newCol] && IsWalkable(grid, newRow, newCol))
                            {
                                double gNew = cells[r, c].g + (neighbour.dr != 0 && neighbour.dc != 0 ? _diagCost : _baseCost);
                                double hNew = CalculateH(newRow, newCol, goal);
                                double fNew = gNew + hNew;

                                if (cells[newRow, newCol].f == double.MaxValue || cells[newRow, newCol].f > fNew)
                                {
                                    open.Enqueue(new Node(newRow, newCol), fNew);

                                    cells[newRow, newCol].f = fNew;
                                    cells[newRow, newCol].g = gNew;
                                    cells[newRow, newCol].h = hNew;
                                    cells[newRow, newCol].parent_i = r;
                                    cells[newRow, newCol].parent_j = c;
                                }
                            }
                        }
                    }
                }
            }

            //Fallback if something goes very wrong!
            if (!foundGoal)
            {
                Console.WriteLine("Could not find goal");
            }
        }

        /// <summary>
        /// Checks whether a potential step is inside the range of the grid
        /// </summary>
        /// <param name="row">row value</param>
        /// <param name="col">col value</param>
        /// <param name="ROW">amount of rows in grid</param>
        /// <param name="COL">amount of columns in grid</param>
        /// <returns>true if the step is inside the grid</returns>
        private bool InRange(int row, int col, int ROW, int COL)
        {
            return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
        }

        /// <summary>
        /// Checks if the current position matches goal
        /// </summary>
        /// <param name="row">current row value</param>
        /// <param name="col">current column value</param>
        /// <param name="goal">the goal node with coordinates</param>
        /// <returns></returns>
        private bool IsGoal(int row, int col, Node goal)
        {
            return goal.Row == row && goal.Col == col;
        }

        /// <summary>
        /// Calculate the heuristic for a certain cell in relation to the goal
        /// </summary>
        /// <param name="row">current row</param>
        /// <param name="col">current column</param>
        /// <param name="goal">the goal node</param>
        /// <returns></returns>
        private double CalculateH(int row, int col, Node goal)
        {
            double dx = Math.Abs(row - goal.Row);
            double dy = Math.Abs(col - goal.Col);
            
            return _baseCost * (dx + dy) + (_diagCost - 2 * _baseCost) * Math.Min(dx, dy);
        }

        /// <summary>
        /// Checks if a cell is walkable
        /// </summary>
        /// <param name="grid">the grid in use</param>
        /// <param name="row">current row</param>
        /// <param name="col">current column</param>
        /// <returns></returns>
        private bool IsWalkable(bool[,] grid, int row, int col)
        {
            return grid[row, col];
        }
    }
}
