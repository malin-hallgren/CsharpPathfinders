using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest.Utils
{
    internal class Layout
    {
        public string selected;
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

        public Layout(int index)
        {
            selected = mazes[index];
        }

        static public string[] GetLines(Random rand)
        {
            string[] lines = mazes[rand.Next(0, mazes.Length - 1)].Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd('\r');
            }
            return lines;
        }

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

        public bool[,] GenerateGrid(Layout layout)
        {

            string[] lines = layout.selected.Split('\n')
                                   .Select(l => l.TrimEnd('\r'))
                                   .ToArray();

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
