﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public struct assignment
    {
        public int[] pos;
        public int value;
    }

    class Sudoku
    {
        static int s = 0;
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

        public static (List<assignment>, bool) BackTrackingSearch(cell[,] sudoku)
        {
            return RecursiveBackTracking(new List<assignment>(), sudoku);
        }


        public static (List<assignment>, bool) RecursiveBackTracking(List<assignment> assignments, cell[,] sudoku)
        {
            string ss = "assign=" + assignments.Count + ",";
            //Console.WriteLine("Searching");
            if (testSolution(sudoku))
            {
                return (assignments, true);
            }
            var variableList = SelectUnassignedVariable(sudoku); // rend la liste de variables prioritaires
            ss += "nb position=" + variableList.Count + ",";
            
            if (variableList.Count == 0)
            {
                int IndexOfLastAssignment = assignments.Count - 1;
                sudoku[assignments[IndexOfLastAssignment].pos[0], assignments[IndexOfLastAssignment].pos[1]].value = 0;
                //sudoku[assignments[IndexOfLastAssignment].pos[0], assignments[IndexOfLastAssignment].pos[1]]
                //    .possibleValues.Remove(assignments[IndexOfLastAssignment].value);
                assignments.RemoveAt(IndexOfLastAssignment);
                return (assignments, false);
            }
            var variable = variableList[0];

            var possibleValues = MyCopy(sudoku[variable[0], variable[1]].possibleValues); // SelectUnassignedValue(sudoku, variable[0]); // rend la liste des valeurs possibles pour la première variable
            ss += "nb valeur=" + possibleValues.Count + ",";
            while(possibleValues.Count>0)
            {
                int value = SelectUnassignedValue(sudoku, variable, possibleValues)[0];
                //Console.WriteLine("(" + variable[0] + " , " + variable[1] + ") tested values is : " + value);
                if (ValueRespectsConstraints(value, variable, sudoku))
                {
                    assignment newAssignment = new assignment();
                    newAssignment.pos = variable;
                    newAssignment.value = value;
                    assignments.Add(newAssignment);
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = newAssignment.value;
                    if (s < assignments.Count)
                    {
                        s = assignments.Count;
                    }
                    //Console.WriteLine(ss);
                    printThisSudoku(sudoku);
                    var result = RecursiveBackTracking(assignments, sudoku);
                    if (result.Item2 == true)
                    {
                        return result;
                    }
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = 0;
                    //sudoku[newAssignment.pos[0], newAssignment.pos[1]].possibleValues.Remove(newAssignment.value);
                    assignments.Remove(newAssignment);
                }
                possibleValues = MyRemove(possibleValues,value);
                //Console.WriteLine("(" + variable[0] + " , " + variable[1] + ") possible value : " + possibleValues.Count);
                //Console.WriteLine("(" + variable[0] + " , " + variable[1] + ") possible value : " + sudoku[variable[0], variable[1]].possibleValues.Count);
            }
            return (assignments, false);
        }

        public static List<int> MyCopy(List<int> listToCopy)
        {
            List<int> rep = new List<int>();
            foreach (int i in listToCopy)
            {
                rep.Add(i);
            }
            return rep;
        }

        public static List<int> MyRemove(List<int> listToScroll, int valueToRemove)
        {
            List<int> rep = new List<int>();
            foreach (int i in listToScroll)
            {
                if (i!= valueToRemove)
                {
                    rep.Add(i);
                } 
            }
            return rep;
        }

        public static bool ValueRespectsConstraints(int value, int[] pos, cell[,] sudoku)
        {
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[i, pos[1]].value == value && i != pos[0])
                {
                    return false;
                }
                if (sudoku[pos[0], i].value == value && i != pos[1])
                {
                    return false;
                }
                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (sudoku[x, y].value == value && i != 4)
                {
                    return false;
                }
            }
            return true;
        }

        public static void printThisSudoku(cell[,] sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                String l = "";
                for (int j=0; j < 8; j++)
                {
                    l += sudoku[i, j].value + " ,";
                }
                l += sudoku[i, 8].value;
                Console.WriteLine(l);
            }
        }

        public static bool AssignmentsIsComplete(List<assignment> assignments, cell[,] sudoku)
        {
            bool rep = false;
            if (assignments.Count == nbOfVariablesLeft(sudoku))
            {
                rep = true;
            }
            return rep;
        }

        public static int nbOfVariablesLeft(cell[,] sudoku)
        {
            int rep = 0;
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                for (int j = 0; j < sudoku.GetLength(1); j++)
                {
                    if (sudoku[i, j].value == 0)
                    {
                        rep++;
                    }
                }
            }
            return rep;
        }

        public static List<int[]> SelectUnassignedVariable(cell[,] sudoku)
        {
            List<int[]> rep = MRV(sudoku);
            if (rep.Count != 1)
            {
                rep = DegreeHeuristic(sudoku, rep);
            }
            return rep;
        }

        public static List<int> SelectUnassignedValue(cell[,] sudoku, int[] variablePosition, List<int> possibleValues)
        {
            return LeastConstrainingValue(sudoku, variablePosition, possibleValues);
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
                        else if (sudoku[i, j].possibleValues.Count < minimum)
                        {
                            minimum = sudoku[i, j].possibleValues.Count;
                            rep = new List<int[]>();
                            rep.Add(new int[] { i, j });
                        }
                    }
                }
            }
            //Console.WriteLine(rep.ToString());
            return rep;
        }

        public static List<int[]> DegreeHeuristic(cell[,] sudoku, List<int[]> variables)
        {
            List<int[]> rep = new List<int[]>();
            int maxConstraints = 0;

            foreach (int[] pos in variables)
            {
                int posConstraints = countConstraintsOnOtherVariables(sudoku, pos);
                if (posConstraints == maxConstraints)
                {
                    rep.Add(pos);
                }
                else if (posConstraints > maxConstraints)
                {
                    maxConstraints = posConstraints;
                    rep = new List<int[]>();
                    rep.Add(pos);
                }
            }
            return rep;
        }

        public static int countConstraintsOnOtherVariables(cell[,] sudoku, int[] pos)
        {
            int count = 0;
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[i, pos[1]].value == 0 && i != pos[0])
                {
                    count++;
                }
                if (sudoku[pos[0], i].value == 0 && i != pos[1])
                {
                    count++;
                }

                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (sudoku[x, y].value == 0 && x != pos[0] && y != pos[1])
                {
                    count++;
                }
            }
            return count;
        }

        public static List<int> LeastConstrainingValue(cell[,] sudoku, int[] pos, List<int> possibleValues)
        {
            List<int> rep = new List<int>();
            int minimumCount = sudoku.GetLength(0) * 3;
            foreach (int value in possibleValues)
            {
                int valueCount = countConstraintsOnOtherValues(sudoku, pos, value);
                if (valueCount == minimumCount)
                {
                    rep.Add(value);
                }
                else if (valueCount < minimumCount)
                {
                    minimumCount = valueCount;
                    rep = new List<int>();
                    rep.Add(value);
                }
            }
            return rep;
        }

        public static int countConstraintsOnOtherValues(cell[,] sudoku, int[] pos, int studiedValue)
        {
            int count = 0;
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[i, pos[1]].value == 0 && sudoku[i, pos[1]].possibleValues.Contains(studiedValue) && i != pos[0])
                {
                    count++;
                }
                if (sudoku[pos[0], i].value == 0 && sudoku[pos[0], i].possibleValues.Contains(studiedValue) && i != pos[1])
                {
                    count++;
                }
                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (sudoku[x, y].value == 0 && sudoku[x, y].possibleValues.Contains(studiedValue) && i != 4)
                {
                    count++;
                }
            }
            return count;
        }

        public static bool testSolution(cell[,] solution)
        {
            if (!testSolutionLines(solution) || !testSolutionColumns(solution) || !testSolutionBlocs(solution))
            {
                return false;
            }
            return true;
        }

        public static bool testSolutionLines(cell[,] solution)
        {
            for (int i = 0; i < solution.GetLength(0); i++)
            {
                for (int j = 0; j < solution.GetLength(1); j++)
                    for (int k = j + 1; k < solution.GetLength(1); k++)
                    {
                        if (solution[i, j].value == solution[i, k].value || solution[i,j].value==0)
                        {
                            return false;
                        }
                    }
            }
            return true;
        }

        public static bool testSolutionColumns(cell[,] solution)
        {
            for (int i = 0; i < solution.GetLength(1); i++)
            {
                for (int j = 0; j < solution.GetLength(0); j++)
                {
                    for (int k = j + 1; k < solution.GetLength(0); k++)
                    {
                        if (solution[j, i].value == solution[k, i].value || solution[i,j].value==0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool testSolutionBlocs(cell[,] solution)
        {
            for (int a = 0; a < solution.GetLength(0) / 3; a++)
            {
                for (int b = 0; b < solution.GetLength(1) / 3; b++)
                {
                    for (int j = 0; j < solution.GetLength(0); j++)
                    {
                        for (int k = j + 1; k < solution.GetLength(0); k++)
                        {
                            if (solution[j / 3 + a * 3, j % 3 + b * 3].value == solution[k / 3 + a * 3, k % 3 + b * 3].value)
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
            for (int i = 1; i <= grid.GetLength(0); i++)
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
            String l = "(" + pos[0] + " , " + pos[1] + ") possible values  are : ";
            
            foreach (int i in rep)
            {
                l += i + " , ";
            }
            //Console.WriteLine(l);
            return rep;
        }
    }
}
