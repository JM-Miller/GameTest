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
        public BuildingSupplies(int x, int y, TileMaps.TileLayer l, Player p)
        {
            this.tileX = x;
            this.tileY = y;
            this.size = 2 * Globals.scale;
            this.size = 2 * Globals.scale;
            this.layerTileX = l.xMapOffset;
            this.layerTileY = l.yMapOffset;
            this.p = p;
        }
        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            if (ShouldRemove)
                return;
            this.screenX = p.map.xOffset + (tileX * (Globals.defaultTileSize * Globals.scale)) + (layerTileX * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            this.screenY = p.map.yOffset + (tileY * (Globals.defaultTileSize * Globals.scale)) + (layerTileY * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, screenRectangle.Width, screenRectangle.Height);
            if (screenRect.IntersectsWith(screenRectangle) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                ShouldRemove = true;
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
            g.FillRectangle(Brushes.brushOrange, screenRectangle);
        }

        public override void DrawOverLayer(System.Drawing.Graphics g)
        {
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, screenRectangle.Width, screenRectangle.Height);
            if (screenRect.IntersectsWith(screenRectangle))
            {
                g.DrawString("Press SPACE to pick up\n" + "Building Supplies", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
