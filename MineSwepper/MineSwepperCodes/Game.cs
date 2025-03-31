using System.IO;
using System.Drawing;
using MineSwepper.Draw;
using System.Windows.Forms;
using MineSwepperGame.MineSwepperCodes;

namespace MineSwepper.MineSwepperCodes
{
    class Game
    {
        public MainForm Form;

        Cells[,] Cell;

        private Point previousCell = new Point(-1, -1);

        public bool CleanCache = true;

        public void Games(Globals globals)
        {
            if (CleanCache)
            { 
                CreateBoard CB = new CreateBoard();
                Cell = CB.Init(globals.curr_board_width, globals.curr_board_height, globals);

                File.WriteAllText(globals.Path, "");
                CleanCache = false;
            }
        }

        internal void Drawing(Graphics graphics, Label label1, Timer timer, int v, Globals globals)
        {
            graphics.Clear(globals.bgColor);
            Board B = new Board();
            B.DrawB(graphics, Cell, label1, timer, v, globals);
        }

        internal void MouseClickAction(MouseEventArgs e, Label PauseLabel, Timer timer, Label label2, Globals globals)
        {
            // Pause label
            if (PauseLabel.Visible==false)
            {
                string[] Lines = File.ReadAllLines(globals.Path);

                if (Lines.Length != 0 && Lines[0] == "0")
                {
                    Form.Update();
                }

                Board B = new Board();
                int X = e.X;
                int Y = e.Y;

                // padding from left and top
                X -= 7;
                Y -= 60;

                if (X > -1 && Y > -1)
                {
                    X /= globals.curr_cube_size;
                    Y /= globals.curr_cube_size;
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (X < globals.curr_board_width && Y < globals.curr_board_height)
                        {
                            //label1.Text = "[" + X + "," + Y + "]";
                            if (Cell[X, Y].flag == false)
                            {
                                Cell[X, Y].IsClick = true;
                                ZeroCell(Cell, X, Y, PauseLabel, globals);
                            }
                        }
                        if (PauseLabel.Visible == false)
                        {
                            ReDraw();
                        }
                    }
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        if (Y < globals.curr_board_height && X < globals.curr_board_width)
                        {
                            if (Cell[X, Y].flag == true)
                            {
                                Cell[X, Y].flag = false;
                            }
                            else
                            {
                                Cell[X, Y].flag = true;
                            }
                            if (PauseLabel.Visible == false)
                            {
                                ReDraw();
                            }
                        }
                    }
                }
            }
        }
        
        // Hover over cells
        internal void OnMouseMove(MouseEventArgs e, Label PauseLabel, Globals globals, Control boardControl)
        {
            try
            {
                if (!PauseLabel.Visible)
                {
                    int X = e.X - 7;  // Apply padding
                    int Y = e.Y - 60;

                    X /= globals.curr_cube_size;
                    Y /= globals.curr_cube_size;

                    bool inBounds = (X >= 0 && Y >= 0 && X < globals.curr_board_width && Y < globals.curr_board_height);
                    Point currentCell = inBounds ? new Point(X, Y) : new Point(-1, -1);

                    if (previousCell != currentCell)
                    {
                        using (Graphics g = boardControl.CreateGraphics())
                        {
                            // Restore previous cell color
                            if (previousCell.X != -1 && previousCell.Y != -1)
                            {
                                int prevX = previousCell.X;
                                int prevY = previousCell.Y;

                                if (prevX >= 0 && prevY >= 0 && prevX < globals.curr_board_width && prevY < globals.curr_board_height)
                                {
                                    // Restore original color
                                    Brush originalBrush = ((prevX + prevY) % 2 == 0 ? globals.brushGrass1 : globals.brushGrass2);
                                    Pen pen = globals.pen2;
                                    if (!Cell[prevX, prevY].IsClick && !Cell[prevX, prevY].flag)
                                    {
                                        g.FillRectangle(originalBrush, Cell[prevX, prevY].Rect);
                                        g.DrawRectangle(pen, Cell[prevX, prevY].Rect);
                                    }
                                }
                            }

                            // Draw hover ONLY if cell is unclicked, unflagged, and within bounds
                            if (inBounds && !Cell[X, Y].flag && !Cell[X, Y].IsClick)
                            {
                                g.FillRectangle(globals.hoverColor, Cell[X, Y].Rect);
                            }

                            // Update previous cell only if hover was drawn
                            previousCell = currentCell;
                        }
                    }
                }
            }
            catch
            {
                // null object error
            }
        }

        public void ReDraw()
        {
            Form.Refresh();
        }

        private void ZeroCell(Cells[,] cell, int i, int j, Label label1, Globals globals)
        {
            // label1 => Showing Win, Lose and Pause  
            if (cell[i, j].IsMine == false)
            {
                if (j > 0)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i, j - 1].IsClick == false && cell[i, j - 1].IsMine == false)
                    {
                        cell[i, j - 1].IsClick = true;
                        if (cell[i, j - 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i, j - 1, label1, globals);
                        }
                    }
                }
                if (j < globals.curr_board_height - 1)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i, j + 1].IsClick == false && cell[i, j + 1].IsMine == false)
                    {
                        cell[i, j + 1].IsClick = true;
                        if (cell[i, j + 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i, j + 1, label1, globals);
                        }
                    }
                }
                if (i < globals.curr_board_width - 1)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i + 1, j].IsClick == false && cell[i + 1, j].IsMine == false)
                    {
                        cell[i + 1, j].IsClick = true;
                        if (cell[i + 1, j].MineCounter == 0)
                        {
                            ZeroCell(cell, i + 1, j, label1, globals);
                        }
                    }
                }
                if (i > 0)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i - 1, j].IsClick == false && cell[i - 1, j].IsMine == false)
                    {
                        cell[i - 1, j].IsClick = true;
                        if (cell[i - 1, j].MineCounter == 0)
                        {
                            ZeroCell(cell, i - 1, j, label1, globals);
                        }
                    }
                }
                if (i > 0 && j > 0)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i - 1, j - 1].IsClick == false && cell[i - 1, j - 1].IsMine == false)
                    {
                        cell[i - 1, j - 1].IsClick = true;
                        if (cell[i - 1, j - 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i - 1, j - 1, label1, globals);
                        }
                    }
                }
                if (i < globals.curr_board_width - 1 && j > 0)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i + 1, j - 1].IsClick == false && cell[i + 1, j - 1].IsMine == false)
                    {
                        cell[i + 1, j - 1].IsClick = true;
                        if (cell[i + 1, j - 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i + 1, j - 1, label1, globals);
                        }
                    }
                }
                if (i > 0 && j < globals.curr_board_height - 1)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i - 1, j + 1].IsClick == false && cell[i - 1, j + 1].IsMine == false)
                    {
                        cell[i - 1, j + 1].IsClick = true;
                        if (cell[i - 1, j + 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i - 1, j + 1, label1, globals);
                        }
                    }
                }
                if (i < globals.curr_board_width - 1 && j < globals.curr_board_height - 1)
                {
                    if (cell[i, j].MineCounter == 0 && cell[i + 1, j + 1].IsClick == false && cell[i + 1, j + 1].IsMine == false)
                    {
                        cell[i + 1, j + 1].IsClick = true;
                        if (cell[i + 1, j + 1].MineCounter == 0)
                        {
                            ZeroCell(cell, i + 1, j + 1, label1, globals);
                        }
                    }
                }
                string[] Lines = File.ReadAllLines(globals.Path);
                if (Lines.Length == 0)
                {
                    int b = Cell[0, 0].MineCount;
                    int All2 = globals.curr_board_height * globals.curr_board_width;
                    All2 -= b;
                    int SoilCount2 = 0;
                    for (int j2 = 0; j2 < Cell.GetLength(1); j2++)
                    {
                        for (int i2 = 0; i2 < Cell.GetLength(0); i2++)
                        {
                            if (Cell[i2, j2].IsClick == true)
                            {
                                if (Cell[i2, j2].IsMine == false)
                                {
                                    SoilCount2++;
                                }
                            }
                        }
                    }
                    if (All2 == SoilCount2)
                    {
                        Lines = File.ReadAllLines(globals.Path);
                        File.WriteAllText(globals.Path, "1\n9");
                        ReDraw();
                    }
                }
                
            }

            else if (cell[i, j].IsMine == true)
            {
                File.WriteAllText(globals.Path, "1");
                label1.Visible = true;
            }
        }
    }
}