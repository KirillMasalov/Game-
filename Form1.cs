using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_
{
    public partial class Form1 : Form
    {
        private int time;
        private Player player = Game.Player;
        private Dictionary<Type, Image> Images;
        private Dictionary<Type, Polygon[]> Details;
        public Form1()
        {
            InitializeComponent();
            var timer = new Timer();
            timer.Interval = 20;
            timer.Tick += TimerTick;

            FillImagesDictionary();
            FillDetailsDictionary();

            var back = Image.FromFile(@"C:\Users\User\Desktop\Игра Прога\Images\BackGround1.png");
            Game.Levels[0] = new Level(new List<IDrawable>(), back, new Point(10,10));

            timer.Start();
        }

        private void FillImagesDictionary()
        {
            if (Images == null)
                Images = new Dictionary<Type, Image>();
            Images[typeof(Player)] = Image.FromFile(@"..\Images\Player.png");
        }
        private void FillDetailsDictionary()
        {
            if (Details == null)
                Details = new Dictionary<Type, Polygon[]>();
            Details[typeof(Player)] = new Polygon[] { new Polygon(170, 300, 200, 300, 220, 450, 190, 450) };

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            ClientSize = new Size(Game.MapWidth, Game.MapHeight + Game.InterfaceHeight);
        }

        private void TimerTick(object sender, EventArgs e)
        {
            time++;
            player.Move(Game.ConvertKeysToOffset());
            Game.MoveObjects();
            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D
                || e.KeyCode == Keys.W)
                Game.PressedKeys.Add(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D
                || e.KeyCode == Keys.W)
                Game.PressedKeys.Remove(e.KeyCode);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Pens.Peru.Brush, 0, 0, Game.MapWidth, Game.MapHeight);
            foreach (var obj in Game.Levels[Game.CurrentLevelNum].Objects)
            {
                e.Graphics.FillRectangle(Pens.Red.Brush, obj.Position.X, obj.Position.Y, 10, 10);
            }
            e.Graphics.DrawImage(RotatedImage(Images[typeof(Player)], 83f, Details[typeof(Player)]),
                player.Position.X - player.Radius, player.Position.Y - player.Radius,
                player.Radius * 2f, player.Radius * 2f);
        }

        private Image RotatedImage(Image img, float angle, Polygon[] additionalDetatils)
        {
            var bmp = new Bitmap(img.Width, img.Height);
            var gfx = Graphics.FromImage(bmp);
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(player.Direction * 57f - angle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
            foreach (var detail in additionalDetatils)
                gfx.FillPolygon(Pens.Black.Brush, detail.GetPoints());
            gfx.DrawImage(img, new Point(0, 0));
            gfx.Dispose();
            return bmp;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            player.Direction = (float)Math.Atan2(e.Y - player.Position.Y, e.X - player.Position.X);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Game.Levels[Game.CurrentLevelNum].Objects.Add(new Bullet(player.Position, player.Direction));
        }
    }
}
