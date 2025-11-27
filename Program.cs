using System.Reflection;

namespace PathfinderTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var random = new Random();
            var layout = new Layout(random.Next(0,4));
            bool[,] grid = layout.GenerateGrid(layout);
            var goalPos = SetGoal(random, grid);
            Console.WriteLine(layout.selected);

            AStar(grid, new Node(1, 1), new Node(goalPos.Item1, goalPos.Item2));
           
        }

        static private (int, int) SetGoal(Random rand, bool[,] passable)
        {
            while (true)
            {
                (int, int) goal = (rand.Next(1, 16), rand.Next(1, 16));
                if (!passable[goal.Item1, goal.Item2])
                {
                    continue;
                }
                return (goal.Item1, goal.Item2);
            }
 
        }

        static private bool IsGoal(int row, int col, Node goal)
        {
            return goal.Row == row && goal.Col == col;

        }

        static private void Trace(Cell[,] cells, Node goal)
        {
            var Path = new Stack<Node>();

            int row = goal.Row;
            int col = goal.Col;

            while (!(cells[row, col].parent_i == row && cells[row, col].parent_j == col))
            {
                Path.Push(new Node(row, col));
                int temp_x = cells[row, col].parent_i;
                int temp_y = cells[row, col].parent_j;
                row = temp_x;
                col = temp_y;
            }

            int stepAmount = 0;
            Path.Push(new Node(row, col));
            while (Path.Count > 0)
            {
                var step = Path.Pop();
                stepAmount++;
                WritePath(step, goal, new Node(1, 1));
                Console.Write($" -> {step.Row}, {step.Col}");
            }

            Console.WriteLine($"\nGoal reached in {stepAmount} steps");
        }

        static private void WritePath(Node step, Node goal, Node start)
        {
            (int, int) defaultCursor = Console.GetCursorPosition();
            Console.SetCursorPosition(step.Col, step.Row);
            if ((step.Row == goal.Row && step.Col == goal.Col) || (step.Row == start.Row && step.Col == start.Col))
            {
                Console.Write("X");
            }
            else
            {
                Console.Write("o");
            }    
            Console.SetCursorPosition(defaultCursor.Item1, defaultCursor.Item2);
        }

        static private bool InRange(int row, int col, int ROW, int COL)
        {
            return (row >= 0) && (row < ROW) && (col >= 0) && (col < COL);
        }

        static private double CalculateH(int row, int col, Node goal)
        {
            return Math.Sqrt(Math.Pow(row - goal.Row, 2) + Math.Pow(col - goal.Col, 2));
        }

        static private bool IsWalkable(bool[,] grid, int row, int col)
        {
            return grid[row, col];
        }

        static private void AStar(bool[,] grid, Node src, Node goal)
        {
            int ROW = grid.GetLength(0);
            int COL = grid.GetLength(1);

            if(!InRange(src.Row, src.Col, ROW, COL) || !InRange(goal.Row, goal.Col, ROW, COL))
            {
                Console.WriteLine("Start or destination out of range");
                return;
            }
            if(src.Row == goal.Row && src.Col == goal.Col)
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
                Node current = open.Dequeue();

                int row = current.Row;
                int col = current.Col;
                closed[row, col] = true;

                for(int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if(i == 0 && j == 0)
                        {
                            continue;
                        }

                        int newRow = row + i;
                        int newCol = col + j;

                        if(InRange(newRow, newCol, ROW, COL))
                        {
                            if(IsGoal(newRow, newCol, goal))
                            {
                                cells[newRow, newCol].parent_i = row;
                                cells[newRow, newCol].parent_j = col;
                                Console.SetCursorPosition(0, ROW + 2);
                                Trace(cells, new Node(newRow, newCol));
                                Console.WriteLine($"The goal has been reached at {newRow}, {newCol}");
                                foundGoal = true;
                                return;
                            }

                            if (i != 0 && j != 0)
                            {
                                if (!IsWalkable(grid, row + i, col) || !IsWalkable(grid, row, col + j))
                                    continue;
                            }

                            if (!closed[newRow, newCol] && IsWalkable(grid, newRow , newCol))
                            {
                                double gNew = cells[row, col].g + 1;
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
            }

            if(!foundGoal)
            {
                Console.WriteLine("Could not find goal");
            }
        }
    }
}
