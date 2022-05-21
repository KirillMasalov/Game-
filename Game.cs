using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Windows.Forms;


namespace Game_
{
    public static class Game
    {
        public static int MapWidth;
        public static int MapHeight;
        public static int InterfaceHeight = 100;

        public static Level[] Levels = new Level[9];
        public static int CurrentLevelNum = 0;

        public static Player Player = new Player(70, 530);
        public static int Scores { get; set; }
        public static bool IsPaused { get; set; }
        public static bool IsOver { get; set; }

        public static HashSet<Keys> PressedKeys = new HashSet<Keys>();
        public static HashSet<string> Sounds = new HashSet<string>();

        public static Size ConvertKeysToOffset()
        {
            var resultDirection = new Size();
            foreach (var key in PressedKeys)
            {
                switch (key)
                {
                    case Keys.A:
                        resultDirection += new Size(-Player.Speed, 0);
                        break;
                    case Keys.D:
                        resultDirection += new Size(Player.Speed, 0);
                        break;
                    case Keys.W:
                        resultDirection += new Size(0, -Player.Speed);
                        break;
                    case Keys.S:
                        resultDirection += new Size(0, Player.Speed);
                        break;
                }
            }
            return resultDirection;
        }

        public static Size CorrectOffsetInBounds(IDrawable obj, Size offset)
        {
            if (obj.Position.X + offset.Width > MapWidth + 20 - obj.Radius)
                offset.Width -= obj.Position.X + obj.Radius + offset.Width - MapWidth - 20;
            if (obj.Position.X + offset.Width < obj.Radius - 20)
                offset.Width -= obj.Position.X - obj.Radius + offset.Width + 20;
            if (obj.Position.Y + offset.Height > MapHeight + 20 - obj.Radius)
                offset.Height -= obj.Position.Y + obj.Radius + offset.Height - MapHeight - 20;
            if (obj.Position.Y + offset.Height < obj.Radius - 20)
                offset.Height -= obj.Position.Y - obj.Radius + offset.Height + 20;
            return offset;
        }

        public static Size SolveCollision(IDrawable obj, Size offset)
        {
            var resultOffset = new Size(0, 0);
            foreach (var collisionObj in Levels[CurrentLevelNum].Objects)
            {
                if (collisionObj is Bullet || collisionObj == obj
                    || collisionObj is DeadEnemy || collisionObj is DeadBoss
                    || (collisionObj is BonusItem && obj is Enemy))
                    continue;

                if (GetDistance(obj.Position + new Size(offset.Width,0), collisionObj.Position) < obj.Radius + collisionObj.Radius - 20)
                {
                    if (collisionObj is BonusItem && obj is Player)
                    {
                        (obj as Player).AddBonus(collisionObj as BonusItem);
                        Levels[CurrentLevelNum].ObjectsToDelete.Add(collisionObj);
                    }
                    else
                        resultOffset += new Size(-offset.Width, 0);
                }

                if (GetDistance(obj.Position + new Size(0, offset.Height), collisionObj.Position) < obj.Radius + collisionObj.Radius - 20)
                {
                    if (collisionObj is BonusItem && obj is Player)
                    {
                        (obj as Player).AddBonus(collisionObj as BonusItem);
                        Levels[CurrentLevelNum].ObjectsToDelete.Add(collisionObj);
                    }
                    else
                        resultOffset += new Size(0, - offset.Height);
                }
            }
            return resultOffset;
        }

        public static double GetDistance(IDrawable first, IDrawable second)
        {
            return GetDistance(first.Position, second.Position);
        }

