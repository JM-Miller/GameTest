using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.PlayerTools
{
    public class Bullet
    {
        public int size;
        public int speed;
        public double x;
        public double y;
        double dir;
        int acc;
        int hits;

        public Bullet(int x, int y, int speed, int size, double dir, int acc, int hits)
        {
            this.size = size;
            this.speed = speed;
            this.dir = dir;
            this.x = x;
            this.y = y;
            this.acc = acc;
            this.hits = hits;
        }

        public void Update()
        {
            x += ((Math.Sin(dir) * speed + new Random().Next(-acc, acc)));
            y += ((-Math.Cos(dir) * speed + new Random().Next(-acc, acc)));
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(new SolidBrush(Color.Gold), (float)x, (float)y, size * Globals.scale, size * Globals.scale);
        }
    }
}
