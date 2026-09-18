using System.IO;
using System.Drawing;
using MineSwepper.Draw;
using System.Windows.Forms;
using MineSwepperGame.MineSwepperCodes;
using System.Collections.Generic;
using System;
using System.Linq;

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
                            if (Cell[X, Y].flag == false && Cell[X, Y].IsClick == false)
                            {
                                Cell[X, Y].IsClick = true;
                                ZeroCell(Cell, X, Y, PauseLabel, globals);
                                if (globals.assistOn)
                                {
                                    SuggestNextMove(globals);
                                }
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


        public void SuggestNextMove(Globals globals)
        {
            // Lists for safe moves and guaranteed bombs
            List<Point> safeMoves = new List<Point>();
            List<Point> guaranteedBombs = new List<Point>();
            // Dictionary to store bomb probabilities for unclicked, unflagged cells
            Dictionary<Point, double> bombProbabilities = new Dictionary<Point, double>();

            // Step 1: Analyze all clicked cells to find safe moves, guaranteed bombs, and probabilities
            for (int i = 0; i < globals.curr_board_width; i++)
            {
                for (int j = 0; j < globals.curr_board_height; j++)
                {
                    if (Cell[i, j].IsClick && Cell[i, j].MineCounter > 0)
                    {
                        List<Point> neighbors = GetNeighbors(i, j, globals);
                        int unclickedNeighbors = 0;
                        int flaggedNeighbors = 0;

                        // Count unclicked and flagged neighbors
                        foreach (var neighbor in neighbors)
                        {
                            if (!Cell[neighbor.X, neighbor.Y].IsClick && !Cell[neighbor.X, neighbor.Y].flag)
                            {
                                unclickedNeighbors++;
                            }
                            if (Cell[neighbor.X, neighbor.Y].flag)
                            {
                                flaggedNeighbors++;
                            }
                        }

                        // Case 1: All mines are flagged -> remaining unclicked neighbors are safe (0% probability)
                        if (Cell[i, j].MineCounter == flaggedNeighbors && unclickedNeighbors > 0)
                        {
                            foreach (var neighbor in neighbors)
                            {
                                if (!Cell[neighbor.X, neighbor.Y].IsClick && !Cell[neighbor.X, neighbor.Y].flag)
                                {
                                    safeMoves.Add(neighbor);
                                }
                            }
                        }
                        // Case 2: Unclicked neighbors exactly match remaining mines -> they are all bombs (100% probability)
                        else if (Cell[i, j].MineCounter - flaggedNeighbors == unclickedNeighbors && unclickedNeighbors > 0)
                        {
                            foreach (var neighbor in neighbors)
                            {
                                if (!Cell[neighbor.X, neighbor.Y].IsClick && !Cell[neighbor.X, neighbor.Y].flag)
                                {
                                    guaranteedBombs.Add(neighbor);
                                }
                            }
                        }
                        // Case 3: Calculate bomb probabilities for remaining cases
                        else if (unclickedNeighbors > 0 && Cell[i, j].MineCounter > flaggedNeighbors)
                        {
                            double probability = (double)(Cell[i, j].MineCounter - flaggedNeighbors) / unclickedNeighbors;
                            foreach (var neighbor in neighbors)
                            {
                                if (!Cell[neighbor.X, neighbor.Y].IsClick && !Cell[neighbor.X, neighbor.Y].flag)
                                {
                                    if (!bombProbabilities.ContainsKey(neighbor))
                                    {
                                        bombProbabilities[neighbor] = probability;
                                    }
                                    else
                                    {
                                        // Use the maximum probability for overlapping constraints
                                        bombProbabilities[neighbor] = Math.Max(bombProbabilities[neighbor], probability);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //// Step 2: Suggest a safe move if available (0% bomb probability)
            //if (safeMoves.Count > 0)
            //{
            //    Point safeMove = safeMoves[0]; // Pick the first safe move
            //    Console.WriteLine($"Suggesting safe cell at ({safeMove.X}, {safeMove.Y}) with 0% chance of being a bomb.");
            //    return;
            //}

            // Step 3: Flag guaranteed bombs and redraw if any are found
            if (guaranteedBombs.Count > 0)
            {
                foreach (var bomb in guaranteedBombs)
                {
                    Cell[bomb.X, bomb.Y].flag = true;
                }
                //Console.WriteLine($"Flagged {guaranteedBombs.Count} guaranteed bomb(s).");
                //ReDraw();
                return;
            }

            //// Step 4: Suggest the cell with the lowest bomb probability
            //if (bombProbabilities.Count > 0)
            //{
            //    var minProbabilityCell = bombProbabilities.OrderBy(kvp => kvp.Value).First();
            //    Console.WriteLine($"No clear safe move. Suggesting cell at ({minProbabilityCell.Key.X}, {minProbabilityCell.Key.Y}) with lowest bomb probability of {minProbabilityCell.Value:P2}.");
            //    return;
            //}

            //// Step 5: Fallback - suggest a random unclicked cell if no probabilities are available
            //for (int i = 0; i < globals.curr_board_width; i++)
            //{
            //    for (int j = 0; j < globals.curr_board_height; j++)
            //    {
            //        if (!Cell[i, j].IsClick && !Cell[i, j].flag)
            //        {
            //            Console.WriteLine($"No probability data available. Randomly suggesting cell at ({i}, {j}).");
            //            return;
            //        }
            //    }
            //}

            //Console.WriteLine("No unclicked cells left to suggest.");
        }


        private List<Point> GetNeighbors(int x, int y, Globals globals)
        {
            List<Point> neighbors = new List<Point>();

            // Check all 8 possible neighbors (up, down, left, right, and diagonals)
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    // Skip the center cell (itself)
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    // Check if the neighbor is within bounds
                    if (nx >= 0 && ny >= 0 && nx < globals.curr_board_width && ny < globals.curr_board_height)
                    {
                        if (Cell[nx, ny].IsClick == false)
                        {
                            neighbors.Add(new Point(nx, ny));
                        }
                    }
                }
            }

            return neighbors;
        }

    }
}