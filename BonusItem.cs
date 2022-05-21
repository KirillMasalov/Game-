using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_
{
    public class BonusItem : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public Bonus ItemBonus { get; set; }

        public BonusItem(Point position, float direction, Bonus bonus,
            int speed = 0, int radius = 30)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
            ItemBonus = bonus;
        }
        public string GetImage()
        {
            return (ItemBonus == Bonus.Heal) ? "Heal" : "Rapid";
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
