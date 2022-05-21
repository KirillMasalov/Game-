using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_
{
    public class Barrel : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public int Heatpoints { get; set; }
        public Bonus Content { get; set; }

        public Barrel(Point position, float direction = 0, int speed = 0,
            int radius = 30, Bonus content = Bonus.Nothing, int heatpoint = 20)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
            Heatpoints = heatpoint;
            Content = content;
        }
        public string GetImage()
        {
            return "Barrel";
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
