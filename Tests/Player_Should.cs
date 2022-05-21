using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Game_.Tests
{
    [TestFixture]
    public class Player_Should
    {
        [SetUp]
        public void SetUp()
        {
            Game.MapWidth = 500;
            Game.MapHeight = 500;
            Game.Levels[0] = new Level(new List<IDrawable>(), null, new Point(100, 100),1);
        }

        public void CheckMove(Size offset)
        {
            Game.Player.Position = new Point(100, 100);
            Game.Player.Move(offset);
            Assert.AreEqual(new Point(100, 100) + offset, Game.Player.Position);

        }

        public void CheckShoot(bool isRapid, int initalScores,
            int bulletsCount, int expectedScores)
        {
            Game.Scores = initalScores;
            Game.Player.IsRapid = isRapid;
            Game.Player.Shoot();
            Assert.AreEqual(bulletsCount, Game.Levels[0].Bullets.Count);
            Assert.AreEqual(expectedScores, Game.Scores);
        }

        public void CheckBonuses(Bonus bonus, bool expectedIsRapid, int hp, int expectedHp)
        {
            var bonusItem = new BonusItem( new Point(0,0), 0, bonus);
            Game.Player.HeatPoints = hp;
            Game.Player.AddBonus(bonusItem);
            Assert.AreEqual(expectedIsRapid, Game.Player.IsRapid);
            Assert.AreEqual(expectedHp, Game.Player.HeatPoints);
        }

        [Test]
        public void MoveRight() { CheckMove(new Size(10, 0)); }

        [Test]
        public void MoveLeft() { CheckMove(new Size(-10, 0)); }

        [Test]
        public void MoveUp() { CheckMove(new Size(0, -10)); }

        [Test]
        public void MoveDown() { CheckMove(new Size(0, 10)); }

        [Test]
        public void StandartShoot() { CheckShoot(false, 5, 1, 4); }

        [Test]
        public void RapidShoot() { CheckShoot(true, 5, 2, 4); }

        [Test]
        public void ZeroScoresShoot() { CheckShoot(false, 0, 1, 0); }

        [Test]
        public void HealBonus() { CheckBonuses(Bonus.Heal, false, 20, 40); }

        [Test]
        public void ExtraHeal() { CheckBonuses(Bonus.Heal, false, 35, 50); }

        [Test]
        public void RapidBonus() { CheckBonuses(Bonus.Rapid, true, 35, 35); }
    }
}
