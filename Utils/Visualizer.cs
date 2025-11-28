using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Visualizer
    {

        static public void Trace(Cell[,] cells, Node goal, Node start, string[] lines)
        {
            Driver.stopWatch.Stop(); //stop watch here as the algorithm is now done and all this is visualization

            //var Path = new Stack<(int, int)>();

            int row = goal.Row;
            int col = goal.Col;
            int idx = 0;

            while (!(cells[row, col].parent_i == row && cells[row, col].parent_j == col))
            {
                idx++;
                int temp_r = cells[row, col].parent_i;
                int temp_c = cells[row, col].parent_j;

                row = temp_r;
                col = temp_c;
            }

            idx++; //starting spot needs to be counted

            var path = new (int, int)[idx];
            row = goal.Row;
            col = goal.Col;

            int i = idx - 1; //we start adding at last index of the array

            while (!(cells[row, col].parent_i == row && cells[row, col].parent_j == col))
            {
                path[i--] = (row, col); //adds coords set up in last itteration/outside loop

                int temp_r = cells[row, col].parent_i;
                int temp_c = cells[row, col].parent_j;

                row = temp_r;
                col = temp_c;
            }

            path[0] = (row, col); //adds start point


            WritePath(path, goal, start, lines);
        }

        static private void WritePath((int, int)[] path, Node goal, Node start, string[] lines)
        {
            var sb = new StringBuilder(64);
            sb.Append("Goal reached in ").Append(path.Length - 1).Append($" steps\nTime elapsed: ").Append(Driver.stopWatch.Elapsed.Microseconds).Append($" Microseconds\nGoal point was at {goal.Row}, {goal.Col}\nPath taken ");

            for (int i = 0; i < path.Length; i++)
            {
                var step = path[i];

                sb.Append('(').Append(step.Item1).Append(',').Append(step.Item2).Append(')');
                if (i + 1 < path.Length)
                {
                    sb.Append("->");
                }
            }

            sb.Append("\n\n");

            for(int r = 0; r < lines.GetLength(0); r++)
            {
                string line = lines[r];
                for (int c = 0; c < line.Length; c++)
                {
                    if (r == start.Row && c == start.Col)
                    {
                        sb.Append('S');
                    }
                    else if (r == goal.Row && c == goal.Col)
                    {
                        sb.Append('G');
                    }
                    else if (path.Contains((r, c))) //Find better solution for this, bool array for checking?
                    {
                        sb.Append('o');
                    }
                    else
                    {
                        sb.Append(line[c]);
                    }
                }
                sb.Append("\n");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
