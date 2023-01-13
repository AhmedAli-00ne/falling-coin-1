namespace m3_el_regala
{
    public class Hero
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public Hero(Bitmap img,int x, int y)
        {
            Img = img;
            X = x;
            Y = y;
        }
    }
    public class Coin
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public int Gravity;
        public bool MarkedForDeletion = false;
        public Coin(Bitmap img,int x, int y, int g)
        {
            Img = img;
            X = x;
            Y = y;
            Gravity = g;
        }
    }
    public partial class Form1 : Form
    {
        Bitmap offImage;
        List<Coin> Coins = new List<Coin>();
        Hero Player;
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        Random R = new Random();
        int IntervalCounter;
        int Count = 0;
        int w = 0;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            this.MouseClick += Form1_MouseClick;
            T.Tick += T_Tick;
            T.Start();
        }

        private void Form1_MouseClick(object? sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if (new Rectangle(e.X, e.Y, 1, 1).IntersectsWith(new Rectangle(Player.X, Player.Y, Player.Img.Width, Player.Img.Height)))
                {
                    w = 1;
                }
            }
            if (e.Button == MouseButtons.Left)
            {
                if (new Rectangle(e.X, e.Y, 1, 1).IntersectsWith(new Rectangle(Player.X, Player.Y, Player.Img.Width, Player.Img.Height)))
                {
                    w = 2;
                }
            }
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Right && Player.X + Player.Img.Width + 10 < this.ClientSize.Width)
            {
                Player.X += 10;
            }
            else if(e.KeyCode == Keys.Left && Player.X - 10 > 0)
            {
                Player.X -= 10;
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            DoubleBuffer(e.Graphics);
        }

        private void T_Tick(object? sender, EventArgs e)
        {
            if(w ==1)
            {
                if(Player.Y>0)
                {
                    Player.Y -= 5;
                }
                else
                {
                    w = 2;
                }
            }
            else if(w == 2)
            {
                if(Player.Y+Player.Img.Height<this.ClientSize.Height)
                {
                    Player.Y += 5;
                }
            }
            CheckForInterval();
            CoinGravity();
            CoinChecker();
            CheckForDeletion();
            Count++;
            DoubleBuffer(this.CreateGraphics());
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            offImage = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            IntervalCounter = R.Next(0, 10);
            Player = new Hero(new Bitmap("Player/PrimitivePlayerIdleRight/PlayerIdle (1).png"), 0, this.ClientSize.Height - 34);
        }
        void DoubleBuffer(Graphics g)
        {
            Graphics offGraphics = Graphics.FromImage(offImage);
            DrawScene(offGraphics);
            g.DrawImage(offImage, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            g.DrawImage(Player.Img, Player.X, Player.Y);
            for(int i=0;i<Coins.Count;i++)
            {
                g.DrawImage(Coins[i].Img, Coins[i].X, Coins[i].Y);
            }
        }
        void CheckForInterval()
        {
            if(Count == IntervalCounter)
            {
                Count = 0;
                IntervalCounter = R.Next(0, 50);
                Coins.Add(new Coin(new Bitmap("Coin/1.png"), R.Next(0, this.ClientSize.Width - 50), 0, R.Next(1, 10)));
            }
        }
        void CoinGravity()
        {
            for(int i=0;i<Coins.Count;i++)
            {
                if (Coins[i].Y < this.ClientSize.Height - Coins[i].Img.Height)
                {
                    Coins[i].Y += Coins[i].Gravity;
                }
            }
        }
        void CoinChecker()
        {
            for(int i=0;i<Coins.Count;i++)
            {
                if(new Rectangle(Player.X,Player.Y,Player.Img.Width,Player.Img.Height).IntersectsWith(new Rectangle(Coins[i].X, Coins[i].Y, Coins[i].Img.Width, Coins[i].Img.Height)))
                {
                    Coins[i].MarkedForDeletion = true;
                }
            }
        }
        void CheckForDeletion()
        {
            for(int i=0;i<Coins.Count;i++)
            {
                if (Coins[i].MarkedForDeletion)
                {
                    Coins.RemoveAt(i);
                }
            }
        }
    }
}