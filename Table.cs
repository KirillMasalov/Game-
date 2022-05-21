using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public class Table : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }

        public Table(Point position, float direction = 0, int speed = 0, int radius = 80)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
        }

        public void Move(Size offset)
        {
            Position += offset;
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
        }

        public string GetImage()
        {
            return "Table";
        }
    }
}
