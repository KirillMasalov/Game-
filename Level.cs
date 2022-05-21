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
        public List<IDrawable> ObjectsToDelete = new List<IDrawable>();
        public List<IDrawable> ObjectsToCreate = new List<IDrawable>();
        public List<Bullet> Bullets = new List<Bullet>();
        public int EnemiesCount { get; set; }
        public Image BackGround;
        public Point PlayerInitialPos { get; set; }

        public Level(List<IDrawable> objects, Image background, Point initialPos, int enemiesCount)
        {
            Objects = objects;
            BackGround = background;
            PlayerInitialPos = initialPos;
            EnemiesCount = enemiesCount;
        }
    }
}
