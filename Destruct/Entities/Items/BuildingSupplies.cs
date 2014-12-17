using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    public class BuildingSupplies : Item
    {
        Player p;
        public BuildingSupplies()
        {

        }
        public BuildingSupplies(int x, int y, TileMaps.TileLayer l, Player p)
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
            if (screenRect.IntersectsWith(rect) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                remove = true;
                if (p.builder.numOfBlocks.Count < 1)
                {
                    p.builder.type = TileMaps.TileType.WoodWall;
                    p.builder.numOfBlocks.Add(TileMaps.TileType.WoodWall, 0);
                } 
                p.builder.numOfBlocks[TileMaps.TileType.WoodWall] += 2;
            }
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillRectangle(Brushes.brushOrange, rect);
        }

        public override void DrawText(System.Drawing.Graphics g)
        {
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
            if (screenRect.IntersectsWith(rect))
            {
                g.DrawString("Press SPACE to pick up\n" + "Building Supplies", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
