using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace g1830176Ⅱday9
{
    public class MapData
    {
        public string[] name { get; set; }
        public int[] id { get; set; }
        public Rectangle[] loc { get; set; }
        public MapData()
        {
            ReadCSV("image_loc_data.csv");
        }
        private void ReadCSV(string file)
        {
            string[] row_data = System.IO.File.ReadAllLines
                (file, Encoding.GetEncoding("shift_jis"));
            string[] temp = row_data[0].Split(',');
            int col_num = temp.Length;
            string[,] csv_data = new string[row_data.Length, col_num];

            for(int i=0; i < row_data.Length; i++)
            {
                temp = row_data[i].Split(',');
                for(int j = 0; j < temp.Length; j++)
                {
                    csv_data[i, j] = temp[j];
                }
            }
            name = new string[row_data.Length];
            id = new int[row_data.Length];
            loc = new Rectangle[row_data.Length];

            for (int i = 0; i < csv_data.GetLength(0); i++)
            {
                name[i] = csv_data[i, 0];
                id[i] = int.Parse(csv_data[i, 1]);
                int x1 = int.Parse(csv_data[i, 2]);
                int y1 = int.Parse(csv_data[i, 3]);
                int x2 = int.Parse(csv_data[i, 4]);
                int y2 = int.Parse(csv_data[i, 5]);
                int width = Math.Abs(x1 - x2);
                int height = Math.Abs(y1 - y2);
                loc[i] = new Rectangle(x1, y1, width, height);
            }
        }
        public int GetNum()
        {
            return name.Length;
        }
    }
}
