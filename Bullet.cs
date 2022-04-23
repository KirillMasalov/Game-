using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    class Bullet : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }

        public Bullet(Point position, float direction, int speed = 15)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
        }

        public void Move(Size offset)
        {
            Position += offset;
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
        }
    }
}
