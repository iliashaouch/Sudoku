using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TP2_Sudoku;


namespace TP2_Sudoku
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
            int[,] grid = {
                { 3, 2, 1, 7, 0, 4, 0, 0, 0 },
                { 6, 4, 0, 0, 9, 0, 0, 0, 7 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 2, 0, 4, 5, 9, 0, 0 },
                { 0, 0, 5, 1, 8, 7, 4, 2, 0 },
                { 0, 0, 4, 9, 6, 2, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 4, 7, 0, 0, 1, 9 },
                { 4, 0, 0, 6, 0, 9, 5, 8, 2 }
            };

            int[,] gridSolved = {
                { 3, 2, 1, 7, 5, 4, 6, 9, 8 },
                { 6, 4, 8, 2, 9, 3, 1, 5, 7 },
                { 5, 7, 9, 8, 1, 6, 2, 3, 4 },
                { 7, 8, 2, 3, 4, 5, 9, 6, 1 },
                { 9, 6, 5, 1, 8, 7, 4, 2, 3 },
                { 1, 3, 4, 9, 6, 2, 8, 7, 5 },
                { 8, 9, 3, 5, 2, 1, 7, 4, 6 },
                { 2, 5, 6, 4, 7, 8, 3, 1, 9 },
                { 4, 1, 7, 6, 3, 9, 5, 8, 2 }
            };
            cell[,] sSolved = Sudoku.gridToCells(gridSolved);
            //Console.WriteLine(Sudoku.testSolution(sSolved));

            int[,] gridtest =
            {
                { 3, 0, 1, 0, 0, 4, 6, 9, 0 },
                { 6, 0, 8, 0, 9, 3, 1, 5, 7 },
                { 0, 7, 9, 0, 1, 6, 2, 0, 4 },
                { 0, 8, 2, 3, 4, 0, 0, 6, 1 },
                { 9, 0, 5, 1, 0, 7, 4, 2, 3 },
                { 1, 3, 4, 0, 6, 0, 0, 7, 5 },
                { 8, 9, 0, 5, 2, 0, 0, 0, 6 },
                { 2, 5, 6, 0, 7, 8, 3, 0, 9 },
                { 0, 1, 0, 6, 3, 0, 5, 8, 2 }
            };
            cell[,] s = Sudoku.gridToCells(grid);
            cell[,] test = Sudoku.gridToCells(gridtest);
            //var repMRV = Sudoku.MRV(s);

            //var repDegree = Sudoku.DegreeHeuristic(s, repMRV);

            //var repLeast = Sudoku.LeastConstrainingValue(s, repDegree[0]);

            var repBackTracking = Sudoku.BackTrackingSearch(s);
            Console.WriteLine("Yeaaaaaah !");
            Console.WriteLine(repBackTracking);
            Console.ReadLine();
        }
    }
}
