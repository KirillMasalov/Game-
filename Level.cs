using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Game_
{

    public class Level
    {
        public List<IDrawable> Objects = new List<IDrawable>();
        public int EnemiesCount { get; set; }
        public Image BackGround;
        public Point PlayerInitialPos { get; set; }
        public static bool IsWin;

        public Level(List<IDrawable> objects, Image background, Point initialPos)
        {
            Objects = objects;
            BackGround = background;
            PlayerInitialPos = initialPos;
        }
    }
}
