using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace Game_
{
    public static class Game
    {
        public static int MapWidth = 1200;
        public static int MapHeight = 580;
        public static int InterfaceHeight = 100;

        public static Level[] Levels = new Level[5];
        public static int CurrentLevelNum = 0;

        public static Player Player = new Player(70, 70);
        public static int Scores { get; set; }
        public static bool IsOver { get; set; }

        public static HashSet<Keys> PressedKeys = new HashSet<Keys>();

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

        public static Size CorrectOffsetInBounds(Player player, Size offset)
        {
            if (player.Position.X + offset.Width > MapWidth + 20 - player.Radius)
                offset.Width -= (int)(player.Position.X + player.Radius + offset.Width - MapWidth - 20);
            if (player.Position.X + offset.Width < player.Radius - 20)
                offset.Width += (int)(player.Position.X - player.Radius - offset.Width + 20);
            if (player.Position.Y + offset.Height > MapHeight + 20 - player.Radius)
                offset.Height -= (int)(player.Position.Y + player.Radius + offset.Height - MapHeight - 20);
            if (player.Position.Y + offset.Height < player.Radius - 20)
                offset.Height += (int)(player.Position.Y - player.Radius - offset.Height + 20);
            return offset;
        }

        public static void MoveObjects()
        {
            foreach (var obj in Levels[CurrentLevelNum].Objects)
            {
                var offset = new Size((int)(Math.Cos(obj.Direction) * obj.Speed), (int)(Math.Sin(obj.Direction) * obj.Speed));
                obj.Position += offset;
            }
        }
    }
}
