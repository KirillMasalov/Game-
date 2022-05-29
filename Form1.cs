using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Game_
{
    public partial class Form1 : Form
    {
        private int time;
        private Timer timer;
        private Player player = Game.Player;
        private static Dictionary<string, Image> Images;
        private static Dictionary<string, string> PathsToSound;

        private Label livesLabel;
        private Label scoresLabel;
        private Label pauseLabel;
        private Label loseLabel;
        private Label winLabel;
        private Button exitButton;
        private Button continueButton;
        private Button resetButton;
        private Button resetBossButton;

        private NAudio.Wave.WaveOutEvent BackGroundMusicPlayer = new NAudio.Wave.WaveOutEvent();

        public Form1()
        {
            InitializeComponent();
            FillImagesDictionary();
            FillSoundsDictionary();

            timer = new Timer();
            timer.Interval = 25;
            timer.Tick += TimerTick;

            BackGroundMusicPlayer.Init(new NAudio.Wave.WaveFileReader(PathsToSound["BackGroundMusic"]));
            BackGroundMusicPlayer.Play();

            timer.Start();
        }

        private void FillImagesDictionary()
        {
            var imagesDirectory = new DirectoryInfo("Images");

            if (Images == null)
                Images = new Dictionary<string, Image>();
            Images["Player"] = Image.FromFile(imagesDirectory.GetFiles("*Player.png").First().FullName);
            Images["TutorialBack"] = Image.FromFile(imagesDirectory.GetFiles("*Back.jpg").First().FullName);
            Images["BackGround"] = Image.FromFile(imagesDirectory.GetFiles("*BackGround3.jpg").First().FullName);
            Images["BackGround2"] = Image.FromFile(imagesDirectory.GetFiles("*BackGround2.jpg").First().FullName);
            Images["Enemy"] = Image.FromFile(imagesDirectory.GetFiles("*Enemy.png").First().FullName);
            Images["Table"] = Image.FromFile(imagesDirectory.GetFiles("*Table.png").First().FullName);
            Images["EnemyDead"] = Image.FromFile(imagesDirectory.GetFiles("*EnemyDead.png").First().FullName);
            Images["BossDead"] = Image.FromFile(imagesDirectory.GetFiles("*BossDead.png").First().FullName);
            Images["Barrel"] = Image.FromFile(imagesDirectory.GetFiles("*Barrel.png").First().FullName);
            Images["Heal"] = Image.FromFile(imagesDirectory.GetFiles("*Heal.png").First().FullName);
            Images["Rapid"] = Image.FromFile(imagesDirectory.GetFiles("*Rapid.png").First().FullName);
            Images["NextLevel"] = Image.FromFile(imagesDirectory.GetFiles("*Next.png").First().FullName);
            Images["Boss"] = Image.FromFile(imagesDirectory.GetFiles("*Boss.png").First().FullName);
        }

        private void FillSoundsDictionary()
        {
            var soundsDirectory = new DirectoryInfo("Sounds");

            if (PathsToSound == null)
                PathsToSound = new Dictionary<string, string>();

            PathsToSound["BackGroundMusic"] = soundsDirectory.GetFiles("*BackGroundMusic.wav").First().FullName;
            PathsToSound["Shoot"] = soundsDirectory.GetFiles("*S.wav").First().FullName;
            PathsToSound["Heal"] = soundsDirectory.GetFiles("*Heal.wav").First().FullName;
            PathsToSound["Rapid"] = soundsDirectory.GetFiles("*Rapid.wav").First().FullName;
            PathsToSound["Damage"] = soundsDirectory.GetFiles("*Damage.wav").First().FullName;
            PathsToSound["Death"] = soundsDirectory.GetFiles("*Death.wav").First().FullName;
            PathsToSound["BarrelBreak"] = soundsDirectory.GetFiles("*BarrelBreak.wav").First().FullName;
            PathsToSound["Victory"] = soundsDirectory.GetFiles("*Victory.wav").First().FullName;
            PathsToSound["SDamage"] = soundsDirectory.GetFiles("*SDamage.wav").First().FullName;
            PathsToSound["EnemyDead"] = soundsDirectory.GetFiles("*EnemyDead.wav").First().FullName;
        }
        private void SetControls()
        {
            livesLabel = new Label();
            livesLabel.Location = new Point(50, Game.MapHeight + 35);
            livesLabel.AutoSize = true;
            livesLabel.Font = new Font("Consuela", 18, FontStyle.Regular);
            livesLabel.BackColor = Color.Black;
            livesLabel.ForeColor = Color.White;
            Controls.Add(livesLabel);

            var heatPointsLabel = new Label();
            heatPointsLabel.Location = new Point(450, Game.MapHeight + 35);
            heatPointsLabel.AutoSize = true;
            heatPointsLabel.Text = "Здоровье";
            heatPointsLabel.Font = new Font("Consuela", 18, FontStyle.Regular);
            heatPointsLabel.BackColor = Color.Black;
            heatPointsLabel.ForeColor = Color.White;
            Controls.Add(heatPointsLabel);

            scoresLabel = new Label();
            scoresLabel.Location = new Point(1000, Game.MapHeight + 35);
            scoresLabel.AutoSize = true;
            scoresLabel.Font = new Font("Consuela", 18, FontStyle.Regular);
            scoresLabel.BackColor = Color.Black;
            scoresLabel.ForeColor = Color.White;
            Controls.Add(scoresLabel);

            pauseLabel = new Label();
            pauseLabel.Location = new Point(4 * Game.MapWidth / 9, 7 * Game.MapHeight / 18);
            pauseLabel.AutoSize = true;
            pauseLabel.Font = new Font("Consuela", 30, FontStyle.Regular);
            pauseLabel.Text = "Пауза";
            pauseLabel.BackColor = Color.Peru;

            loseLabel = new Label();
            loseLabel.Location = new Point(14 * Game.MapWidth / 36, 7 * Game.MapHeight / 18);
            loseLabel.AutoSize = true;
            loseLabel.Font = new Font("Consuela", 27, FontStyle.Regular);
            loseLabel.Text = "Вы проиграли";
            loseLabel.BackColor = Color.Peru;

            winLabel = new Label();
            winLabel.Location = new Point(14 * Game.MapWidth / 36, 7 * Game.MapHeight / 18);
            winLabel.AutoSize = true;
            winLabel.Font = new Font("Consuela", 27, FontStyle.Regular);
            winLabel.Text = "Игра пройдена";
            winLabel.BackColor = Color.Peru;

            exitButton = new Button();
            exitButton.Text = "Выйти";
            exitButton.Font = new Font("Consuela", 15, FontStyle.Regular);
            exitButton.BackColor = Color.Chocolate;
            exitButton.Click += (sender, eventArgs) =>
            {
                BackGroundMusicPlayer.Stop();
                BackGroundMusicPlayer.Dispose();
                Application.Exit();
            };

            continueButton = new Button();
            continueButton.Text = "Продолжить";
            continueButton.Font = new Font("Consuela", 15, FontStyle.Regular);
            continueButton.BackColor = Color.Chocolate;
            continueButton.Click += (sender, eventArgs) => OnKeyDown(new KeyEventArgs(Keys.Escape));

            resetButton = new Button();
            resetButton.Text = "Начать сначала";
            resetButton.Font = new Font("Consuela", 15, FontStyle.Regular);
            resetButton.BackColor = Color.Chocolate;
            resetButton.Click += (sender, eventArgs) =>
            {
                ResetFormView();
                BackgroundImage = Images["TutorialBack"];
                BackGroundMusicPlayer.Stop();
                BackGroundMusicPlayer.Init(new NAudio.Wave.WaveFileReader(PathsToSound["BackGroundMusic"]));
                BackGroundMusicPlayer.Play();
                timer.Start();
            };

            resetBossButton = new Button();
            resetBossButton.Text = "Начать перед боссом";
            resetBossButton.Font = new Font("Consuela", 15, FontStyle.Regular);
            resetBossButton.BackColor = Color.Chocolate;
            resetBossButton.Click += (sender, eventArgs) =>
            {
                ResetFormView();
                BackgroundImage = Images["BackGround2"];
                Game.CurrentLevelNum = 7;
                player.Position = new Point(100, 300);
                timer.Start();
            };
        }

        private void ResetFormView()
        {
            Game.CreateLevels(Images);
            Game.ResetGame();
            player = Game.Player;
            Controls.Remove(exitButton);
            Controls.Remove(loseLabel);
            Controls.Remove(winLabel);
            Controls.Remove(resetBossButton);
            Controls.Remove(resetButton);
        }

        private void SetButtonsParameters(Button button, Point location, Size size)
        {
            button.Location = location;
            button.Size = size;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            WindowState = FormWindowState.Maximized;
            Text = "Wild Wild West";
            Game.MapHeight = ClientSize.Height - 100;
            Game.MapWidth = ClientSize.Width;
            SetControls();
            Game.CreateLevels(Images);
            BackgroundImage = Game.Levels[Game.CurrentLevelNum].BackGround;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            livesLabel.Text = "Жизни: " + player.Lives;
            scoresLabel.Text = "Очки: " + Game.Scores;

            if (BackGroundMusicPlayer.PlaybackState != NAudio.Wave.PlaybackState.Playing)
            {
                BackGroundMusicPlayer.Init(new NAudio.Wave.WaveFileReader(PathsToSound["BackGroundMusic"]));
                BackGroundMusicPlayer.Play();
            }

            if (Game.IsOver)
            {
                timer.Stop();
                if (player.Lives <= 0)
                {
                    if (Game.CurrentLevelNum == Game.Levels.Length - 1)
                        CreateGameOverBossMenu();
                    else
                        CreateGameOverMenu();
                }
                else
                {
                    CreateWinMenu();
                    BackGroundMusicPlayer.Stop();
                    BackGroundMusicPlayer.Init(new NAudio.Wave.WaveFileReader(PathsToSound["Victory"]));
                    BackGroundMusicPlayer.Play();
                }
                Invalidate();
            }

            time++;

            if (time >= 40)
            {
                time = 0;
                Game.EnemiesShoots();
            }

            if (Game.CurrentLevelNum == Game.Levels.Length - 1 && time >= 15)
            {
                time = 0;
                Game.BossShoot();
            }


            player.Move(Game.ConvertKeysToOffset());
            Game.MoveObjects();
            Game.DeleteObjects();
            Game.CreateObjects();
            PlaySounds();

            if (Game.Levels[Game.CurrentLevelNum].EnemiesCount <= 0
                && player.Position.X > Game.MapWidth - 30)
            {
                Game.ToNextLevel();
                BackgroundImage = Game.Levels[Game.CurrentLevelNum].BackGround;
            }
            Invalidate();
        }

        private void PlaySounds()
        {
            foreach (var sd in Game.Sounds)
            {
                var otherSoundsPlayer = new NAudio.Wave.WaveOutEvent();
                NAudio.Wave.WaveFileReader sound = new NAudio.Wave.WaveFileReader(PathsToSound[sd]);
                otherSoundsPlayer.Init(sound);
                otherSoundsPlayer.Play();
            }
            Game.Sounds.Clear();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D
                || e.KeyCode == Keys.W)
                Game.PressedKeys.Add(e.KeyCode);
            if (e.KeyCode == Keys.Escape)
            {
                Game.IsPaused = !Game.IsPaused;
                if (Game.IsPaused)
                {
                    timer.Stop();
                    CreatePauseMenu();
                    Invalidate();
                }
                else
                {
                    timer.Start();
                    Controls.Remove(exitButton);
                    Controls.Remove(pauseLabel);
                    Controls.Remove(continueButton);
                }
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D
                || e.KeyCode == Keys.W)
                Game.PressedKeys.Remove(e.KeyCode);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Pens.Black.Brush, 0, Game.MapHeight, Game.MapWidth, Game.InterfaceHeight);
            e.Graphics.FillRectangle((player.HeatPoints <= 15)
                ? Pens.Red.Brush : Pens.Green.Brush, 580, Game.MapHeight + 35, player.HeatPoints * 4, 30);

            foreach (var obj in Game.Levels[Game.CurrentLevelNum].Objects)
            {
                e.Graphics.DrawImage(DrawRotatedImage(obj, Images[obj.GetImage()], 0),
                        obj.Position.X - obj.Radius, obj.Position.Y - obj.Radius,
                        obj.Radius * 2f, obj.Radius * 2f);
            }

            foreach (var bullet in Game.Levels[Game.CurrentLevelNum].Bullets)
            {
                e.Graphics.FillEllipse((bullet.Owner == OwnerType.Player)
                    ? Pens.Orange.Brush : Pens.Red.Brush, bullet.Position.X - bullet.Radius / 2,
                    bullet.Position.Y - bullet.Radius / 2, bullet.Radius, bullet.Radius);
            }

            if (Game.Levels[Game.CurrentLevelNum].EnemiesCount <= 0)
                e.Graphics.DrawImage(Images["NextLevel"],
                Game.MapWidth - 180, (Game.MapHeight - 150) / 2, 150, 150);

            e.Graphics.DrawImage(DrawRotatedImage(player, Images[player.GetImage()], 180f),
                player.Position.X - player.Radius, player.Position.Y - player.Radius,
                player.Radius * 2f, player.Radius * 2f);

            if (Game.CurrentLevelNum == Game.Levels.Length - 1)
            {
                var boss = Game.Levels[Game.CurrentLevelNum].Objects.Where(obj => obj is Boss).FirstOrDefault() as Boss;
                if (boss != null)
                {
                    e.Graphics.FillRectangle(Pens.Black.Brush, 250 - 2, 30 - 2, 804, 14);
                    e.Graphics.FillRectangle(Pens.Red.Brush, 250, 30, 4 * boss.HeatPoints, 10);
                }
            }

            if (Game.IsPaused || Game.IsOver)
                DrawInfoPanel(e);
        }

        private Image DrawRotatedImage(IDrawable obj, Image img, float additionalAngle)
        {
            var bmp = new Bitmap(img.Width, img.Height);
            var gfx = Graphics.FromImage(bmp);
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
            gfx.RotateTransform(obj.Direction * 57f - additionalAngle);
            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            gfx.DrawImage(img, new Point(0, 0));
            gfx.Dispose();
            return bmp;
        }

        private void DrawInfoPanel(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Pens.Black.Brush, Game.MapWidth / 3 - 2,
               Game.MapHeight / 3 - 2, Game.MapWidth / 3 + 4, Game.MapHeight / 3 + 4);
            e.Graphics.FillRectangle(Pens.Peru.Brush, Game.MapWidth / 3,
                Game.MapHeight / 3, Game.MapWidth / 3, Game.MapHeight / 3);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            player.Direction = (float)Math.Atan2(e.Y - player.Position.Y, e.X - player.Position.X);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            player.Direction = (float)Math.Atan2(e.Y - player.Position.Y, e.X - player.Position.X);
            player.Shoot();
        }

        private void CreatePauseMenu()
        {
            SetButtonsParameters(exitButton, new Point(11 * Game.MapWidth / 30, Game.MapHeight / 2),
                  new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            SetButtonsParameters(continueButton, new Point(15 * Game.MapWidth / 30, Game.MapHeight / 2),
                new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            Controls.Add(exitButton);
            Controls.Add(pauseLabel);
            Controls.Add(continueButton);
        }

        private void CreateWinMenu()
        {
            SetButtonsParameters(exitButton, new Point(11 * Game.MapWidth / 30, Game.MapHeight / 2),
               new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            SetButtonsParameters(resetButton, new Point(15 * Game.MapWidth / 30, Game.MapHeight / 2),
                new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            Controls.Add(winLabel);
            Controls.Add(exitButton);
            Controls.Add(resetButton);
        }

        private void CreateGameOverMenu()
        {
            SetButtonsParameters(exitButton, new Point(11 * Game.MapWidth / 30, Game.MapHeight / 2),
                new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            SetButtonsParameters(resetButton, new Point(15 * Game.MapWidth / 30, Game.MapHeight / 2),
                new Size(Game.MapWidth / 8, Game.MapHeight / 8));
            Controls.Add(loseLabel);
            Controls.Add(exitButton);
            Controls.Add(resetButton);
        }

        private void CreateGameOverBossMenu()
        {
            SetButtonsParameters(exitButton, new Point(60 * Game.MapWidth / 180, Game.MapHeight / 2),
                   new Size(11 * Game.MapWidth / 100, Game.MapHeight / 8 + 10));
            SetButtonsParameters(resetButton, new Point(80 * Game.MapWidth / 180, Game.MapHeight / 2),
               new Size(11 * Game.MapWidth / 100, Game.MapHeight / 8 + 10));
            SetButtonsParameters(resetBossButton, new Point(100 * Game.MapWidth / 180, Game.MapHeight / 2),
               new Size(11 * Game.MapWidth / 100, Game.MapHeight / 8 + 10));
            Controls.Add(loseLabel);
            Controls.Add(exitButton);
            Controls.Add(resetButton);
            Controls.Add(resetBossButton);
        }
    }
}
