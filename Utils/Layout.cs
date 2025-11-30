using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Layout
    {
        static private readonly string[] maps = {"../../../Maps/Map1.txt", "../../../Maps/Map2.txt"}; 
        static public string[] mazes =
        {
@"-----------------
|     |         |
| -- ---- |     |
|      |     |  |
| ---- | ----   |
| |    |        |
| | ---- ----   |
| |         |   |
| ---- | ----   |
|    | |        |
| -- | ---- --- |
|    |          |
| ---- ---- ----|
|               |
| ----      ----|
|               |
|               |
-----------------",
@"----------------
|     |        |
| ---- ---- |  |
|      |    |  |
| ---- | ----  |
| |    |       |
| | ---- ----  |
| |        |   |
| ---- | ----  |
|    | |       |
| -- | ---- -- |
|    |         |
| ---- ---- ---|
|              |
| ----     ----|
----------------",
@"----------------
|              |
| -- ---- ---- |
|    |    |    |
| -- | -- | -- |
|    | |  |    |
| --   |  ---- |
| | -- |       |
| |    ---- -- |
| |       |    |
| ---- -- | -- |
|       | |    |
| -- ------- --|
|              |
| ----     ----|
----------------",
@"----------------
| |   |      | |
| | | ---- --- |
| | |       |  |
| --- ---- --- |
|   |       |  |
| -- ---- ---- |
|   |     |    |
| -- ---- -- --|
|       |      |
| ---- -- ---- |
|       |      |
| -- ------- --|
|       |      |
| ---- | ----  |
|              |
|              |
----------------"
        };

        /// <summary>
        /// Gets a maze cut into lines/rows 
        /// </summary>
        /// <param name="rand">The random object to run the picking of the maze on</param>
        /// <returns>a string array containing a maze cut into rows</returns>
        static public string[] GetLines(Random rand)
        {
            string[] lines = mazes[rand.Next(0, mazes.Length - 1)].Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
            }
            return lines;
        }

        /// <summary>
        /// Generates a walkable grid from a maze in string array format
        /// </summary>
        /// <param name="lines">the maze cut into rows as a string array</param>
        /// <returns>a 2D bool array denominating whether a coordinate is walkable or not</returns>
        static public bool[,] GetGrid(string[] lines)
        {
            int rows = lines.GetLength(0);
            int columns = lines[0].Length;

            bool[,] grid = new bool[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    char c = lines[i][j];
                    grid[i, j] = c == ' ';
                }
            }
            return grid;
        }
    }
}
