using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_
{
    public class DeadBoss : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }

        public DeadBoss(Point position, float direction, int speed = 0, int radius = 80)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
        }

        public string GetImage()
        {
            return "BossDead";
        }

        public void Move(Size offset)
        {
            Position += offset;
        }

        public void Move(int x, int y)
        {
            Move(new Size(x, y));
        }
    }
}
