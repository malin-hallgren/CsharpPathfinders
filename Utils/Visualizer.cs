using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Visualizer
    {
        /// <summary>
        /// Traces the path taken to reach the goal and prints it onto the map and as a string
        /// </summary>
        /// <param name="cells">array containing visited cells with parent nodes</param>
        /// <param name="goal">Goal node</param>
        /// <param name="start">Start node</param>
        /// <param name="lines">the maze as strings by row</param>
        static public void Trace(Cell[,] cells, Node goal, Node start, string[] lines)
        {
            Driver.stopWatch.Stop(); //stop watch here as the algorithm is now done and all this is visualization

            int row = goal.Row;
            int col = goal.Col;
            int idx = 0;

            // Counts the steps taken
            while (!(cells[row, col].parent_i == row && cells[row, col].parent_j == col))
            {
                idx++;
                int temp_r = cells[row, col].parent_i;
                int temp_c = cells[row, col].parent_j;

                row = temp_r;
                col = temp_c;
            }

            idx++; //starting spot needs to be counted

            // Creates array to hold the path
            var path = new (int, int)[idx];
            row = goal.Row;
            col = goal.Col;

            bool[,] onPath = new bool[lines.Length, lines[0].Length];

            int i = idx - 1; //we start adding at last index of the array

            // Adds the path to the array, in reverse so that we get the first step first
            while (!(cells[row, col].parent_i == row && cells[row, col].parent_j == col))
            {
                path[i--] = (row, col); //adds coords set up in last itteration/outside loop

                if (row >= 0 && row < onPath.GetLength(0) && col >= 0 && col < onPath.GetLength(1))
                {
                    onPath[row, col] = true;
                }

                int temp_r = cells[row, col].parent_i;
                int temp_c = cells[row, col].parent_j;
                

                row = temp_r;
                col = temp_c;
            }

            path[0] = (row, col); //adds start point
            onPath[row, col] = true;

            WritePath(path, onPath, goal, start, lines);
        }

        /// <summary>
        /// Writes the path onto the map and as a diagnostics string
        /// </summary>
        /// <param name="path">Array containing the path as a ints</param>
        /// <param name="goal">the goal node</param>
        /// <param name="start">the start node</param>
        /// <param name="lines">the maze as strings by row</param>
        static private void WritePath((int, int)[] path, bool[,] onPath, Node goal, Node start, string[] lines)
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
                    else if (onPath[r, c] == true)
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