        public static double GetDistance(Point first, Point second)
        {
            var dx = first.X - second.X;
            var dy = first.Y - second.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static void MoveObjects()
        {
            foreach (var obj in Levels[CurrentLevelNum].Objects)
            {
                obj.Move(new Size(0, 0));
            }
            foreach (var bullet in Levels[CurrentLevelNum].Bullets)
            {
                var offset = new Size((int)(Math.Cos(bullet.Direction) * bullet.Speed),
                    (int)(Math.Sin(bullet.Direction) * bullet.Speed));
                bullet.Move(offset);
            }
        }

        public static void EnemiesShoots()
        {
            foreach (var enemy in Levels[CurrentLevelNum].Objects.Where(obj => obj is Enemy)
                .Select(obj => obj as Enemy))
                enemy.Shoot();
        }

        public static void BossShoot()
        {
            var boss = Levels[CurrentLevelNum].Objects.Where(obj => obj is Boss).FirstOrDefault();
            if (boss != null)
                (boss as Boss).Shoot();
        }

        public static void DeleteObjects()
        {
            foreach (var obj in Levels[CurrentLevelNum].ObjectsToDelete)
            {
                if (obj is Bullet)
                    Levels[CurrentLevelNum].Bullets.Remove(obj as Bullet);
                else
                    Levels[CurrentLevelNum].Objects.Remove(obj);
            }
            Levels[CurrentLevelNum].ObjectsToDelete.Clear();
        }

        public static void CreateObjects()
        {
            foreach (var obj in Levels[CurrentLevelNum].ObjectsToCreate)
                Levels[CurrentLevelNum].Objects.Insert(0, obj);
            Levels[CurrentLevelNum].ObjectsToCreate.Clear();
        }

        public static void ToNextLevel()
        {
            CurrentLevelNum++;
            Player.Position = Levels[CurrentLevelNum].PlayerInitialPos;
        }
        public static void GameOver()
        {
            IsOver = true;
        }

        public static void ResetGame()
        {
            IsOver = false;
            Scores = 0;
            Player = new Player(new Point(70, 530));
            CurrentLevelNum = 0;
        }

        public static void CreateLevels(Dictionary<string, Image> images)
        {
            Levels[0] = new Level(new List<IDrawable>(), images["TutorialBack"], ConvertToScreenSize(100, 100), 1);
            Levels[0].Objects.Add(new Enemy(ConvertToScreenSize(1300, 70), 0));
            Levels[0].Objects.Add(new Barrel(ConvertToScreenSize(1200, 70), content: Bonus.Rapid));
            Levels[0].Objects.Add(new Barrel(ConvertToScreenSize(1200, 135), content: Bonus.Nothing));
            Levels[0].Objects.Add(new Barrel(ConvertToScreenSize(1260, 135), content: Bonus.Heal));
            Levels[0].Objects.Add(new Barrel(ConvertToScreenSize(1320, 135), content: Bonus.Nothing));

            Levels[1] = new Level(new List<IDrawable>(), images["BackGround"], ConvertToScreenSize(100, 100), 4);
            Levels[1].Objects.Add(new Table(ConvertToScreenSize(300, 400)));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(600, 70), content: Bonus.Heal));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(665, 70), content: Bonus.Nothing));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(600, 135), content: Bonus.Nothing));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(1320, 550), content: Bonus.Heal));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(1320, 490), content: Bonus.Nothing));
            Levels[1].Objects.Add(new Barrel(ConvertToScreenSize(1260, 550), content: Bonus.Nothing));
            Levels[1].Objects.Add(new Enemy(ConvertToScreenSize(1100, 150), 0));
            Levels[1].Objects.Add(new Enemy(ConvertToScreenSize(1100, 300), 0));
            Levels[1].Objects.Add(new Enemy(ConvertToScreenSize(1100, 450), 0));
            Levels[1].Objects.Add(new Enemy(ConvertToScreenSize(800, 300), 0));

            Levels[2] = new Level(new List<IDrawable>(), images["BackGround"], ConvertToScreenSize(100, 300), 6);
            Levels[2].Objects.Add(new Table(ConvertToScreenSize(350, 200)));
            Levels[2].Objects.Add(new Table(ConvertToScreenSize(650, 490)));
            Levels[2].Objects.Add(new Table(ConvertToScreenSize(1000, 200)));
            Levels[2].Objects.Add(new Barrel(ConvertToScreenSize(1150, 470), content: Bonus.Nothing));
            Levels[2].Objects.Add(new Barrel(ConvertToScreenSize(1150, 530), content: Bonus.Heal));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(400, 500), 0));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(500, 200), 0));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(675, 300), 0));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(800, 200), 0));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(900, 500), 0));
            Levels[2].Objects.Add(new Enemy(ConvertToScreenSize(1200, 500), 0));

            Levels[3] = new Level(new List<IDrawable>(), images["BackGround"], ConvertToScreenSize(100, 500), 7);
            Levels[3].Objects.Add(new Barrel(ConvertToScreenSize(300, 80), content: Bonus.Heal));
            for (var i = 0; i < 4; i++)
                Levels[3].Objects.Add(new Barrel(ConvertToScreenSize(300 + 75 * i, 155), content: Bonus.Nothing));
            Levels[3].Objects.Add(new Barrel(ConvertToScreenSize(600, 540), content: Bonus.Rapid));
            for (var i = 0; i < 6; i++)
                Levels[3].Objects.Add(new Barrel(ConvertToScreenSize(600 + 75 * i, 465), content: Bonus.Nothing));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(500, 500), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(900, 250), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(900, 400), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(400, 100), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(500, 100), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(700, 550), 0));
            Levels[3].Objects.Add(new Enemy(ConvertToScreenSize(800, 550), 0));

            Levels[4] = new Level(new List<IDrawable>(), images["BackGround"], ConvertToScreenSize(100, 300), 6);
            Levels[4].Objects.Add(new Table(ConvertToScreenSize(300, 200)));
            Levels[4].Objects.Add(new Table(ConvertToScreenSize(300, 400)));
            Levels[4].Objects.Add(new Table(ConvertToScreenSize(900, 110)));
            Levels[4].Objects.Add(new Table(ConvertToScreenSize(900, 490)));
            for (var i = 0; i < 3; i++)
                Levels[4].Objects.Add(new Barrel(ConvertToScreenSize(900, 225 + 75 * i), content: Bonus.Nothing));
            for (var i = 0; i < 4; i++)
                Levels[4].Objects.Add(new Enemy(ConvertToScreenSize(420, 150 + 75 * i), 0));
            Levels[4].Objects.Add(new Enemy(ConvertToScreenSize(1100, 110), 0));
            Levels[4].Objects.Add(new Enemy(ConvertToScreenSize(1100, 490), 0));

            Levels[5] = new Level(new List<IDrawable>(), images["BackGround2"], ConvertToScreenSize(100, 300), 9);
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(320, 550), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(320, 475), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(545, 550), content: Bonus.Heal));
            for (var i = 0; i < 6; i++)
                Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(320 + 75 * i, 400), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(320, 180), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(400, 100), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(475, 180), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(1100, 150), content: Bonus.Nothing));
            Levels[5].Objects.Add(new Barrel(ConvertToScreenSize(1175, 160), content: Bonus.Rapid));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(395, 475), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(600, 550), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(600, 475), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(300, 100), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(600, 100), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(430, 160), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(900, 200), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(950, 240), 0));
            Levels[5].Objects.Add(new Enemy(ConvertToScreenSize(1200, 500), 0));

            Levels[6] = new Level(new List<IDrawable>(), images["BackGround2"], ConvertToScreenSize(100, 300), 12);
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(320, 180), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(350, 280), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(350, 360), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(320, 450), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(480, 520), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(470, 450), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(500, 380), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(550, 560), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(420, 120), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(490, 80), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(600, 60), content: Bonus.Heal));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(570, 120), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(570, 200), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(600, 260), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(700, 240), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(680, 300), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(680, 375), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(700, 435), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(800, 200), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(900, 300), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(1000, 200), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(900, 100), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(1100, 100), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(1200, 500), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(1100, 550), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Barrel(ConvertToScreenSize(1250, 380), content: Bonus.Nothing));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(395, 550), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(395, 250), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(200, 100), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(550, 450), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(570, 525), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(740, 280), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(740, 355), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(740, 120), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(800, 500), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(900, 480), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(1000, 450), 0));
            Levels[6].Objects.Add(new Enemy(ConvertToScreenSize(1100, 400), 0));

            Levels[7] = new Level(new List<IDrawable>(), images["BackGround2"], ConvertToScreenSize(100, 300), 2);
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(200, 100)));
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(650, 100)));
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(1100, 100)));
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(200, 500)));
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(650, 500)));
            Levels[7].Objects.Add(new Table(ConvertToScreenSize(1100, 500)));
            Levels[7].Objects.Add(new Barrel(ConvertToScreenSize(850, 100), content: Bonus.Nothing));
            Levels[7].Objects.Add(new Barrel(ConvertToScreenSize(400, 500), content: Bonus.Nothing));
            Levels[7].Objects.Add(new Enemy(ConvertToScreenSize(1100, 250), 0));
            Levels[7].Objects.Add(new Enemy(ConvertToScreenSize(1100, 350), 0));

            Levels[8] = new Level(new List<IDrawable>(), images["BackGround2"], ConvertToScreenSize(100, 300), 2);
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(100, 150)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(100, 450)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(300, 150)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(300, 450)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(500, 150)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(500, 450)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(700, 80)));
            Levels[8].Objects.Add(new Table(ConvertToScreenSize(700, 520)));
            for (var i = 0; i < 5; i++)
                Levels[8].Objects.Add(new Barrel(ConvertToScreenSize(1000, 60 + 120 * i), content: Bonus.Nothing));
            Levels[8].Objects.Add(new Boss(ConvertToScreenSize(1200, 300)));
        }

        private static Point ConvertToScreenSize(int x, int y)
        {
            return new Point(x * MapWidth / 1360, y * MapHeight / 600);
        }
    }
}
