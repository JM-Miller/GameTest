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

        public GunPickUp()
        {

        }
        public GunPickUp(int x, int y, TileMaps.TileLayer l)
        {
            this.xPos = x;
            this.yPos = y;
            this.w = 2 * Globals.scale;
            this.h = 1 * Globals.scale;
            this.xOff = l.xMapOffset;
            this.yOff = l.yMapOffset;
            hasThisGun = false;
            GetGunType();
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

        public override void Update(Player p)
        {
            if (remove)
                return;
            this.x = p.map.xOffset + (xPos * (Globals.defaultTileSize * Globals.scale)) + (xOff * (Globals.defaultTileSize * Globals.scale)) + (w * Globals.scale / 2);
            this.y = p.map.yOffset + (yPos * (Globals.defaultTileSize * Globals.scale)) + (yOff * (Globals.defaultTileSize * Globals.scale)) + (h * Globals.scale / 2);
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, (rect.Width * Globals.scale),(rect.Height * Globals.scale));
            if(p.guns.Any(i => i.name == name))
                hasThisGun = true;
            if(screenRect.IntersectsWith(rect) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Space))
            {
                remove = true;
                PlayerTools.Gun g = new PlayerTools.Gun(size, speed, acc, rate, pauseTime, reloadTime, totalAmmo, currentAmmo, ammoSize, hits, p, name);
                if (p.guns.Count == 0)
                    p.activeGun = g;
                p.guns.Add(g);
                return;
            }
            if (screenRect.IntersectsWith(rect) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.F))
            {
                p.guns.Where(i => i.name == name).ToArray()[0].totalAmmo += totalAmmo;
                totalAmmo = 0;
            }
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillRectangle(Brushes.brushBlack, rect);
        }
        public override void DrawText(System.Drawing.Graphics g)
        {
            Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
            if (screenRect.IntersectsWith(rect))
            {
                g.DrawString("Press SPACE to pick up\n" + name, new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
                if(hasThisGun && totalAmmo > 0)
                    g.DrawString("\n\nOr press F to take its ammo.", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
            }
        }
    }
}
