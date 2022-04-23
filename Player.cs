using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public class Player : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public float Radius = 40f;
        public int HeatPoints;
        public int Lives;

        public Player(Point position, int speed = 4)
        {
            Position = position;
            Speed = speed;
        }
        public Player(int x, int y, int speed = 4)
        {
            Position = new Point(x, y);
            Speed = speed;
        }

        public void Move(Size offset)
        {
            Position += Game.CorrectOffsetInBounds(this, offset);
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
        }

        public void Shoot()
        {
            var objects = Game.Levels[Game.CurrentLevelNum].Objects;
            var player = Game.Player;
            objects.Add(new Bullet(player.Position, player.Direction));
        }
    }
}
