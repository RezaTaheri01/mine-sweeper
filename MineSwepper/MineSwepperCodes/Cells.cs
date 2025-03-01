using System.Drawing;

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace MineSwepper.MineSwepperCodes
{
    class Cells
    {
       public int i, j;
       public  Rectangle Rect;
       public Rectangle MineRect;
       public bool IsMine = false;
       public bool flag = false;
       public bool IsClick = false;
       public int MineCounter;
       public int MineCount;
    }
}
