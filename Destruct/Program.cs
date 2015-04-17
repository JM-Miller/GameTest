using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Destruct
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Globals.speed = 1;
            Globals.scale = 2;
            Globals.defaultTileSize = 8;
            Globals.screenSize = 640;
            Globals.screenRectangle = new Rectangle(0, 0, Globals.screenSize, Globals.screenSize);
            Brushes.InitBrushes();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}
