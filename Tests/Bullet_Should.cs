using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Windows.Forms;
using System.Drawing;

namespace Game_.Tests
{
    [TestFixture]
    public class Bullet_Should
    {
        [SetUp]
        public void SetUp()
        {
            Game.MapHeight = 500;
            Game.MapWidth = 500;
            Game.Levels[0] = new Level(new List<IDrawable>(), null, new Point(70, 70), 1);
        }

        public void DeleteCheck(Point bulletInitialPos, Size offset, IDrawable addObj,
            OwnerType owner, int expectedCount = 0)
        {
            var bullet = new Bullet(bulletInitialPos, 0, owner);
            Game.Levels[0].Objects.Add(addObj);
            Game.Levels[0].Bullets.Add(bullet);
            bullet.Move(offset);
            Game.DeleteObjects();

            Assert.AreEqual(expectedCount, Game.Levels[0].Bullets.Count);
        }

        public void CheckDamage(Point bulletInitialPos, IDrawable addObj, OwnerType owner,
            int objToDeleteCount = 2, int expectedHP = 40, int initialHP = 50)
        {
            var bullet = new Bullet(bulletInitialPos, 0, owner);
            Game.Levels[0].Objects.Add(addObj);
            Game.Levels[0].Bullets.Add(bullet);
            Game.Player.HeatPoints = initialHP;
            bullet.Move(new Size(0,0));

            if (addObj is Enemy)
                Assert.AreEqual(objToDeleteCount, Game.Levels[0].ObjectsToDelete.Count);
            else
                Assert.AreEqual(expectedHP, Game.Player.HeatPoints);
        }

        [Test]
        public void BulletOutBounds()
        {
            DeleteCheck(new Point(495, 10), new Size(10, 0),
                new Barrel(new Point(20, 20)), OwnerType.Player);
        }

        [Test]
        public void BulletCollisionWithBarrel()
        {
            DeleteCheck(new Point(50, 50),
                new Size(10, 0), new Barrel(new Point(85, 50)), OwnerType.Player);
        }

        [Test]
        public void BulletCollisionWithEnemy()
        {
            DeleteCheck(new Point(50, 50),
                new Size(10, 0), new Enemy(new Point(85, 50), 0), OwnerType.Player);
        }

        [Test]
        public void BulletCollisionWithPlayer()
        {
            DeleteCheck(new Point(50, 50),
                new Size(10, 0), new Enemy(new Point(95, 50), 0), OwnerType.Enemy);
        }

        [Test]
        public void BulletCollisionWithPlayerByPlayer()
        {
            DeleteCheck(new Point(50, 50),
                new Size(10, 0), new Player(new Point(95, 50), 0), OwnerType.Player, 1);
        }

        [Test]
        public void BulletCollisionWithEnemyByEnemy()
        {
            DeleteCheck(new Point(200, 200),
                new Size(10, 0), new Enemy(new Point(235, 200), 0), OwnerType.Enemy, 1);
        }

        [Test]
        public void DealDamageToEnemy()
        {
            CheckDamage(new Point(150,150), new Enemy(new Point(150,150),0), OwnerType.Player);
        }

        [Test]
        public void DealDamageToPlayer()
        {
            CheckDamage(new Point(70, 70), new Table(new Point(150,150)), OwnerType.Enemy);
        }

        [Test]
        public void DealDamageToPlayerWithLowHP()
        {
            CheckDamage(new Point(70, 70), new Table(new Point(150, 150)), OwnerType.Enemy, expectedHP:50, initialHP:5);
        }
    }
}
