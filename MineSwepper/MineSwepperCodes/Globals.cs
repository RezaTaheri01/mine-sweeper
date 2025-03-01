using System;
using System.Drawing;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace MineSwepperGame.MineSwepperCodes
{
    class Globals
    {
        public string workingDirectory;
        public string Path;

        // To edit mines' colors, Go to Board.cs line 202
        // Colors
        public Brush[] mineCountColors = new Brush[9] { Brushes.Blue, Brushes.Green, Brushes.Red, Brushes.Purple, Brushes.DarkRed, Brushes.Black, Brushes.DarkGray, Brushes.DarkCyan, Brushes.DarkCyan };

        public Color bgColor = Color.DarkGreen;
        public Color foreColor = Color.WhiteSmoke;

        public Brush brushGrass1 = Brushes.LightGreen;
        public Brush brushGrass2 = Brushes.DarkGreen;
        public Brush brushSoil1 = Brushes.SandyBrown;
        public Brush brushSoil2 = Brushes.NavajoWhite;
        public Brush flagColor = Brushes.SkyBlue;

        public Pen pen = new Pen(Brushes.DarkOliveGreen, 3);
        public Pen pen2 = new Pen(Brushes.DarkGreen, 2);

        public int curr_cube_size = 45;
        public int curr_board_width= 10;
        public int curr_board_height= 10;
        public int curr_mine_precent=10;
        public int curr_mine_time = 65;

        // Cube Size(cell), Board Width, Board Height, Mines Precents, minesAnimationTimer
        // Board Width => cell in row
        // Board Height => cell in column
        int[] easy = new int[5] { 35, 10, 10, 10, 65 };
        int[] medium = new int[5] { 30, 16, 16, 18, 40 };
        int[] hard = new int[5] { 27, 20, 20, 20, 20 };
        int[] ultra_hard = new int[5] { 25, 30, 20, 22, 7 };
        // width and height should be even that labels place in the middle

        public int[][] sizes;

        public Globals()
        {
            workingDirectory = Environment.CurrentDirectory;
            workingDirectory += "\\cache";
            Path = workingDirectory + "\\data.txt";

            sizes = new int[][] { easy, medium, hard, ultra_hard };

            curr_cube_size = sizes[0][0];
            curr_board_width = sizes[0][1];
            curr_board_height = sizes[0][2];
            curr_mine_precent = sizes[0][3];
            curr_mine_time = sizes[0][4];
        }
    }
}
