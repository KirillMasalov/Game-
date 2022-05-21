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
        public int Radius { get; set; }
        public int HeatPoints { get; set; }
        public int Lives { get; set; }
        public bool IsRapid { get; set; }

        public Player(Point position, int speed = 8, int radius = 40,
            int hp = 50, int lives = 3, bool rapid = false)
        {
            Position = position;
            Speed = speed;
            Radius = radius;
            HeatPoints = hp;
            Lives = lives;
            IsRapid = rapid;
        }
        public Player(int x, int y, int speed = 8, int radius = 40,
            int hp = 50, int lives = 3, bool rapid = false)
        {
            Position = new Point(x, y);
            Speed = speed;
            Radius = radius;
            HeatPoints = hp;
            Lives = lives;
            IsRapid = rapid;
        }

        public string GetImage()
        {
            return "Player";
        }

        public void Move(Size offset)
        {
            Position += Game.SolveCollision(this, offset);
            Position += Game.CorrectOffsetInBounds(this, offset);
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
        }

        public void Shoot()
        {
            Game.Scores = (Game.Scores == 0) ? 0 : Game.Scores - 1;
            var bullets = Game.Levels[Game.CurrentLevelNum].Bullets;
            var player = Game.Player;
            bullets.Add(new Bullet(player.Position, player.Direction, OwnerType.Player));
            if (IsRapid)
                bullets.Add(new Bullet(player.Position + new Size((int)(Math.Cos(player.Direction) * 35),
                    (int)(Math.Sin(player.Direction) * 35)), player.Direction, OwnerType.Player));
            Game.Sounds.Add("Shoot");
        }

        public void AddBonus(BonusItem bonus)
        {
            if (bonus.ItemBonus == Bonus.Heal)
            {
                HeatPoints = (HeatPoints <= 30) ? HeatPoints + 20 : 50;
                Game.Sounds.Add("Heal");
            }
            else
            {
                IsRapid = true;
                Game.Sounds.Add("Rapid");
            }
        }
    }
}
