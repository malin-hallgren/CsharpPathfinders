using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfinderTest
{
    internal class Layout
    {
        public string selected;
        public string[] mazes =
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
                    grid[i, j] = (c == ' ');
                }
            }
            return grid;
        }
    }
}
