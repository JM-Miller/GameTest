using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

public static class Brushes
{

    public static void InitBrushes()
    {
        Brushes.brushOrange = new SolidBrush(Color.Orange);
        Brushes.brushGreen = new SolidBrush(Color.Green);
        Brushes.brushBrown = new SolidBrush(Color.Brown);
        Brushes.brushSlateGray = new SolidBrush(Color.SlateGray);
        Brushes.brushTransDarkGreen = new SolidBrush(Color.FromArgb(200, Color.DarkGreen));
        Brushes.brushBlack = new SolidBrush(Color.Black);
    }

    public static SolidBrush brushBlack { get; set; }
    public static SolidBrush brushOrange { get; set; }
    public static SolidBrush brushGreen { get; set; }
    public static SolidBrush brushBrown { get; set; }
    public static SolidBrush brushSlateGray { get; set; }
    public static SolidBrush brushTransDarkGreen { get; set; }
}
public static class Globals
{
    public static int halfScreenSize { get { return screenSize / 2; } }
    public static int defaultTileSize { get; set; }
    public static int screenSize { get; set; }
    public static int scale { get; set; }
    public static int mouseX { get; set; }
    public static int mouseY { get; set; }
    public static int speed { get; set; }
    public static int playerId { get; set; }
    public static Rectangle screenRectangle { get; set; }
}
namespace Destruct
{
    public partial class GameForm : Form
    {
        public bool isDone;
        SolidBrush brush;
        Graphics g;
        Bitmap buffer;
        System.Windows.Forms.Timer step;
        GameStates.GameStateManager gsm;
        public GameForm()
        {
            InitializeComponent();
            isDone = false;
            g = this.CreateGraphics();
            brush = new SolidBrush(Color.Blue);
            step = new System.Windows.Forms.Timer();
            step.Interval = 1;
            step.Tick += step_Tick;
            step.Start();
            buffer = new Bitmap(this.Width, this.Height);
            g = Graphics.FromImage(buffer);
            DoubleBuffered = true;
            gsm = new GameStates.GameStateManager();
            gsm.Init(this);
        }

        void step_Tick(object sender, EventArgs e)
        {
            update();
            this.Invalidate();
        }

        public void update()
        {
            Globals.mouseX = this.PointToClient(Control.MousePosition).X;
            Globals.mouseY = this.PointToClient(Control.MousePosition).Y;
            gsm.Update();
        }
        public void draw()
        {
            //g.FillRectangle(brush, 0, 0, this.Width, this.Height);
            gsm.Draw(g);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            draw();
            e.Graphics.DrawImage(buffer, 0, 0);
        }
    }
}
