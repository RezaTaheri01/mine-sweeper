using MineSwepperGame.MineSwepperCodes;
using MineSwepper.Draw;
using System.Drawing;
using System;

//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Collections.Generic;


namespace MineSwepper.MineSwepperCodes
{
    class CreateBoard
    {
        public Cells[,] Cell;
        int Plus = 0;
        int PMine = 0;
        int X, Y;
        int FirstX = 7, FirstY = 60;
        int WidthRec = 25;
        int HeightRec = 25;
        public int i2, j2;
        Board B = new Board();

        public Cells[,] Init(int CcellX, int CcellY, Globals globals)
        {
            i2 = CcellX; j2 = CcellY;
            X = FirstX; Y = FirstY;

            WidthRec = globals.curr_cube_size;
            HeightRec = globals.curr_cube_size;
            Plus = globals.curr_cube_size;
            PMine = globals.curr_mine_precent;

            Size size = new Size(WidthRec, HeightRec);
            Size size2 = new Size(WidthRec/2, HeightRec/2);
            Cell = new Cells[CcellX, CcellY];
            Random R = new Random();
            for (int j = 0; j < Cell.GetLength(1); j++)
            {
                for (int i = 0; i < Cell.GetLength(0); i++)
                {
                    Point Loc = new Point(X, Y);
                    Point Loc2 = new Point(X+(WidthRec/4), Y+(HeightRec/4));
                    Cell[i, j] = new Cells();
                    Cell[i, j].Rect = new Rectangle(Loc, size);
                    Cell[i, j].MineRect = new Rectangle(Loc2, size2);
                    Cell[i, j].i = i;
                    Cell[i, j].j = j;
                    int P = R.Next(0, 100);
                    if (P < PMine)
                    {
                        Cell[i, j].IsMine = true;
                        Cell[0,0].MineCount += 1;
                    }
                    else
                    {
                        Cell[i, j].IsMine = false;
                    }
                    X += Plus;
                }
                X = FirstX;
                Y += Plus;   
            }
            MineCount(Cell);
            return Cell;
        }

        public void MineCount(Cells [,] Cell)
        {
            for (int j = 0; j < Cell.GetLength(1); j++)
            {
                for (int i = 0; i < Cell.GetLength(0); i++)
                {
                    Cell[i, j].MineCounter = CalculateMine(Cell, i, j);
                }
            }
        }

        private int CalculateMine(Cells[,] cell, int iIndex, int jIndex)
        {
            if(Cell[iIndex,jIndex].IsMine==true)
            {
                return 0;
            }
            int Counter=0;
            for(int j=jIndex-1; j<=jIndex+1;j++)
            {
                for(int i=i=iIndex-1;i<=iIndex+1;i++)
                {
                    if (i > -1 && j > -1 && i < i2 && j < j2)
                    {
                        if (cell[i, j].IsMine == true)
                        {
                            Counter++;
                        }
                    }
                }
            }
            return Counter;
        }
    }
}
