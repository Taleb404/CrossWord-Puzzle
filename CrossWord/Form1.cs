using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossWord
{
    public partial class Form1 : Form
    {
        Grid grid = new Grid();
        public Form1()
        {
          
          InitializeComponent();
        
        }
        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
        public void buildWordList()
        {
            
            //  grid = GridGenerator.createWordSearch(GridGenerator.readWords("e:\\2.txt"));
           
            string w1;
            int size = grid.solution.Count;
            for (int i = 0; i < size; i += 1)
            {
                int len = grid.solution[i].Length;
                w1 = Regex.Replace(grid.solution[i], @"[^A-Z]+", String.Empty);
                int r = grid.solution[i].ElementAt(len - 7) - '0';
                int c = grid.solution[i].ElementAt(len - 9) - '0';

                string num = i.ToString();
                string di = "", d = "";
                if (grid.dir[i] == 1) 
                {
                    di = "ACROSS";
                    d = "A";
                    GridGenerator.idc.Add(new id_cells(r, c, di, num, w1, "c"));
                    Console.WriteLine(w1 + " " + num);
                   clue_table.Rows.Add(new String[] { num, di, w1 });

                }
                else if (grid.dir[i] == 0)       
                {
                    di = "DOWN";
                    d = "D";

                    GridGenerator.idc.Add(new id_cells(r, c, di, num, w1, "c"));
                    Console.WriteLine(w1 + " " + num);
                    clue_table.Rows.Add(new String[] { num, di, w1 });

                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About", " CrossWord Puzzles ");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            
        }
        private void initiliazeBoard()
        {
            Board.BackgroundColor = Color.Black;
            Board.DefaultCellStyle.BackColor = Color.Black;

            for(int i = 0; i < 21; i++)
                Board.Rows.Add();
                 
            foreach(DataGridViewColumn c in Board.Columns)
                    c.Width = Board.Width / Board.Columns.Count;

            foreach (DataGridViewRow c in Board.Rows)
                    c.Height = Board.Height / Board.Columns.Count;

            for(int row = 0; row < Board.Rows.Count; row++)
            {
                for(int col=0; col<Board.Columns.Count; col++)
                {
                    Board[col, row].ReadOnly = true;
                }
            }
           
            foreach(id_cells i in GridGenerator.idc)
            {
                int start_col = i.X;
                int start_row = i.Y;
                char[] word = i.word.ToCharArray();
                for (int j=0;j<word.Length;j++)
                {
                    if (i.direction.ToUpper() == "ACROSS")
                        formatCell(start_row, start_col + j, word[j].ToString());

                    if (i.direction.ToUpper() == "DOWN")
                        formatCell(start_row + j, start_col , word[j].ToString());

                }
            }

        }

        private void formatCell(int row , int col , string Letter)
        {
            DataGridViewCell c = Board[col, row];
            c.Style.BackColor = Color.White;
            c.ReadOnly = false;
            c.Style.SelectionBackColor = Color.Cyan;
            c.Tag = Letter;
            
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            
        }

        private void Board_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Board[e.ColumnIndex, e.RowIndex].Value = Board[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
            }
            catch { }

            try
            {
              if(Board[e.ColumnIndex, e.RowIndex].Value.ToString().Length > 1)
                {
                    Board[e.ColumnIndex, e.RowIndex].Value = Board[e.ColumnIndex, e.RowIndex].Value.ToString().Substring(0, 1);

                }
            }
            catch { }

            try
            {
                if (Board[e.ColumnIndex, e.RowIndex].Value.ToString().Equals(Board[e.ColumnIndex, e.RowIndex].Tag.ToString().ToUpper()))
                    Board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.DarkGreen;
                else
                    Board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;

            }
            catch { }
        }

        private void Board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            String number = "";
       
            
                if (GridGenerator.idc.Any(c => (number = c.number) != "" && c.X == e.ColumnIndex && c.Y == e.RowIndex)) 
                {
                    Rectangle r = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
                    e.Graphics.FillRectangle(Brushes.White, r);
                    Font f = new Font(e.CellStyle.Font.FontFamily, 7);
                    e.Graphics.DrawString(number, f, Brushes.Black, r);
                    e.PaintContent(e.ClipBounds);
                    e.Handled = true;
               }
            
        }

        private void Board_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Puzzle File";
            theDialog.Filter = "Puzzle files (*.pzl)|*.pzl|All files (*.*)|*.*";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    grid = ReadFromBinaryFile<Grid>(theDialog.FileName);
                    buildWordList();
                    initiliazeBoard();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }

   
    
}
