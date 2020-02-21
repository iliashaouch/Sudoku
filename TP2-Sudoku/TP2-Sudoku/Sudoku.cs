using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_Sudoku
{
    public struct cell
    {
        public int value;
        public List<int> possibleValues;
    }

    class Sudoku
    {

        public Sudoku()
        {
            int[,] grid = {
                { 3, 2, 1, 7, 0, 4, 0, 0, 0 },
                { 6, 4, 0, 0, 9, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 4, 5, 9, 0, 0 },
                { 0, 0, 5, 1, 8, 7, 4, 0, 0 },
                { 0, 0, 4, 9, 6, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 0, 7, 0, 0, 1, 9 },
                { 0, 0, 0, 6, 0, 9, 5, 8, 2 }
            };
        }

        public int[,] solveSudoku(int[,] sudokuini)
        {

            return sudokuini;
        }


        public static List<int[]> MRV(cell[,] sudoku)
        {
            List<int[]> rep = new List<int[]>();
            int minimum = sudoku.GetLength(0);
            for (int i = 0; i < sudoku.GetLength(1); i++)
            {
                for (int j = 0; j < sudoku.GetLength(0); j++)
                {
                    if (sudoku[i, j].value == 0)
                    {
                        if (sudoku[i, j].possibleValues.Count == minimum)
                        {
                            rep.Add(new int[] { i, j });
                        }
                        if (sudoku[i, j].possibleValues.Count < minimum)
                        {
                            minimum = sudoku[i, j].possibleValues.Count;
                            rep = new List<int[]>();
                            rep.Add(new int[] { i, j });
                        }
                    }
                }
            }
            Console.WriteLine(rep);
            return rep;
        }

        public bool testSolution(int[,] solution)
        {
            if (!testSolutionLines(solution) || !testSolutionColumns(solution) || !testSolutionBlocs(solution))
            {
                return false;
            }
            return true;
        }

        public bool testSolutionLines(int[,] solution)
        {
            for (int i = 0; i < solution.GetLength(0); i++)
            {
                for (int j = 0; j < solution.GetLength(1); j++)
                    for (int k = j + 1; k < solution.GetLength(1); k++)
                    {
                        if (solution[i, j] == solution[i, k])
                        {
                            return false;
                        }
                    }
            }
            return true;
        }

        public bool testSolutionColumns(int[,] solution)
        {
            for (int i = 0; i < solution.GetLength(1); i++)
            {
                for (int j = 0; j < solution.GetLength(0); j++)
                {
                    for (int k = j + 1; k < solution.GetLength(0); k++)
                    {
                        if (solution[j, i] == solution[k, i])
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public bool testSolutionBlocs(int[,] solution)
        {
            for (int a = 0; a < solution.GetLength(0) / 3; a++)
            {
                for (int b = 0; b < solution.GetLength(1) / 3; b++)
                {
                    for (int j = 0; j < solution.GetLength(0); j++)
                    {
                        for (int k = j + 1; k < solution.GetLength(0); k++)
                        {
                            if (solution[j / 3 + a * 3, j % 3 + b * 3] == solution[k / 3 + a * 3, k % 3 + b * 3])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public static cell[,] gridToCells(int[,] grid)
        {
            cell[,] rep = new cell[grid.GetLength(0), grid.GetLength(1)];
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    rep[i, j].value = grid[i, j];
                    rep[i, j].possibleValues = getPossibleValues(grid, new int[] { i, j });
                }
            }
            return rep;
        }

        public static List<int> getPossibleValues(int[,] grid, int[] pos)
        {
            List<int> rep = new List<int>();
            if (grid[pos[0], pos[1]] != 0)
            {
                return rep;
            }
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                rep.Add(i);
            }
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                if (grid[i, pos[1]] != 0 && rep.Contains(grid[i, pos[1]]))
                {
                    rep.Remove(grid[i, pos[1]]);
                }
                if (grid[pos[0], i] != 0 && rep.Contains(grid[pos[0], i]))
                {
                    rep.Remove(grid[pos[0], i]);
                }
                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (grid[x, y] != 0 && rep.Contains(grid[x, y]))
                {
                    rep.Remove(grid[x, y]);
                }
            }
            return rep;
        }
    }
}
