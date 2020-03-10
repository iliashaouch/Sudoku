using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP2_Sudoku
{
    public partial class Form1 : Form
    {
        private Panel buttonPan = new Panel();
        private Button resolveButton = new Button();
        private Button loadButton = new Button();

        private TextBox[,] ori = new TextBox[9, 9];
        private TextBox[,] solv = new TextBox[9, 9];

        cell[,] sud;

        public Form1()
        {
            InitializeComponent();
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
            this.sud = Sudoku.gridToCells(grid);
            this.Load += new EventHandler(FormLoad);
        }

        private void sudokuBindingSource_CurrentChanged(object sender, EventArgs e)
        {

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

            buttonPan.Controls.Add(resolveButton);
            buttonPan.Controls.Add(loadButton);
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
                    //ori[i, j].MouseDown += new MouseEventHandler(handleInput);
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
                    //textBoxes[i, j].ContextMenuStrip = mnuInput;
                    if ((i + j) % 2 == 0)
                    {
                        solv[i, j].BackColor = Color.LightBlue;
                    }
                    this.Controls.Add(solv[i, j]);
                    //textBoxes[i, j].MouseDown += new MouseEventHandler(HandleInput);
                }
            }
            //grid = new SudokuGrid();
            //UpdateDisplay();
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
            Sudoku.BackTrackingSearch(sud);
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

        }

        private void resolveButton_Click(object sender, EventArgs e)
        {
            int[,] temp = new int[9, 9];
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (int.TryParse(ori[i, j].Text, out temp[i, j])) ;
                    else temp[i, j] = 0;
                }
            }
            //sud = Sudoku.gridToCells(temp);
            Sudoku.BackTrackingSearch(sud);
            remplitGrid_solv();
        }
        #endregion

        /* private void mnuInput_Opening(object sender, CancelEventArgs e)
         {
             ContextMenu menu;
             mnuInput.Items.Clear();
             if (grid[tempR, tempC].Value == 0)
             {
                 for (int i = 1; i <= 9; i++)
                 {
                     if (grid[tempR, tempC][i] == 0)
                     {
                         mnuInput.Items.Add("Make " + i);
                     }
                 }
             }
             else
             {
                 mnuInput.Items.Add("Remove Number");
             }
         }*/
    }
}
