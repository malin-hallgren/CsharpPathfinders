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

        static public string[] GetLines(Random rand)
        {
            string path = maps[rand.Next(0, maps.Length)];

            try
            {
                //not performant, consider replacing for the array
                string[] lines = File.ReadAllLines(path);

                if (lines.Length == 0)
                {
                    throw new Exception("Maze must have a length greater than 0");
                }
                if (!lines.All(x => x.Length == lines[0].Length))
                {
                    throw new Exception("Maze must be uniform");
                }
                    
                return lines;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"{ex.Message}\nGenerating standard maze...");

                string[] lines = new string[]
                {
                    "-----------------",
                    "|     |         |",
                    "| -- ---- |     |",
                    "|      |     |  |",
                    "| ---- | ----   |",
                    "| |    |        |",
                    "| | ---- ----   |",
                    "| |         |   |",
                    "| ---- | ----   |",
                    "|    | |        |",
                    "| -- | ---- --- |",
                    "|    |          |",
                    "| ---- ---- ----|",
                    "|               |",
                    "| ----      ----|",
                    "|               |",
                    "|               |",
                    "-----------------"
                };
                return lines;
            }
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
