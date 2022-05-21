using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_
{
    public class Boss : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public int HeatPoints { get; set; }

        private Point targetMove { get; set; }

        public Boss(Point position, float direction = 0, int speed = 10,
            int radius = 50, int hp = 200)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
            HeatPoints = hp;
        }

        public string GetImage()
        {
            return "Boss";
        }

        public void Move(Size offset)
        {
            var rnd = new Random();
            if (targetMove == Point.Empty || Math.Abs(Position.Y - targetMove.Y) <= 12)
                targetMove = new Point(Position.X, rnd.Next(50, Game.MapHeight - 50));
            offset += new Size(0, Math.Sign(targetMove.Y - Position.Y) * Speed);
            Position += offset;
            Direction = (float)Math.Atan2(Position.Y - Game.Player.Position.Y, Position.X - Game.Player.Position.X);
        }

        public void Move(int x, int y)
        {
            Move(new Size(x, y));
        }

        public void Shoot()
        {
            var bullets = Game.Levels[Game.CurrentLevelNum].Bullets;
            bullets.Add(new Bullet(Position, Direction - 15.75f, OwnerType.Enemy));
        }
    }
}
