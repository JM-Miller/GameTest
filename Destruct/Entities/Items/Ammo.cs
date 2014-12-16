using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    public class Ammo : Item
    {
        Player p;
        public Ammo(int x, int y, TileMaps.TileLayer l, Player p)
        {
            this.xPos = x;
            this.yPos = y;
            this.w = 2 * Globals.scale;
            this.h = 2 * Globals.scale;
            this.xOff = l.xMapOffset;
            this.yOff = l.yMapOffset;
            this.p = p;
        }
        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void Update(Player p)
        {
            if (remove)
                return;
            this.x = p.map.xOffset + (xPos * (Globals.defaultTileSize * Globals.scale)) + (xOff * (Globals.defaultTileSize * Globals.scale)) + (w * Globals.scale / 2);
            this.y = p.map.yOffset + (yPos * (Globals.defaultTileSize * Globals.scale)) + (yOff * (Globals.defaultTileSize * Globals.scale)) + (h * Globals.scale / 2);
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
            if (p.guns.Count > 0 && screenRect.IntersectsWith(rect) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                remove = true;
                PlayerTools.Gun g = p.guns[new Random().Next(0, p.guns.Count)];
                g.totalAmmo += new Random().Next(1, g.ammoSize);
            }
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            if (p.guns.Count > 0 )
                g.FillRectangle(Brushes.brushBlack, rect);
        }

        public override void DrawText(System.Drawing.Graphics g)
        {
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
            if (p.guns.Count <= 0)
                return;
            if (screenRect.IntersectsWith(rect))
            {
                g.DrawString("Press SPACE to pick up\n" + "Ammo", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
