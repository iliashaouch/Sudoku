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
        private DataGridView gridView = new DataGridView();
        private Button resolveButton = new Button();
        private Button loadButton = new Button();
        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(FormLoad);
        }

        private void sudokuBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        #region setup
        private void FormLoad(Object sender, EventArgs e)
        {
            SetupLayout();
            Setup_Grid();
            remplitGrid();
        }

        private void SetupLayout()
        {
            this.Size = new Size(1000, 500);

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

        private void Setup_Grid()
        {
            this.Controls.Add(gridView);
            gridView.ColumnCount = 9;
            gridView.RowCount = 9;
            gridView.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            gridView.ColumnHeadersVisible = false;
            gridView.RowHeadersVisible = false;

        }

        private void remplitGrid()
        {
            for (int i = 0; i < gridView.RowCount; i++)
            {
                for (int j = 0; j < gridView.ColumnCount; j++)
                {
                    int c = i + 1, r = j + 1;
                    gridView[i, j].Value = c + "" + r;
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

        }
        #endregion
    }
}
