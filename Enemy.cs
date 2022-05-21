using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public class Enemy : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }

        private int timeWithTarget { get; set; }
        private Point target { get; set; }
        public Enemy(Point intialPosition, float direction, int speed = 3, int radius = 30)
        {
            Position = intialPosition;
            Direction = direction;
            Speed = speed;
            Radius = radius;
        }
        public void Move(Size offset)
        {
            offset = GetMoveDirection();
            Direction = (float)Math.Atan2(Position.Y - Game.Player.Position.Y, Position.X - Game.Player.Position.X);
            Position += Game.SolveCollision(this, offset);
            Position += Game.CorrectOffsetInBounds(this, offset);
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
        }

        public Size GetMoveDirection()
        {
            var distance = Game.GetDistance(Position, target);
            if (distance < 300 || target == Point.Empty
                || timeWithTarget > 30)
            {
                var rnd = new Random();
                target = new Point(100 + (CollatzAlgoritm(Position.X + rnd.Next(-10, 10)) % (Game.MapWidth - 200)),
                   100 + ((rnd.Next(-5, 5) % 2 == 0) ? 1 : -1)
                   * (CollatzAlgoritm(Position.Y + rnd.Next(-10, 10)) % (Game.MapHeight - 200)));
                timeWithTarget = 0;
            }

            var toTarget = Math.Atan2(Position.Y - target.Y, Position.X - target.X);
            timeWithTarget++;

            var offset = new Size(-(int)(Math.Cos(toTarget) * Speed), -(int)(Math.Sin(toTarget) * Speed));

            CorrectBetweenObjects(ref offset);
            CorrectBetweenBounds(ref offset);

            return offset;
        }
        private int CollatzAlgoritm(int x)
        {
            for (var i = 0; i < 7; i++)
            {
                if (x % 2 == 0)
                    x /= 2;
                else
                    x = 3 * x + 1;
            }
            return x;
        }
        private void CorrectBetweenObjects(ref Size offset)
        {
            foreach (var obj in Game.Levels[Game.CurrentLevelNum].Objects)
            {
                if ((obj is Enemy || obj is Table) && obj != this
                    && Game.GetDistance(this, obj) < 60)
                {
                    offset += new Size((Position.X - obj.Position.X) / 30, (Position.Y - obj.Position.Y) / 30);
                }
            }
        }
        private void CorrectBetweenBounds(ref Size offset)
        {
            if (Position.X < 40)
                offset += new Size(40 - Position.X, 0);
            if (Position.X > Game.MapWidth - 40)
                offset += new Size(Game.MapWidth - 40 - Position.X, 0);
            if (Position.Y < 40)
                offset += new Size(0, 40 - Position.Y);
            if (Position.Y > Game.MapHeight - 40)
                offset += new Size(0, Game.MapHeight - 40 - Position.Y);

        }

        public string GetImage()
        {
            return "Enemy";
        }

        public void Shoot()
        {
            var bullets = Game.Levels[Game.CurrentLevelNum].Bullets;
            bullets.Add(new Bullet(Position, Direction + 15.75f, OwnerType.Enemy));
        }
    }
}
