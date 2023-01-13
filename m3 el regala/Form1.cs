namespace m3_el_regala
{
    public class Hero
    {
        public Bitmap Img;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Hero(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
    public partial class Form1 : Form
    {
        Bitmap offImage;
        Hero Player = new Hero(0, 0, 50, 50);
        Hero Coin = new Hero(0, 0, 50, 50);  
        bool Collected = false;
        int Score = 0;
        System.Windows.Forms.Timer T = new System.Windows.Forms.Timer();
        Random R = new Random();
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            this.Paint += Form1_Paint;
            this.KeyDown += Form1_KeyDown;
            T.Tick += T_Tick;
            T.Start();
        }

        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Right && Player.X + Player.Width + 10 < this.ClientSize.Width)
            {
                Player.X += 10;
            }
            else if(e.KeyCode == Keys.Left && Player.X - 10 > 0)
            {
                Player.X -= 10;
            }
            else if(e.KeyCode == Keys.Up && Player.Y - 10 > 0)
            {
                Player.Y -= 10;
            }
            else if (e.KeyCode == Keys.Down && Player.Y + Player.Height + 10 < this.ClientSize.Height)
            {
                Player.Y += 10;
            }
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            DoubleBuffer(e.Graphics);
        }

        private void T_Tick(object? sender, EventArgs e)
        {
            CheckCoin();
            CoinGravity();
            DoubleBuffer(this.CreateGraphics());
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            offImage = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            Coin.Img = new Bitmap("Coin/1.png");
            Coin.Img = new Bitmap(Coin.Img, new Size(50, 50));
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
            g.FillRectangle(Brushes.Black,Player.X, Player.Y, Player.Width, Player.Height);
            g.DrawImage(Coin.Img, Coin.X, Coin.Y);
            g.DrawString("Score: "+Score.ToString(), new Font("Arial", 20), Brushes.Black, 0, 0);
        }
        void CheckCoin()
        {
            if(new Rectangle(Coin.X,Coin.Y,Coin.Img.Width,Coin.Img.Height).IntersectsWith(new Rectangle(Player.X,Player.Y,Player.Width,Player.Height)))
            {
                Score++;
                Coin.X = R.Next(0, this.ClientSize.Width - Coin.Img.Width);
                Coin.Y = 0;
            }
        }
        void CoinGravity()
        {
            if (Coin.Y < this.ClientSize.Height - Coin.Img.Height)
            {
                Coin.Y += 5;
            }
        }
    }
}