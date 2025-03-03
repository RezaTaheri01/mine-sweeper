using System.Drawing;

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
