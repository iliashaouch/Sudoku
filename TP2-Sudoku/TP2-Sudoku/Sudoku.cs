using System;
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

        public static (List<assignment>, bool) BackTrackingSearch(cell[,] sudoku)
        {
            sudoku = AC3(sudoku);
            return RecursiveBackTracking(new List<assignment>(), sudoku);
        }

        public static cell[,] AC3(cell[,] sudoku)
        {
            List<int[,]> queue = getAllArcs(sudoku);
            while (queue.Count > 0)
            {
                int[,] Arc = queue[0];
                queue.RemoveAt(0);
                if (needToRemoveInconsistantValue(sudoku, Arc))
                {
                    int[] pos = { Arc[0, 0], Arc[0, 1] };
                    List<int[]> neighbors = getAllNeighbors(sudoku, pos);
                    foreach (int[] neighbor in neighbors)
                    {
                        int[,] newArc = new int[2, 2];
                        newArc[0, 0] = pos[0];
                        newArc[0, 1] = pos[1];
                        newArc[1, 0] = neighbor[0];
                        newArc[1, 1] = neighbor[1];
                        if (!queue.Contains(newArc))
                        {
                            queue.Add(newArc);
                        }
                    }
                }
            }

            return sudoku;
        }

        public static bool needToRemoveInconsistantValue(cell[,] sudoku, int[,] Arc)
        {
            bool neededToRemove = false;
            int[] pos = { Arc[0, 0], Arc[0, 1] };
            int[] target = { Arc[1, 0], Arc[1, 1] };
            if (sudoku[target[0], target[1]].possibleValues.Count == 1
                && sudoku[pos[0], pos[1]].possibleValues.
                Contains(sudoku[target[0], target[1]].possibleValues[0]))
            {
                sudoku[pos[0], pos[1]].possibleValues.
                    Remove(sudoku[target[0], target[1]].possibleValues[0]);
                neededToRemove = true;
            }
            return neededToRemove;
        }

        public static List<int[,]> getAllArcs(cell[,] sudoku)
        {
            List<int[,]> rep = new List<int[,]>();

            for (int l = 0; l < sudoku.GetLength(0); l++)
            {
                for (int c = 0; c < sudoku.GetLength(1); c++)
                {
                    int[] pos = { l, c };
                    List<int[]> neighbors = getAllNeighbors(sudoku, pos);
                    foreach (int[] neighbor in neighbors)
                    {
                        int[,] Arc = new int[2, 2];
                        Arc[0, 0] = pos[0];
                        Arc[0, 1] = pos[1];
                        Arc[1, 0] = neighbor[0];
                        Arc[1, 1] = neighbor[1];
                        rep.Add(Arc);
                    }
                }
            }
            return rep;
        }

        public static List<int[]> getAllNeighbors(cell[,] sudoku, int[] pos)
        {
            List<int[]> rep = new List<int[]>();
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[i, pos[1]].value == 0 && i != pos[0])
                {
                    rep.Add(new int[] { i, pos[1] });
                }
                if (sudoku[pos[0], i].value == 0 && i != pos[1])
                {
                    rep.Add(new int[] { pos[0], i });
                }
                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (sudoku[x, y].value == 0 && x!= pos[0] && y!= pos[1])
                {
                    rep.Add(new int[] { x, y });
                }
            }

            return rep;
        }



        public static (List<assignment>, bool) RecursiveBackTracking(List<assignment> assignments, cell[,] sudoku)
        {
            if (testSolution(sudoku))
            {
                return (assignments, true);
            }
            var variableList = SelectUnassignedVariable(sudoku); // rend la liste de variables prioritaires
            
            if (variableList.Count == 0)
            {
                int IndexOfLastAssignment = assignments.Count - 1;
                sudoku[assignments[IndexOfLastAssignment].pos[0], assignments[IndexOfLastAssignment].pos[1]].value = 0;
                assignments.RemoveAt(IndexOfLastAssignment);
                return (assignments, false);
            }
            var variable = variableList[0];

            var possibleValues = MyCopy(sudoku[variable[0], variable[1]].possibleValues); // rend la liste des valeurs possibles pour la première variable
            while(possibleValues.Count>0)
            {
                int value = SelectUnassignedValue(sudoku, variable, possibleValues)[0];
                if (ValueRespectsConstraints(value, variable, sudoku))
                {
                    assignment newAssignment = new assignment();
                    newAssignment.pos = variable;
                    newAssignment.value = value;
                    assignments.Add(newAssignment);
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = newAssignment.value;
                    //Console.WriteLine("");
                    //printThisSudoku(sudoku);
                    //Console.WriteLine("");
                    var result = RecursiveBackTracking(assignments, sudoku);
                    if (result.Item2 == true)
                    {
                        return result;
                    }
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = 0;
                    assignments.Remove(newAssignment);
                }
                possibleValues = MyRemove(possibleValues,value);
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
                if (sudoku[x, y].value == value && x != pos[0] && y != pos[1])
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
                if (sudoku[x, y].value == 0 && sudoku[x, y].possibleValues.Contains(studiedValue) && x != pos[0] && y != pos[1])
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
            return rep;
        }
    }
}
