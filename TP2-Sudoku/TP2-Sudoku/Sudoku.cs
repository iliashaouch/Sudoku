using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2_Sudoku
{
    // la structure cell représent une cellule de notre sudoku, elle contient sa valeurs et ses valeurs possibles si sa valeurs originale était "0" (vide)
    public struct cell
    {
        public int value;
        public List<int> possibleValues;
    }

    // la structure assignment correspond à l'assignement d'une valeur dans une case par notre programme, elle comprend la position de la case en question et sa valeur assignée
    public struct assignment
    {
        public int[] pos;
        public int value;
    }

    class Sudoku
    {

        // la fonction BacktrackingSearch prend en paramètre un sudoku et rend la liste des assignments attribués pour le compléter et un booléen 
        // indiquant si la solution à bien été trouvée
        public static (List<assignment>, bool) BackTrackingSearch(cell[,] sudoku)
        {
            sudoku = AC3(sudoku); // On commence par réaliser un AC-3 sur notre sudoku afin de réduire le nombre de valeurs possibles originalement
            var rep = RecursiveBackTracking(new List<assignment>(), sudoku);
            return rep; // on appelle ensuite la fonction RecursiveBackTracking qui résoudra le sudoku
        }

        public static cell[,] AC3(cell[,] sudoku)
        {
            // On créer une liste "queue" de tous les Arc, chaque Arc est constitué d'une liste de deux position ((x1,y1),(x2,y2)) de cases non assignés s'influençant
            List<int[,]> queue = getAllArcs(sudoku);
            while (queue.Count > 0) // Tant que la liste "queue" contient des éléments
            {
                int[,] Arc = queue[0];
                queue.RemoveAt(0); // on étudie le premier élément de "queue" que l'on supprime de cette dernière 
                if (needToRemoveInconsistantValue(sudoku, Arc)) // Si l'étude de cette Arc provoque des changements dans les valeurs possibles de sa première case
                {
                    int[] pos = { Arc[0, 0], Arc[0, 1] };
                    List<int[]> neighbors = getAllNeighbors(sudoku, pos); // on prend la liste de toutes les cases influencés par la case étudiée (première case de l'arc)
                    foreach (int[] neighbor in neighbors) // pour chacun de ces "voisins" on ajoute un nouvelle arc constitué de la case étudié et de ce voisin  à la liste "queue"
                    {
                        int[,] newArc = new int[2, 2];
                        newArc[1, 0] = pos[0];
                        newArc[1, 1] = pos[1];
                        newArc[0, 0] = neighbor[0];
                        newArc[0, 1] = neighbor[1];
                        if (!queue.Contains(newArc))
                        {
                            queue.Add(newArc);
                        }
                    }
                }
            }
            return sudoku;
        }

        // la fonction "needToRemoveInconsistantValue" prend en variable un sudoku et un arc et vérifie que la deuxième case de cette arc 
        // n'entraine pas l'invalidation d'une des variables possible de première case de l'arc. Si c'est le cas, elle effectue la modification 
        // dans les valeurs possibles de la première case et rend true (needed to remove)
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

        // La fonction getAllArcs prend en paramètre un sudoku et rend la liste de tous ses Arc, c'est à dire que que pour chaqe case du sudoku, 
        // elle rend un arc pour chacun de ses "voisines" (les cases qu'elle influence)
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

        // La fonction getAllNeighbors prend en paramètre un sudoku et la position d'une case à étudier et rend la liste de toutes les cases qu'elle influence
        // (toute ses "voisines")
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
                if (sudoku[x, y].value == 0 && x != pos[0] && y != pos[1])
                {
                    rep.Add(new int[] { x, y });
                }
            }

            return rep;
        }


        // la fonction RecursiveBacktracking prend une liste d'assignements et un sudoku et rend une liste de commentaire et un booléen indiquant si la solution à été trouvée
        public static (List<assignment>, bool) RecursiveBackTracking(List<assignment> assignments, cell[,] sudoku)
        {
            // On test le sudoku pour savoir si il s'agit de la solution
            if (testSolution(sudoku))
            {
                return (assignments, true);
            }
            var variableList = SelectUnassignedVariable(sudoku); // rend la liste de variables prioritaires

            if (variableList.Count == 0) // si aucune variable n'a été trouvé on revient en arrière
            {
                int IndexOfLastAssignment = assignments.Count - 1;
                sudoku[assignments[IndexOfLastAssignment].pos[0], assignments[IndexOfLastAssignment].pos[1]].value = 0;
                assignments.RemoveAt(IndexOfLastAssignment);
                return (assignments, false);
            }
            var variable = variableList[0]; // On prend la première variable

            var possibleValues = MyCopy(sudoku[variable[0], variable[1]].possibleValues); // rend la liste des valeurs possibles pour la première variable
            while (possibleValues.Count > 0) // Tant que l'on a pas testé l'ensemble des valeurs possibles pour cette variable :
            {
                int value = SelectUnassignedValue(sudoku, variable, possibleValues)[0];
                if (ValueRespectsConstraints(value, variable, sudoku)) // si cette valeur respecte les règles du sudoku :
                {
                    // on ajoute cette assignement à la liste d'assignements et on rapelle la foncion RecursiveBackTracking
                    assignment newAssignment = new assignment();
                    newAssignment.pos = variable;
                    newAssignment.value = value;
                    assignments.Add(newAssignment);
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = newAssignment.value;
                    //Console.WriteLine("");
                    //printThisSudoku(sudoku);
                    //Console.WriteLine("");
                    var result = RecursiveBackTracking(assignments, sudoku);
                    if (result.Item2 == true) // si la solution à été trouvé on remonte
                    {
                        return result;
                    }
                    // sinon on retire cette assignement
                    sudoku[newAssignment.pos[0], newAssignment.pos[1]].value = 0;
                    assignments.Remove(newAssignment);
                }
                // on retire la valeure testée de la liste des valeurs possibles
                possibleValues = MyRemove(possibleValues, value);
            }
            // si l'étude d'aucune valeur pour cette variable n'a été concluente on remonte
            return (assignments, false);
        }

        // Cette fonction est une fonction de copie de liste personnalisée
        public static List<int> MyCopy(List<int> listToCopy)
        {
            List<int> rep = new List<int>();
            foreach (int i in listToCopy)
            {
                rep.Add(i);
            }
            return rep;
        }

        // Cette fonction est une fonction retirant l'element recherché d'une liste donnée
        public static List<int> MyRemove(List<int> listToScroll, int valueToRemove)
        {
            List<int> rep = new List<int>();
            foreach (int i in listToScroll)
            {
                if (i != valueToRemove)
                {
                    rep.Add(i);
                }
            }
            return rep;
        }

        // Cette fonction vérifie si l'attribution d'une valeur donnée dans une case donnée (de coordonnée "pos") dans un sudoku respecte les règles du sudoku
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

        // Cette fonction permet l'affichage console d'un sudoku
        public static void printThisSudoku(cell[,] sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                String l = "";
                for (int j = 0; j < 8; j++)
                {
                    l += sudoku[i, j].value + " ,";
                }
                l += sudoku[i, 8].value;
                Console.WriteLine(l);
            }
        }

        // cette fonction rend une liste de Variables non assignés choisis via un MRV suivie d'un degree heuristic en cas d'égalité
        public static List<int[]> SelectUnassignedVariable(cell[,] sudoku)
        {
            List<int[]> rep = MRV(sudoku);
            if (rep.Count != 1)
            {
                rep = DegreeHeuristic(sudoku, rep);
            }
            return rep;
        }

        // cette fonction rend une liste de valeurs pour la position indiqués ("variablePosition") parmis la liste de ses valeurs possibles 
        // fournis ("possibleValues") choisis via Least constraining value
        public static List<int> SelectUnassignedValue(cell[,] sudoku, int[] variablePosition, List<int> possibleValues)
        {
            return LeastConstrainingValue(sudoku, variablePosition, possibleValues);
        }

        // La fonction MRV (minimum remaining values)prend en paramètre un sudoku et rends une liste de position des cases 
        // ayant le moins de valeurs possibles
        public static List<int[]> MRV(cell[,] sudoku)
        {
            List<int[]> rep = new List<int[]>();
            int minimum = sudoku.GetLength(0); // "minimum" est le nombre de valeurs possibles minimum trouvé pour le moment
            for (int i = 0; i < sudoku.GetLength(1); i++)
            {
                for (int j = 0; j < sudoku.GetLength(0); j++)
                {
                    if (sudoku[i, j].value == 0)
                    {
                        if (sudoku[i, j].possibleValues.Count == minimum) // si le nombre de valeurs possibles d'une case est égale au minimum,
                        {
                            rep.Add(new int[] { i, j }); // on ajoute cette case à la liste réponse
                        }
                        else if (sudoku[i, j].possibleValues.Count < minimum) // si le nombre de valeurs possibles d'une case est infèrieur au minimum,
                        {
                            minimum = sudoku[i, j].possibleValues.Count; // le minimum est redéfini
                            rep = new List<int[]>(); // La liste est vidée
                            rep.Add(new int[] { i, j }); // et on ajoute la position de la case étudié à notre nouvelle liste
                        }
                    }
                }
            }
            return rep;
        }

        // La fonction DegreeHeuristic rend la liste des position des cases influençant le plus grand nombre de variables encore non définies 
        // parmis une liste entrée ("variables")
        public static List<int[]> DegreeHeuristic(cell[,] sudoku, List<int[]> variables)
        {
            List<int[]> rep = new List<int[]>();
            int maxConstraints = 0; // "maxConstraints" est le nombre de variables influencé maximum trouvé pour le moment

            foreach (int[] pos in variables) // pour chaque variables de la liste fournie
            {
                int posConstraints = countConstraintsOnOtherVariables(sudoku, pos);
                if (posConstraints == maxConstraints) // si elle influence un nombre de variables égale au  maximum actuel 
                {
                    rep.Add(pos); // on l'ajoute à la liste réponse
                }
                else if (posConstraints > maxConstraints) // si elle influence un nombre de variables supèrieur au  maximum actuel 
                {
                    maxConstraints = posConstraints; // le maximum est redéfini
                    rep = new List<int[]>(); // La liste de réponse est vidée
                    rep.Add(pos); // on l'ajoute à la liste réponse
                }
            }
            return rep;
        }

        // la fonction countConstraintsOnOtherVariables prend en paramètre la position d'une case de notre sudoku est rend le nombre
        // de variables encore non définie qu'elle influence
        public static int countConstraintsOnOtherVariables(cell[,] sudoku, int[] pos)
        {
            int count = 0; // count est un conteur que l'on incrémentera progressovement puis que l'on rendra
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < sudoku.GetLength(0); i++)
            {
                if (sudoku[i, pos[1]].value == 0 && i != pos[0]) // exploration de la colonne
                {
                    count++;
                }
                if (sudoku[pos[0], i].value == 0 && i != pos[1]) // exploration de la ligne
                {
                    count++;
                }

                int x = xbloc + i / 3;
                int y = ybloc + i % 3;
                if (sudoku[x, y].value == 0 && x != pos[0] && y != pos[1]) // exploration du bloc
                {
                    count++;
                }
            }
            return count;
        }

        // La fonction LeastConstrainingValue prend en paramètre la position d'une case à étudier et une liste de ses valeurs
        // possibles et rend celles réduisant le moins les valeurs possibles des cases "voisines" de la case étudiée.
        public static List<int> LeastConstrainingValue(cell[,] sudoku, int[] pos, List<int> possibleValues)
        {
            List<int> rep = new List<int>();
            int minimumCount = sudoku.GetLength(0) * 3; // "minimumCount" est le nombre de cases qui verraient leur nombre de valeurs possibles minimum trouvé pour le moment
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

        // La fonction countConstraintsOnOtherValues prend en paramètre la position d'une case et une valeur à étudier
        // et rend le nombre de variables "voisines" à celles étudiée qui verraient leur nombre de valeurs possibles diminuer si la valeur étudié était choisie.
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

        // la fonction testSolution prend en paramètre un sudoku et rend un booléen indiquant si le sudoku entré est complété et valide
        public static bool testSolution(cell[,] solution)
        {
            if (!testSolutionLines(solution) || !testSolutionColumns(solution) || !testSolutionBlocs(solution))
            {
                return false;
            }
            return true;
        }

        // la fonction testSolutionLines prend en paramètre un sudoku et rend un booléen indiquant si aucune ligne du sudoku n'a de 
        // valeurs en double
        public static bool testSolutionLines(cell[,] solution)
        {
            for (int i = 0; i < solution.GetLength(0); i++)
            {
                for (int j = 0; j < solution.GetLength(1); j++)
                    for (int k = j + 1; k < solution.GetLength(1); k++)
                    {
                        if (solution[i, j].value == solution[i, k].value || solution[i, j].value == 0)
                        {
                            return false;
                        }
                    }
            }
            return true;
        }

        // la fonction testSolutionColumns prend en paramètre un sudoku et rend un booléen indiquant si aucune colonne du sudoku n'a de 
        // valeurs en double
        public static bool testSolutionColumns(cell[,] solution)
        {
            for (int i = 0; i < solution.GetLength(1); i++)
            {
                for (int j = 0; j < solution.GetLength(0); j++)
                {
                    for (int k = j + 1; k < solution.GetLength(0); k++)
                    {
                        if (solution[j, i].value == solution[k, i].value || solution[i, j].value == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        // la fonction testSolutionBlocs prend en paramètre un sudoku et rend un booléen indiquant si aucun bloce du sudoku n'a de 
        // valeurs en double
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

        // la fonction gridToCells prend en paramètre un tableau d'entiers (correspondant à un sudoku) et rend un tableau de cellules
        // correspondant au tableau de sudoku entré et dont les valeurs possibles des cases vides ont été renseignés
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

        // la fonction getPossibleValues prend en paramètre un tableau d'entiers (correspondant à un sudoku) et la position d'une case à étudier 
        // et rend, si celle si est vide, la liste de valeurs possibles pour cette case
        public static List<int> getPossibleValues(int[,] grid, int[] pos)
        {
            List<int> rep = new List<int>();
            if (grid[pos[0], pos[1]] != 0)
            {
                return rep;
            }
            for (int i = 1; i <= grid.GetLength(0); i++) // la liste réponse est remplies avec toutes les valeurs possibles
            {
                rep.Add(i);
            }
            int xbloc = pos[0] / 3 * 3;  // position x du point supèrieur gauche du bloc auquel la valeur étudié appartient
            int ybloc = pos[1] / 3 * 3;  // position y du  point supèrieur gauche du bloc auquel la valeur étudié appartient
            for (int i = 0; i < grid.GetLength(0); i++) // pour chaque "voisine" de la case étudié n'étant pas vide, si sa valeur 
            {                                           // n'a pas encore été retiré de la liste réponse, on la retire de cette dernière
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
