using MineSwepperGame.MineSwepperCodes;
using MineSwepper.MineSwepperCodes;
using System.Windows.Forms;
using MineSwepper.Draw;
using System.Drawing;
using System.IO;
using System;

//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.ComponentModel;
//using System.Data;
//using System.Linq;
//using System.Text;


namespace MineSwepper
{
    public partial class MainForm : Form
    {
        //CreateBoard CB = new CreateBoard();
        Board B = new Board();
        Game G = new Game();
        Globals globals = new Globals();

        int X, Y;
        int s = 0;

        public MainForm()
        {
            InitializeComponent();
            B.Form1 = this;
            G.Form = this;
            Folder();

            // Set colors
            menuStrip1.BackColor = globals.bgColor;
            label2.BackColor = globals.bgColor;
            label2.BackColor = globals.bgColor;
            toolStripMenuItem1.BackColor = globals.bgColor;

            menuStrip1.ForeColor = globals.foreColor;
            label2.ForeColor = globals.foreColor;
            label2.ForeColor = globals.foreColor;
            toolStripMenuItem1.ForeColor = globals.foreColor;

            // Set "easy" config for start
            FormSize();
            Size = new Size(X, Y);

            label2.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 32, 0);
            label1.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 64, 31);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        public void MainForm_Paint(object sender, PaintEventArgs e)
        {
            G.Games(globals);
            string[] Lines = File.ReadAllLines(globals.Path);
            if (Lines.Length != 0 && Lines[0] == "2")
            { }
            else
            {
                if (Lines.Length == 0)
                { }
                else if (Lines[0] == "1")
                {
                    pauseToolStripMenuItem.Visible = false;
                    File.WriteAllText(globals.Path, "0");
                    DoubleBuffered = false;
                    Refresh();
                }
                string[] Lines2 = File.ReadAllLines(globals.Path);
                if (Lines2.Length != 0 && Lines2[0] == "0")
                {
                    G.Games(globals);
                    if (Lines.Length == 2)
                    {
                        G.Drawing(e.Graphics, label1, timer, 1, globals);
                    }
                    else
                    {
                        G.Drawing(e.Graphics, label1, timer, 0, globals);
                    }
                    if (B.GameOverF == false)
                    {
                        timer.Start();
                    }
                    else
                    {
                        timer.Stop();
                    }
                    if(Lines.Length == 2)
                    {
                        this.Paint -= new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
                    }
                    File.WriteAllText(globals.Path, "2\n9");
                    
                }
                else if(Lines.Length==2)
                {
                    G.Drawing(e.Graphics, label1, timer, 1, globals);
                }
                else
                {
                    G.Games(globals);
                        G.Drawing(e.Graphics, label1, timer, 1, globals);
                    if (B.GameOverF == false)
                    {
                        timer.Start();
                    }
                    else
                    {
                        timer.Stop();
                    }
                }
            }
        }
        public void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            DoubleBuffered = true;
            G.MouseClickAction(e, label1, timer,label2, globals);
        }
        private void Timer(object sender, EventArgs e)
        {
            
            if (s<10)
            {
                label2.Text = " 000" + s;
            }
            else if(s<100)
            {
                label2.Text = " 00" + s;
            }
            else if(s<1000)
            {
                
                label2.Text ="0"+s;
            }
            else
            {
                label2.Text ="" +s;
            }
            if (label1.Visible == false)
            {
                s++;
            }
            if(s==7202)
            {
                Close();
            }
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        // Ultra Hard
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
            pauseToolStripMenuItem.Text = "Pause";
            pauseToolStripMenuItem.Visible = true;
            File.WriteAllText(globals.Path, "");
            DoubleBuffered = true;
            label1.Visible = false;
            G.CleanCache = true;
            s = 0;
            //label2.Location = new Point(340, 0);
            //label1.Location = new Point(312, 31);
            toolStripMenuItem1.Text = "U-Hard";

            globals.curr_cube_size = globals.sizes[3][0];
            globals.curr_board_width = globals.sizes[3][1];
            globals.curr_board_height = globals.sizes[3][2];
            globals.curr_mine_precent = globals.sizes[3][3];
            globals.curr_mine_time = globals.sizes[3][4];
            globals.change_font_size();

            label2.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 32, 0);
            label1.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 64, 31);

            FormSize();
            Size = new Size(X, Y);
            Invalidate();
            CenterToScreen();
        }
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseToolStripMenuItem.Text = "Pause";
            pauseToolStripMenuItem.Visible = true;
            File.WriteAllText(globals.Path, "");
            DoubleBuffered = true;
            label1.Visible = false;
            G.CleanCache = true;
            s = 0;
            //label1.Location = new Point(162, 31);
            //label2.Location = new Point(190, 0);
            toolStripMenuItem1.Text = "Medium";
            globals.curr_cube_size = globals.sizes[1][0];
            globals.curr_board_width = globals.sizes[1][1];
            globals.curr_board_height = globals.sizes[1][2];
            globals.curr_mine_precent = globals.sizes[1][3];
            globals.curr_mine_time = globals.sizes[1][4];
            globals.change_font_size();

            label2.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 32, 0);
            label1.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 64, 31);

            FormSize();
            Size = new Size(X, Y);
            CenterToScreen();
            Invalidate();

        }
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseToolStripMenuItem.Text = "Pause";
            pauseToolStripMenuItem.Visible = true;
            File.WriteAllText(globals.Path, "");
            DoubleBuffered = true;
            label1.Visible = false;
            G.CleanCache = true;
            s = 0;
            //label2.Location = new Point(236, 0);
            //label1.Location = new Point(209, 31);
            toolStripMenuItem1.Text = "  Hard";
            globals.curr_cube_size = globals.sizes[2][0];
            globals.curr_board_width = globals.sizes[2][1];
            globals.curr_board_height = globals.sizes[2][2];
            globals.curr_mine_precent = globals.sizes[2][3];
            globals.curr_mine_time = globals.sizes[2][4];
            globals.change_font_size();

            label2.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 32, 0);
            label1.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 64, 31);

            FormSize();
            Size = new Size(X, Y);
            CenterToScreen();
            Invalidate();
        }
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pauseToolStripMenuItem.Text = "Pause";
            pauseToolStripMenuItem.Visible = true;
            File.WriteAllText(globals.Path, "");
            DoubleBuffered = true;
            label1.Visible = false;
            G.CleanCache = true;
            s = 0;
            toolStripMenuItem1.Text = "  Easy";
            globals.curr_cube_size = globals.sizes[0][0];
            globals.curr_board_width = globals.sizes[0][1];
            globals.curr_board_height = globals.sizes[0][2];
            globals.curr_mine_precent = globals.sizes[0][3];
            globals.curr_mine_time = globals.sizes[0][4];
            globals.change_font_size();

            label2.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 32, 0);
            label1.Location = new Point((globals.curr_board_width / 2) * globals.curr_cube_size - 64, 31);

            FormSize();
            Size = new Size(X, Y);
            CenterToScreen();
            Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void FormSize()
        {
            X = globals.curr_board_width * globals.curr_cube_size;
            X += 30;
            Y = globals.curr_board_height * globals.curr_cube_size;
            Y += 105;
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            if (B.GameOverF == false)
            {
                if (label1.Visible == true)
                {
                    label1.Text = "";
                    label1.Visible = false;
                    timer.Start();
                    pauseToolStripMenuItem.Text = "Pause";
                }
                else
                {
                    label1.ForeColor = Color.WhiteSmoke;
                    label1.Text = "    Puase";
                    label1.Visible = true;
                    timer.Stop();
                    pauseToolStripMenuItem.Text = "Resume";
                }
                Update();
            }
            else
            {
                pauseToolStripMenuItem.Visible = false;
                Update();
            }

        }
        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void Folder()
        {
            if (!File.Exists(globals.Path))
            {
                if (!Directory.Exists(globals.workingDirectory))
                {
                    Directory.CreateDirectory(globals.workingDirectory);
                }
                using (FileStream fs = File.Create(globals.Path)) {}
            }
        }
    }
}
