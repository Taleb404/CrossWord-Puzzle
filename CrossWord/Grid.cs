using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWord
{
    [Serializable]
    public class Grid
    {
        public int numAttempts;
        public char[,] cells = new char[10, 10];
        public List<String> solution = new List<String>();
        public List<int> dir = new List<int>();
    }
}
