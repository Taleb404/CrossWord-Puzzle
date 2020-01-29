using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrossWord;
using System.IO;

namespace CrossWordBuilder
{

    public partial class Form1 : Form 
    {
        int i ,j , ii , jj ;
        Grid tmp;
        List<string> fi, fj , clues , words,direct;
        public Form1()
        {
            InitializeComponent();
            initiliazeBoard();
            tmp = new Grid();
            fi = new List<string>();
            fj = new List<string>();
            clues = new List<string>();
            words = new List<string>();
            direct = new List<string>();

        } 

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void initiliazeBoard()
        {
            Board.BackgroundColor = Color.Black;
            Board.DefaultCellStyle.BackColor = Color.Black;

            for (int i = 0; i < 21; i++)
                Board.Rows.Add();

            foreach (DataGridViewColumn c in Board.Columns)
                c.Width = Board.Width / Board.Columns.Count;

            foreach (DataGridViewRow c in Board.Rows)
                c.Height = Board.Height / Board.Columns.Count;

            for (int row = 0; row < Board.Rows.Count; row++)
            {
                for (int col = 0; col < Board.Columns.Count; col++)
                {
                    DataGridViewCell c = Board[col, row];
                    c.Style.BackColor = Color.White;
                    c.Style.SelectionBackColor = Color.Cyan;


                }
            }
        
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
                if (Board[e.ColumnIndex, e.RowIndex].Value.ToString().Length > 1)
                {
                    Board[e.ColumnIndex, e.RowIndex].Value = Board[e.ColumnIndex, e.RowIndex].Value.ToString().Substring(0, 1);

                }
            }
            catch { }

        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label2.Text = "End Index :" + Board.CurrentCell.RowIndex + "," + Board.CurrentCell.ColumnIndex;
            ii = Board.CurrentCell.RowIndex;
            jj = Board.CurrentCell.ColumnIndex;
            string tmp = "";
            for (int row = i; row <= ii; row++)
            {
                for (int col = j; col <= jj; col++)
                {
                        tmp += Board[col, row].Value.ToString();
                }
            }
            label3.Text =  tmp;

        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            int num = clue_table.RowCount;
            num++;
            string di = (radioButton1.Checked) ? "Across" : "Down";
            string word = label3.Text;
            string clue = textBox1.Text;
            clue_table.Rows.Add(new String[] { num.ToString(), di, word,clue });
            tmp.solution.Add(String.Format("{0} ({1},{2})({3},{4})", word, i, j, ii, jj));
            for (int row = i; row <= ii; row++)
            {
                for (int col = j; col <= jj; col++)
                {
                    char c = Board[col, row].Value.ToString()[0];
                    tmp.cells[col,row] =c;
                }
            }
            tmp.dir.Add((radioButton1.Checked) ? 1 : 0);
            fi.Add(i.ToString());
            fj.Add(j.ToString());
            direct.Add((radioButton1.Checked) ? "across" : "down");
            words.Add(word);
            clues.Add(clue);
            
        }

        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = "words.txt";
            savefile.Filter = "Puzzle files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {

                string final = "x|y|direction|number|word|clue" + Environment.NewLine;
                //0|4|across|1|lock|need a keys
                for(int i = 0; i <words.Count; i++)
                {

                    final += fi[i] +"|"+fj[i]+"|"+direct[i]+"|"+ (i+1).ToString() +"|" +
                        words[i]+"|"+clues[i] + Environment.NewLine;
                }
                File.WriteAllText(savefile.FileName, final);
                
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void clue_table_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text ="Start Index :" + Board.CurrentCell.RowIndex + ","+ Board.CurrentCell.ColumnIndex;
            i = Board.CurrentCell.RowIndex;
            j = Board.CurrentCell.ColumnIndex;
        }
    }
}
