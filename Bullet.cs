using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_
{
    public class Bullet : IDrawable
    {
        public Point Position { get; set; }
        public float Direction { get; set; }
        public int Speed { get; set; }
        public int Radius { get; set; }
        public OwnerType Owner;

        public Bullet(Point position, float direction, OwnerType owner, int speed = 20, int radius = 10)
        {
            Position = position;
            Direction = direction;
            Speed = speed;
            Radius = radius;
            Owner = owner;
        }

        public void Move(Size offset)
        {
            Position += offset;
            CheckToDelete();
        }
        public void Move(int dx, int dy)
        {
            Move(new Size(dx, dy));
            CheckToDelete();
        }

        public string GetImage()
        {
            throw new InvalidOperationException("Для пуль не предусмотрено изображение");
        }
        private void CheckToDelete()
        {
            if (Position.X < 0 || Position.X > Game.MapWidth
                || Position.Y < 0 || Position.Y > Game.MapHeight)
                Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(this);

            foreach (var obj in Game.Levels[Game.CurrentLevelNum].Objects)
            {
                if (obj is Bullet || obj is Table
                    || obj is DeadEnemy || obj is DeadBoss)
                    continue;
                if (Game.GetDistance(this, obj) < obj.Radius
                    && ((Owner == OwnerType.Player && (obj is Enemy || obj is Boss))
                    || (obj is Barrel)))
                {
                    DealDamage(obj);
                    Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(this);
                }
            }
            if (Owner == OwnerType.Enemy
                && Game.GetDistance(this, Game.Player) < Game.Player.Radius)
            {
                DealDamage(Game.Player);
                Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(this);
            }
        }

        private void DealDamage(IDrawable obj)
        {
            if (Owner == OwnerType.Player && obj is Enemy)
            {
                Game.Sounds.Add("EnemyDead");
                Game.Scores += 10;
                Game.Levels[Game.CurrentLevelNum].EnemiesCount--;
                Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(obj);
                Game.Levels[Game.CurrentLevelNum].ObjectsToCreate.Add(new DeadEnemy(obj.Position, obj.Direction));
            }

            if (Owner == OwnerType.Enemy && obj is Player)
            {
                var player = Game.Player;
                player.HeatPoints -= 10;
                Game.Sounds.Add("SDamage");
                if (player.HeatPoints <= 0)
                {
                    player.Lives--;
                    player.IsRapid = false;
                    if (player.Lives < 1)
                    {
                        Game.Sounds.Add("Death");
                        Game.GameOver();
                    }
                    else
                    {
                        player.HeatPoints = 50;
                        Game.Sounds.Add("Damage");
                    }
                }
            }
            if (obj is Barrel)
            {
                var barrel = obj as Barrel;
                barrel.Heatpoints -= 5;
                if (barrel.Heatpoints <= 0)
                {
                    Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(barrel);
                    if (barrel.Content != Bonus.Nothing)
                        Game.Levels[Game.CurrentLevelNum].ObjectsToCreate
                            .Add((new BonusItem(barrel.Position, barrel.Direction, barrel.Content)));
                    Game.Sounds.Add("BarrelBreak");
                }
            }
            if (obj is Boss)
            {
                var boss = obj as Boss;
                boss.HeatPoints -= 5;
                if (boss.HeatPoints <= 0)
                {
                    Game.Sounds.Add("EnemyDead");
                    Game.Levels[Game.CurrentLevelNum].ObjectsToDelete.Add(boss);
                    Game.Levels[Game.CurrentLevelNum].ObjectsToCreate.Add(new DeadBoss(obj.Position, obj.Direction));
                    Game.GameOver();
                }
            }
        }
    }
}
