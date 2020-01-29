using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrossWord
{
   public class GridGenerator
    {

        public static List<id_cells> idc = new List<id_cells>();

        public static int[,] DIRS = new int[,] { { 1, 0 }, { 0, 1 } };

        public static int nRows = 10, nCols = 10;
        public static int gridSize = nRows * nCols;

        public static int minWords = 5;
        public static Random RANDOM = new Random();
       

        public static List<String> readWords(String filename)
        {
            int maxLength = Math.Max(GridGenerator.nRows, nCols);
            List<String> words = new List<String>();
            StreamReader reader = File.OpenText("e:\\2.txt");
            string line = reader.ReadLine();

            try
            {
                while (line != null)
                {
                    String s = line;
                    s = s.Trim();
                    s = s.ToLower();
                    Regex word = new Regex("^[a-z]{3," + maxLength + "}$");
                    Match m = word.Match(s);
                    if (m.Success)
                    {
                        s = s.ToUpper();
                        words.Add(s);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
            return words;
        }
        public static void Shuffle<T>(IList<T> list, int seed)
        {
            var rng = new Random(seed);
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Grid createWordSearch(List<String> words)
        {
            Grid grid = null;
            int numAttempts = 0;


            while (++numAttempts < 5000)
            {
                Shuffle(words, 3245);
                // Collections.shuffle(words);
                words.OrderBy(item => RANDOM.Next());


                grid = new Grid();

                int messageLength = gridSize - 13;
                int target = gridSize - messageLength;
                int cellsFilled = 0;
                foreach (String word in words)
                {
                    cellsFilled += tryPlaceWord(grid, word);
                    if (cellsFilled == target)
                    {
                        if (grid.solution.Count >= minWords)
                        {
                            grid.numAttempts = numAttempts;
                            return grid;
                        }
                        else break;
                    }
                }
            }
           
            return grid;
        }


        public static int tryPlaceWord(Grid grid, String word)
        {
            int letterPlaced = 0;

            for (int dir = 0; dir < (DIRS.Length / 2); dir++) //from 0 to 3 
            {
                int randDir = RANDOM.Next(DIRS.Length / 2);
                int randPos = RANDOM.Next(gridSize);

                dir = (dir + randDir) % (DIRS.Length / 2);
                //Console.WriteLine("die"+dir);
                for (int pos = 0; pos < gridSize; pos++)
                {
                    pos = (pos + randPos) % gridSize;

                    if (dir == 0 || dir == 1)
                        letterPlaced = tryLocation(grid, word, dir, pos);
                    if (letterPlaced > 0)
                    {


                        return letterPlaced;
                    }
                }
            }
            return 0;
        }

        public static int tryLocation(Grid grid, String word, int dir, int pos)
        {

            int r = pos / nCols;
            int c = pos % nCols;
            int Length = word.Length;

            if ((DIRS[dir, 0] == 1 && (Length + c) > nCols)
                    || (DIRS[dir, 0] == -1 && (Length - 1) > c)
                    || (DIRS[dir, 1] == 1 && (Length + r) > nRows)
                    || (DIRS[dir, 1] == -1 && (Length - 1) > r))
            {

                return 0; // out of range
            }
            int i, rr, cc, overlaps = 0;

            for (i = 0, rr = r, cc = c; i < Length; i++)
            {
                if (grid.cells[rr, cc] != 0 && grid.cells[rr, cc] != word[i])
                {
                    return 0;
                }
                cc += DIRS[dir, 0];
                rr += DIRS[dir, 1];
                //  System.out.println(rr + " " + cc);
            }

            for (i = 0, rr = r, cc = c; i < Length; i++)
            {
                if (grid.cells[rr, cc] == word[i])
                {
                    overlaps++;
                }
                else
                {
                    grid.cells[rr, cc] = word[i];
                }
                if (i < Length - 1)
                {
                    cc += DIRS[dir, 0];
                    rr += DIRS[dir, 1];


                    // Console.WriteLine("w" + word);
                    // Console.WriteLine("die c" + DIRS[dir, 0]);
                }
            }



            int lettersPlaced = Length - overlaps;

            grid.solution.Add(String.Format("{0} ({1},{2})({3},{4})", word, c, r, cc, rr));
            grid.dir.Add(dir);


            return lettersPlaced;
        }



    }
}
