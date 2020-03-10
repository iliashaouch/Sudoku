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
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            

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

            //int[,] gridSolved = {
            //    { 3, 2, 1, 7, 5, 4, 6, 9, 8 },
            //    { 6, 4, 8, 2, 9, 3, 1, 5, 7 },
            //    { 5, 7, 9, 8, 1, 6, 2, 3, 4 },
            //    { 7, 8, 2, 3, 4, 5, 9, 6, 1 },
            //    { 9, 6, 5, 1, 8, 7, 4, 2, 3 },
            //    { 1, 3, 4, 9, 6, 2, 8, 7, 5 },
            //    { 8, 9, 3, 5, 2, 1, 7, 4, 6 },
            //    { 2, 5, 6, 4, 7, 8, 3, 1, 9 },
            //    { 4, 1, 7, 6, 3, 9, 5, 8, 2 }
            //};

            Console.WriteLine("Voulez vous entrer votre propre sudoku (0) ou utiliser celui par defaut (1) ? :");

            if (Console.ReadLine() == "0")
            {
                String t = null;
                String a = null;

                int[,] grilleSudoku = new int[9, 9];

                Console.WriteLine("Entre votre sudoku (Appuyez sur entrée pour laissez une case vide) :");

                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        a = Console.ReadLine();
                        if (a == "") { a = "0"; }
                        while (System.Convert.ToInt32(a) > 9 || a.Length > 1)
                        {
                            Console.WriteLine("Valeur interdite.Recommencez :");
                            a = Console.ReadLine();
                            if (a == "") { a = "0"; }
                        }
                        t = t + a + " |";
                        Console.WriteLine(t);
                        grilleSudoku[i, j] = System.Convert.ToInt32(a);
                    }
                    t = t + "\n";
                }

                cell[,] ss = Sudoku.gridToCells(grilleSudoku);

                var repBackTracking = Sudoku.BackTrackingSearch(ss);

                if (repBackTracking.Item2) Console.WriteLine("\nGrille correctement completee !");
                else Console.WriteLine("\nGrille entree invalide !");

                Console.WriteLine("\nGrille de départ :");
                Sudoku.printThisSudoku(Sudoku.gridToCells(grilleSudoku));
                Console.WriteLine("\nGrille finale :");
                Sudoku.printThisSudoku(ss);
            }
            else
            {
                cell[,] s = Sudoku.gridToCells(grid);
                var repBackTracking = Sudoku.BackTrackingSearch(s);
                //Console.WriteLine(repBackTracking);

                if (repBackTracking.Item2) Console.WriteLine("\nGrille correctement completee !");
                else Console.WriteLine("\nGrille entree invalide !");

                Console.WriteLine("\nGrille de départ :");
                Sudoku.printThisSudoku(Sudoku.gridToCells(grid));
                Console.WriteLine("\nGrille finale :");
                Sudoku.printThisSudoku(s);
            }
            


            Console.ReadLine();
        }
    }
}
