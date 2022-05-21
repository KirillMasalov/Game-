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
    public class Game_Should
    {
        [SetUp]
        public void SetUp()
        {
            Game.MapHeight = 500;
            Game.MapWidth = 500;
            Game.Levels[0] = new Level(new List<IDrawable>(), null, new Point(70, 70), 1);
        }

        public void CheckKeysConverts(HashSet<Keys> pressedkeys, Size expectedOffsett)
        {
            Game.PressedKeys = pressedkeys;
            var offset = Game.ConvertKeysToOffset();
            Assert.AreEqual(expectedOffsett, offset);
        }

        public void CheckBoundsCorrection(Size offset, Point expextedPosition)
        {
            Game.Player.Position = new Point(Game.Player.Radius - 20, Game.Player.Radius - 20);
            Game.Player.Position += Game.CorrectOffsetInBounds(Game.Player, offset);
            Assert.AreEqual(expextedPosition, Game.Player.Position);
        }

        public void CheckObjectsCreation(List<IDrawable> objects)
        {
            var objectsCount = objects.Count;
            Game.Levels[0].ObjectsToCreate = objects;
            Game.CreateObjects();
            Assert.AreEqual(objectsCount, Game.Levels[0].Objects.Count);
            Assert.IsEmpty(Game.Levels[0].ObjectsToCreate);
        }

        public void CheckObjectsDelete(List<IDrawable> objects, List<IDrawable> additionalObjects)
        {
            Game.Levels[0].ObjectsToDelete = objects;

            foreach (var obj in objects)
                Game.Levels[0].Objects.Add(obj);
            foreach (var obj in additionalObjects)
                Game.Levels[0].Objects.Add(obj);

            var initialObjectsCount = Game.Levels[0].Objects.Count;
            var deleteObjectsCount = objects.Count;

            Game.DeleteObjects();

            Assert.AreEqual(initialObjectsCount - deleteObjectsCount, Game.Levels[0].Objects.Count);
            Assert.IsEmpty(Game.Levels[0].ObjectsToDelete);
        }

        public void CheckScores(Point bulletInitialPos, bool withShoot, int expectedScores, int initialScores = 10)
        {
            Game.Scores = initialScores;
            if (withShoot)
                Game.Player.Shoot();
            var bullet = new Bullet(bulletInitialPos, 0, OwnerType.Player);
            Game.Levels[0].Bullets.Add(bullet);
            Game.Levels[0].Objects.Add(new Enemy(new Point(150,150), 0));
            bullet.Move(new Size(0,0));
            Game.DeleteObjects();
            Assert.AreEqual(expectedScores, Game.Scores);
        }

        [Test]
        public void EmptyPressedKeys() { CheckKeysConverts(new HashSet<Keys>(), new Size(0, 0)); }

        [Test]
        public void OnePressedKeys() { CheckKeysConverts(new HashSet<Keys>() { Keys.A }, new Size(-Game.Player.Speed, 0)); }

        [Test]
        public void SomePressedKeys()
        {
            CheckKeysConverts(new HashSet<Keys>() { Keys.A,
            Keys.S }, new Size(-Game.Player.Speed, Game.Player.Speed));
        }

        [Test]
        public void OpponentPressedKeys() { CheckKeysConverts(new HashSet<Keys>() { Keys.A, Keys.D }, new Size(0, 0)); }

        [Test]
        public void BoundsCorrectionByX()
        {
            CheckBoundsCorrection(new Size(-10, 0),
                new Point(Game.Player.Radius - 20, Game.Player.Radius - 20));
        }

        [Test]
        public void BoundsCorrectionByXFree()
        {
            CheckBoundsCorrection(new Size(10, 0),
                new Point(Game.Player.Radius - 10, Game.Player.Radius - 20));
        }

        [Test]
        public void BoundsCorrectionByY()
        {
            CheckBoundsCorrection(new Size(0, -10),
                new Point(Game.Player.Radius - 20, Game.Player.Radius - 20));
        }

        [Test]
        public void BoundsCorrectionByYFree()
        {
            CheckBoundsCorrection(new Size(0, 10),
                new Point(Game.Player.Radius - 20, Game.Player.Radius - 10));
        }

        [Test]
        public void EmptyCreationList() { CheckObjectsCreation(new List<IDrawable>()); }

        [Test]
        public void CreateObjects() { CheckObjectsCreation(new List<IDrawable>() { new Table(new Point(100, 100)) }); }

        [Test]
        public void EmptyDeleteList() { CheckObjectsDelete(new List<IDrawable>(), new List<IDrawable>()); }

        [Test]
        public void DeleteAllObjects()
        {
            CheckObjectsDelete(new List<IDrawable>() { new Table(new Point(50, 50)) },
                new List<IDrawable>());
        }

        [Test]
        public void DeleteSomeObjects()
        {
            CheckObjectsDelete(new List<IDrawable>() { new Table(new Point(50, 50)) },
                new List<IDrawable>() { new Table(new Point(200, 200)) });
        }

        [Test]
        public void ScoresByEnemy() { CheckScores(new Point(150, 150), false, 20); }

        [Test]
        public void ScoresByShoot() { CheckScores(new Point(100, 100), true, 9); }

        [Test]
        public void ScoresByShootWithZero() { CheckScores(new Point(100, 100), true, 0,0); }
    }
}
