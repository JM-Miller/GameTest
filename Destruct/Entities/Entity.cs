using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities
{
    public abstract class Entity
    {
        public int x;// { get; set; }
        public int y;// { get; set; }
        public int w;// { get; set; }
        public int h;// { get; set; }
        public Rectangle rect { get { return new Rectangle(x, y, w, h); } }
        public Brush brush;// { get; set; }
        public Color color;// { get; set; }
        public abstract void Init();
        public abstract void Update();
        public abstract void Draw(Graphics g);
    }
}
