using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Visualizer
    {

        static public void Trace(Cell[,] cells, Node goal, Node start, string[] lines)
        {
            //stack <int,int>?
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

            (int, int)[] pathTaken = new (int, int)[Path.Count];
            while (Path.Count > 0)
            {
                var step = Path.Pop();
                pathTaken[stepAmount] = (step.Row, step.Col);
                stepAmount++;
                //Console.Write($" -> {step.Row}, {step.Col}");
            }

            WritePath(pathTaken, goal, start, lines);
        }

        static private void WritePath((int, int)[] path, Node goal, Node start, string[] lines)
        {
            //(int, int) defaultCursor = Console.GetCursorPosition();

            //foreach (var step in path) //bake this into display of the map
            //{
            //    Console.SetCursorPosition(step.Item2, step.Item1);

            //    if ((step.Item1 == goal.Row && step.Item2 == goal.Col) || (step.Item1 == start.Row && step.Item2 == start.Col))
            //    {
            //        Console.Write("X");
            //    }
            //    else
            //    {
            //        Console.Write("o");
            //    }
            //}

            //Console.SetCursorPosition(defaultCursor.Item1, defaultCursor.Item2);

            var sb = new StringBuilder(64);
            sb.Append("Goal reached in ").Append(path.Length).Append($" steps\nGoal point was at {goal.Row}, {goal.Col}\nPath taken ");

            for (int i = 0; i < path.Length; i++)
            {
                var step = path[i];

                sb.Append('(').Append(step.Item1).Append(',').Append(step.Item2).Append(')');
                if (i + 1 < path.Length)
                {
                    sb.Append("->");
                }
            }

            var rows = lines.Select(l => l.ToCharArray()).ToArray();
            foreach (var (r, c) in path)
            {
                var row = rows[r];
                if (r == start.Row && c == start.Col)
                {
                    row[c] = 'S';
                }
                else if (r == goal.Row && c == goal.Col)
                {
                    row[c] = 'G';
                }
                else
                {
                    row[c] = 'o';
                }
            }

            //another stringbuilder?
            foreach (var row in rows)
            {
                Console.WriteLine(new string(row));
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
