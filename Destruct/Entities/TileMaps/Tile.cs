using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.TileMaps
{
    public enum TileType
    {
        Blank = 0,
        Grass = 1,
        Door = 3,
        DefaultWall = 2,
        Tree = 6,
        TreeLeaves = 7,
        WoodWall = 4,
        StoneWall = 5,
        StoneRoof = 8,
        WoodRoof = 9,
        Item = 10,
    }

    public class Tile
    {
        public int screenSize;
        public int screenX;
        public int screenY;
        public int layerX;
        public int layerY;
        public int hits;
        public int maxHits;
        public int xOff;
        public int yOff;
        public int size;
        public SolidBrush brush;
        public Color color;
        public bool visible;
        public bool solid;
        public bool shouldHide;
        public bool roof;
        public TileLayer layer;
        public int opac;
        public List<Block> blocks;
        
        public Tile()
        {

        }
        public Tile(int x, int y, int s, SolidBrush b, bool vis, bool sol, int hits, TileLayer l, bool isRoof = false)
        {
            this.layerX = x;
            this.layerY = y;
            this.xOff = l.xMapOffset;
            this.yOff = l.yMapOffset;
            this.size = s;
            this.screenSize = s * Globals.scale;
            this.visible = vis;
            this.solid = sol;
            this.layer = l;
            this.hits = hits;
            this.maxHits = hits;
            this.roof = isRoof;
            this.shouldHide = false;
            blocks = new List<Block>();
            brush = b;
            blocks.Add(new Block(0, 0, size, brush));
        }

        public void Draw(Graphics g)
        {
            foreach(Block b in blocks)
            {
                brush.Color = Color.FromArgb(opac, brush.Color);
                if(visible && getRect(b).IntersectsWith(new Rectangle(0,0,Globals.screenSize,Globals.screenSize)))
                    g.FillRectangle(brush, getRect(b));
            }
        }

        public Rectangle getRect(Block block)
        {
            return new Rectangle((block.xOff * Globals.scale) + layer.map.xOffset + (layerX * screenSize) + (xOff * screenSize), (block.yOff * Globals.scale) + layer.map.yOffset + (layerY * screenSize) + (yOff * screenSize), block.screenSize, block.screenSize);
        }

        public void Update(int opac)
        {
            this.opac = opac;
            if (roof && layer.map.cells[this.layer.cellId].health < 1)
                visible = false;
        }

        public bool CheckForCol(Rectangle rect, int xAdd, int yAdd)
        {
            if (!this.solid && !this.roof)
                return false;
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
            if (xAdd > 0)
            {
                screenRect.X -= xAdd;
            }
            else if (xAdd < 0)
            {
                screenRect.X -= xAdd;
            }
            if (yAdd > 0)
            {
                screenRect.Y -= yAdd + Globals.scale;
            }
            else if (yAdd < 0)
            {
                screenRect.Y -= yAdd + Globals.scale;
            }
            for (int i = 0; i < blocks.Count; i++)
            {
                if (!getRect(blocks[i]).IntersectsWith(screenRect) && roof)
                    shouldHide = false;
                if (getRect(blocks[i]).IntersectsWith(screenRect) && roof)
                {
                    shouldHide = true;
                    break;
                }

                    

                if (getRect(blocks[i]).IntersectsWith(screenRect) && solid && visible && !roof)
                {
                    Rectangle rects = getRect(blocks[i]);
                    return true;
                }
            }
            return false;
        }
        public bool CheckForHit(Rectangle rect, int hits)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                if (getRect(blocks[i]).IntersectsWith(rect) && solid && visible)
                {
                    Block b = blocks[i];
                    this.hits -= hits;
                    if (this.hits < 1)
                    {
                        this.layer.map.cells[this.layer.cellId].health -= 1;
                        BreakBlock(blocks[i]);
                        if (b.size < 2)
                        {
                            blocks.Remove(b);
                        }
                        hits = maxHits;
                    }
                    return true;
                }
            }
            return false;
        }

        public void BreakBlock(Block b)
        {
            if (b.size < 2)
            {
                blocks.Remove(b);
                hits = maxHits;
                return;
            }
            b.size /= 2;
            blocks.Add(new Block((b.xOff + b.size), (b.yOff + b.size), b.size, brush));
            blocks.Add(new Block((b.xOff + b.size), (b.yOff), b.size, brush));
            blocks.Add(new Block((b.xOff), (b.yOff + b.size), b.size, brush));
        }
    }
}
