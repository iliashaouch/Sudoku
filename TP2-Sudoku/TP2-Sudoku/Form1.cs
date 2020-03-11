﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TP2_Sudoku
{
    public partial class Form1 : Form
    {
        private Panel buttonPan = new Panel();
        private Button resolveButton = new Button();
        private Button loadButton = new Button();
        private Label status = new Label();

        private TextBox[,] ori = new TextBox[9, 9];
        private TextBox[,] solv = new TextBox[9, 9];

        private cell[,] sud;
        private List<int[,]> listgrille = new List<int[,]>();


        public Form1()
        {
            InitializeComponent();
            int[,] grid0 = {
                {3, 0, 6, 5, 0, 8, 4, 0, 0},
                {5, 2, 0, 0, 0, 0, 0, 0, 0},
                {0, 8, 7, 0, 0, 0, 0, 3, 1},
                {0, 0, 3, 0, 1, 0, 0, 8, 0},
                {9, 0, 0, 8, 6, 3, 0, 0, 5},
                {0, 5, 0, 0, 9, 0, 6, 0, 0},
                {1, 3, 0, 0, 0, 0, 2, 5, 0},
                {0, 0, 0, 0, 0, 0, 0, 7, 4},
                {0, 0, 5, 2, 0, 6, 3, 0, 0}
            };
            int[,] grid1 = {
                { 4, 6, 0, 2, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 6, 4, 0, 0, 0, 7 },
                { 0, 0, 3, 0, 0, 8, 0, 4, 9 },
                { 0, 9, 0, 8, 6, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 8, 0, 9, 7, 0, 6, 0 },
                { 6, 5, 0, 7, 0, 0, 4, 0, 0 },
                { 7, 0, 0, 0, 1, 4, 0, 5, 0 },
                { 0, 0, 0, 0, 0, 6, 0, 1, 3 }
            };
            int[,] grid2 = {
                { 1, 0, 0, 0, 0, 7, 0, 9, 0 },
                { 0, 3, 0, 0, 2, 0, 0, 0, 8 },
                { 0, 0, 9, 6, 0, 0, 5, 0, 0 },
                { 0, 0, 5, 3, 0, 0, 9, 0, 0 },
                { 0, 1, 0, 0, 8, 0, 0, 0, 2 },
                { 6, 0, 0, 0, 0, 4, 0, 0, 0 },
                { 3, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 4, 0, 0, 0, 0, 0, 0, 7 },
                { 0, 0, 7, 0, 0, 0, 3, 0, 0 }
            };
            listgrille.Add(grid0);
            listgrille.Add(grid1);
            listgrille.Add(grid2);
            this.Load += new EventHandler(FormLoad);
        }



        #region setup
        private void FormLoad(Object sender, EventArgs e)
        {
            SetupLayout();
            Setup_Grid_ori();
            Setup_Grid_sol();
        }

        private void SetupLayout()
        {
            this.Size = new Size(1050, 590);
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            resolveButton.Text = "Resolve";
            resolveButton.Location = new Point(10, 10);
            resolveButton.Click += new EventHandler(resolveButton_Click);

            loadButton.Text = "Load";
            loadButton.Location = new Point(100, 10);
            loadButton.Click += new EventHandler(loadButton_Click);

            status.Text = "";
            status.Font = new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold);
            status.Location = new Point(700, 0);
            status.AutoSize = true;
            status.Visible = true;

            buttonPan.Controls.Add(resolveButton);
            buttonPan.Controls.Add(loadButton);
            buttonPan.Controls.Add(status);
            buttonPan.Height = 50;
            buttonPan.Dock = DockStyle.Bottom;

            this.Controls.Add(this.buttonPan);
        }


        private void Setup_Grid_ori()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ori[i, j] = new TextBox();
                    ori[i, j].Name = i + "" + j;
                    ori[i, j].Text = "";
                    ori[i, j].Multiline = true;
                    ori[i, j].Size = new Size(50, 50);
                    ori[i, j].Location = new Point(10 + ((j) * 55), 10 + ((i) * 55));
                    ori[i, j].ReadOnly = false;
                    ori[i, j].TextAlign = HorizontalAlignment.Center;
                    ori[i, j].TabStop = false;
                    ori[i, j].Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    ori[i, j].Cursor = Cursors.Default;
                    if ((i + j) % 2 == 0)
                    {
                        ori[i, j].BackColor = Color.LightBlue;
                    }
                    this.Controls.Add(ori[i, j]);
                }
            }
        }


        private void Setup_Grid_sol()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    solv[i, j] = new TextBox();
                    solv[i, j].Name = i + "" + j;
                    solv[i, j].Text = "";
                    solv[i, j].Multiline = true;
                    solv[i, j].Size = new Size(50, 50);
                    solv[i, j].Location = new Point(525 + ((j) * 55), 10 + ((i) * 55));
                    solv[i, j].ReadOnly = true;
                    solv[i, j].Enabled = false;
                    solv[i, j].TextAlign = HorizontalAlignment.Center;
                    solv[i, j].TabStop = false;
                    solv[i, j].Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
                    solv[i, j].Cursor = Cursors.Default;
                    if ((i + j) % 2 == 0)
                    {
                        solv[i, j].BackColor = Color.LightBlue;
                    }
                    this.Controls.Add(solv[i, j]);
                }
            }
        }


        private void remplitGrid_ori()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    ori[i, j].Text = sud[i, j].value.ToString();
                }
            }
        }

        private void remplitGrid_solv()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    solv[i, j].Text = sud[i, j].value.ToString();
                }
            }
        }
        #endregion

        #region button
        private void loadButton_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            sud = Sudoku.gridToCells(listgrille[r.Next(0, listgrille.Count)]);
            remplitGrid_ori();
        }

        private void resolveButton_Click(object sender, EventArgs e)
        {
            status.Text = "CALCULATING...";
            int[,] temp = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (int.TryParse(ori[i, j].Text, out temp[i, j])) ;
                    else temp[i, j] = 0;
                }
            }
            sud = Sudoku.gridToCells(temp);
            var rep = Sudoku.BackTrackingSearch(sud);
            if (rep.Item2) status.Text = "DONE";
            else status.Text = "NO SOLUTION FOUND";
            remplitGrid_solv();
        }
        #endregion
    }
}
