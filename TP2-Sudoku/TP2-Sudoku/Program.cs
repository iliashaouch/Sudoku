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
                { 0, 0, 0, 0, 4, 5, 9, 0, 0 },
                { 0, 0, 5, 1, 8, 7, 4, 0, 0 },
                { 0, 0, 4, 9, 6, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 2, 0, 0, 0, 7, 0, 0, 1, 9 },
                { 0, 0, 0, 6, 0, 9, 5, 8, 2 }
            };

            cell[,] s = Sudoku.gridToCells(grid);

            //var repMRV = Sudoku.MRV(s);

            //var repDegree = Sudoku.DegreeHeuristic(s, repMRV);

            //var repLeast = Sudoku.LeastConstrainingValue(s, repDegree[0]);

            //var repBackTracking = Sudoku.BackTrackingSearch(s);
            //Console.WriteLine("Yeaaaaaah !");

            Console.ReadLine();
        }
    }
}
