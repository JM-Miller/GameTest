using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    public abstract class Item
    {
        public int x;
        public int y;
        public int xPos;
        public int yPos;
        public int xOff;
        public int yOff;
        public int w;
        public int h;
        public Rectangle rect { get { return new Rectangle(x, y, w * Globals.scale, h * Globals.scale); } }
        public bool remove;
        public abstract void Init();
        public abstract void Update(Player p);
        public abstract void Draw(Graphics g);
        public abstract void DrawText(System.Drawing.Graphics g);
    }
}
