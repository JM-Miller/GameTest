using Destruct.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Items
{
    public class GunPickUp : Item
    {

        int size;
        int speed;
        int acc;
        int rate;
        int curRate;
        int maxAmmo;
        public int ammoSize;
        public int totalAmmo;
        int currentAmmo;
        double dir;
        int pauseTime;
        int pause;
        int reloadTime;
        public Player p;
        int hits;
        int leftBar;
        public string name;
        public int reloadPause;
        public bool hasThisGun;
        public int[][] path;
        int pathIndex = 0;
        bool pathDone = false;

        public GunPickUp(int x, int y, TileMaps.TileLayer l, ref Player player)
        {
            this.tileX = x;
            this.tileY = y;
            this.size = 2 * Globals.scale;
            this.layerTileX = l.xMapOffset;
            this.layerTileY = l.yMapOffset;
            hasThisGun = false;
            p = player;
            GetGunType();
            this.screenX = p.map.xOffset + (tileX * (Globals.defaultTileSize * Globals.scale)) + (layerTileX * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            this.screenY = p.map.yOffset + (tileY * (Globals.defaultTileSize * Globals.scale)) + (layerTileY * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            path = PathFinder.GetPath(p.x, p.y, this.screenX, this.screenY, p.map);//.Reverse().ToArray();
            path = PathFinder.GetPath(this.screenX, this.screenY, p.x, p.y, p.map);//.Reverse().ToArray();
        }

        public void GetGunType()
        {
            switch(new Random().Next(0,4))
            {
                case 0:
                        size = 3;
                        speed = 35; 
                        acc = 1; 
                        rate = 1; 
                        pauseTime = 50; 
                        reloadTime = 100; 
                        totalAmmo = new Random().Next(0, 200); 
                        ammoSize = 5;
                        currentAmmo = new Random().Next(0, ammoSize); 
                        hits = 100;
                        name = "Sniper Rifle";
                    break;
                case 1:
                    size = 2;
                    speed = 25;
                    acc = 10;
                    rate = 25;
                    pauseTime = 5;
                    reloadTime = 70;
                    totalAmmo = new Random().Next(0, 750);
                    ammoSize = 45;
                    currentAmmo = new Random().Next(0, ammoSize);
                    hits = 5;
                    name = "Machine Gun";
                    break;
                case 2:
                    size = 2;
                    speed = 30;
                    acc = 7;
                    rate = 5;
                    pauseTime = 30;
                    reloadTime = 50;
                    totalAmmo = new Random().Next(0, 500);
                    ammoSize = 25;
                    currentAmmo = new Random().Next(0, ammoSize);
                    hits = 8;
                    name = "Assault Rifle";
                    break;
                case 3:
                    size = 2;
                    speed = 15;
                    acc = 5;
                    rate = 1;
                    pauseTime = 30;
                    reloadTime = 50;
                    totalAmmo = new Random().Next(0, 250);
                    ammoSize = 15;
                    currentAmmo = new Random().Next(0, ammoSize);
                    hits = 15;
                    name = "Pistol";
                    break;
            }
        }
        public override void Init()
        {
            throw new NotImplementedException();
        }

        public override void Update()
        {
            //this.screenX = p.map.xOffset + (tileX * (Globals.defaultTileSize * Globals.scale)) + (layerTileX * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            //this.screenY = p.map.yOffset + (tileY * (Globals.defaultTileSize * Globals.scale)) + (layerTileY * (Globals.defaultTileSize * Globals.scale)) + (size * Globals.scale / 2);
            if (!pathDone)
            {
                this.screenX = MovePointTowards(new Point(this.screenX, this.screenY), new Point(path[pathIndex][0], path[pathIndex][1]), 2).X;
                this.screenY = MovePointTowards(new Point(this.screenX, this.screenY), new Point(path[pathIndex][0], path[pathIndex][1]), 2).Y;
                if (pathIndex < path.Length - 1 && (path[pathIndex][0] - screenX) * (path[pathIndex][0] - screenX) + (path[pathIndex][1] - screenY) * (path[pathIndex][1] - screenY) < 6 * 6)
                {
                    pathIndex++;
                }
                if (pathIndex >= path.Length)
                    pathDone = true;
            }
            //if (ShouldRemove)
            //    return;
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, (screenRectangle.Width * Globals.scale),(screenRectangle.Height * Globals.scale));
            if(p.guns.Any(i => i.name == name))
                hasThisGun = true;
            if(screenRect.IntersectsWith(screenRectangle) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                ShouldRemove = true;
                PlayerTools.Gun g = new PlayerTools.Gun(size, speed, acc, rate, pauseTime, reloadTime, totalAmmo, currentAmmo, ammoSize, hits, p, name);
                if (p.guns.Count == 0)
                    p.activeGun = g;
                p.guns.Add(g);
                return;
            }
            if (screenRect.IntersectsWith(screenRectangle) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.F))
            {
                p.guns.Where(i => i.name == name).ToArray()[0].totalAmmo += totalAmmo;
                totalAmmo = 0;
            }
        }
        static public int linear(int x, int x0, int x1, int y0, int y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        public Point MovePointTowards(Point a, Point b, double distance)
        {
            var vector = new Point(b.X - a.X, b.Y - a.Y);
            var length = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            double[] unitVector = 
            { 
                (double)(vector.X / length), (double)(vector.Y / length)
            };
            return new Point((int)(a.X + unitVector[0] * distance), (int)(a.X + unitVector[1] * distance));
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillRectangle(Brushes.brushBlack, new Rectangle(screenX, screenY, size * Globals.scale, size * Globals.scale));
        }
        public override void DrawOverLayer(System.Drawing.Graphics g)
        {
            foreach (int[] point in path)
            {
                if(point == path.First())
                    g.FillEllipse(new SolidBrush(Color.Blue), new Rectangle(point[0], point[1], 5, 5));
                else if (point == path.Last())
                    g.FillEllipse(new SolidBrush(Color.Yellow), new Rectangle(point[0], point[1], 5, 5));
                else
                    g.FillEllipse(new SolidBrush(Color.Red), new Rectangle(point[0], point[1], 5, 5));
            }
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, screenRectangle.Width, screenRectangle.Height);
            g.FillRectangle(Brushes.brushBlack, screenRectangle);
            if (screenRect.IntersectsWith(screenRectangle))
            {
                g.DrawString("Press SPACE to pick up\n" + name, new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
                if(hasThisGun && totalAmmo > 0)
                    g.DrawString("\n\nOr press F to take its ammo.", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
