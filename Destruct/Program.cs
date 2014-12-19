using System;
using System.Collections.Generic;
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
            Globals.renderArea = 640 * 3;
            Brushes.InitBrushes();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}
