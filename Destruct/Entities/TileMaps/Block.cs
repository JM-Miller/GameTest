using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Destruct
{
    public class Block
    {
        public int xOff;
        public int yOff;
        public int size;
        public int screenSize;
        [XmlIgnore]
        public Brush b;

        
        public Block()
        {
            b = Brushes.brushBlack;
        }

        public Block(int x, int y, int s, Brush b)
        {
            this.xOff = x;
            this.yOff = y;
            this.size = s;
            this.screenSize = s * Globals.scale;
            this.b = b;
        }
    }
}
