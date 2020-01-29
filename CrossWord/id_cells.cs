using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWord
{
   public class id_cells
    {
        public int X;
        public int Y;
        public String direction;
        public String word;
        public String number;
        public String clue;

        public id_cells(int x, int y, String d, String n, String w, String c)
        {
            this.X = x;
            this.Y = y;
            this.number = n;
            this.direction = d;
            this.word = w;
            this.clue = c;

        }
    }
}
