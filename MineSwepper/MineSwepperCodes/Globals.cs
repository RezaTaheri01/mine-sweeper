using System;
using System.Drawing;

namespace MineSwepperGame.MineSwepperCodes
{
    class Globals
    {
        // Todo: Save
        // Variables for save: hoverOn, assistOn, level, time | list of cell values(is_mine, is_clicked, is_flag).
        // Each cell note: in process of creating a board we use this 3 values instead of probability.

        // Disable hover by set it to false
        public bool hoverOn = false;
        public string hoverOn_text = "Turn off hover";
        public string hoverOff_text = "Turn on hover";

        // Assist
        public bool assistOn = false;
        public string assistOn_text = "Turn off assist";
        public string assistOff_text = "Turn on assist";


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
        public Brush flagColor = Brushes.PaleVioletRed;
        public Brush hoverColor = Brushes.LightSeaGreen;


        public Pen pen = new Pen(Brushes.DarkOliveGreen, 3);
        public Pen pen2 = new Pen(Brushes.DarkGreen, 2);

        // mine counter font
        public Font font;

        private int start_up_level = 0;

        public int curr_cube_size = 45;
        public int curr_board_width= 10;
        public int curr_board_height= 10;
        public int curr_mine_precent=10;
        public int curr_mine_time = 65;

        // Cube Size(cell), Board Width, Board Height, Mines Precents, minesAnimationTimer
        // Board Width => cell in row
        // Board Height => cell in column
        int[] easy = new int[5] { 44, 10, 10, 10, 75 };
        int[] medium = new int[5] { 32, 16, 16, 18, 40 };
        int[] hard = new int[5] { 30, 20, 20, 20, 20 };
        int[] ultra_hard = new int[5] { 28, 30, 20, 22, 7 };
        // width and height should be even that labels place in the middle

        public int[][] sizes;

        public Globals()
        {
            workingDirectory = Environment.CurrentDirectory;
            workingDirectory += "\\cache";
            Path = workingDirectory + "\\data.txt";

            sizes = new int[][] { easy, medium, hard, ultra_hard };

            curr_cube_size = sizes[start_up_level][0];
            curr_board_width = sizes[start_up_level][1];
            curr_board_height = sizes[start_up_level][2];
            curr_mine_precent = sizes[start_up_level][3];
            curr_mine_time = sizes[start_up_level][4];
            change_font_size();
        }

        public void change_font_size()
        {
            font = new Font("Serif", curr_cube_size * 9 / 20, FontStyle.Bold);
        }
    }
}
