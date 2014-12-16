using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Destruct.Entities.PlayerTools
{
    public class Gun
    {
        public List<Bullet> bullets;
        bool isOldKeyDown = false;
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

        public Gun(int size, int speed, int acc, int rate, int pauseTime, int reloadTime, int totalAM, int curAM, int AMSize, int hits, Player p, string name)
        {
            this.name = name;
            this.size = size;
            this.speed = speed;
            this.acc = acc;
            this.rate = rate;
            this.pauseTime = pauseTime;
            this.p = p;
            bullets = new List<Bullet>();
            this.ammoSize = AMSize;
            this.totalAmmo = totalAM;
            this.currentAmmo = curAM;
            this.hits = hits;
            this.reloadTime = reloadTime;
        }

        public void Update(TileMaps.TileMap map)
        {
            reloadPause--;
            pause--;
            isOldKeyDown = Destruct.Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.LeftMouse);
            float xDiff = Globals.mouseX - (Globals.halfScreenSize);
            float yDiff = Globals.mouseY - (Globals.halfScreenSize);
            dir = Math.Atan2(xDiff, -yDiff);
            if (!Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Shift) &&Destruct.Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.LeftMouse))
            {
                if (ReloadDone() && PauseDone())
                {
                    if (curRate < rate && currentAmmo > 0)
                    {
                        currentAmmo--;
                        curRate++;
                        bullets.Add(new Bullet(Globals.halfScreenSize + (p.w / 2) - (size * Globals.scale / 2), Globals.halfScreenSize + (p.h / 2) - (size * Globals.scale / 2), this.speed, this.size, dir, acc, hits));
                        if (curRate >= rate)
                        {
                            pause = pauseTime;
                            curRate = 0;
                            return;
                        }
                    }
                }
            }
            if (currentAmmo < 1)
            {
                reloadPause = reloadTime;
                if (totalAmmo > ammoSize)
                {
                    currentAmmo = ammoSize;
                    totalAmmo -= ammoSize;
                }
                else
                {
                    currentAmmo = totalAmmo;
                    totalAmmo = 0;
                }
                curRate = 0;
            }
            List<Bullet> newBullets = bullets;
            for(int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                bullets[i].Update();
                if (map.CheckForHit(new Rectangle((int)b.x, (int)b.y, b.size * Globals.scale, b.size * Globals.scale), hits))
                    newBullets.Remove(b);
                if (!new Rectangle((int)b.x, (int)b.y, b.size * Globals.scale, b.size * Globals.scale).IntersectsWith(new Rectangle(0, 0, Globals.screenSize, Globals.screenSize)))
                    newBullets.Remove(b);
            }
            bullets = newBullets;
        }

        public void Draw(Graphics g)
        {
            foreach (Bullet b in bullets)
                b.Draw(g);
            g.DrawLine(new Pen(new SolidBrush(Color.Red), Globals.scale), (float)Globals.halfScreenSize + (p.w / 2), (float)Globals.halfScreenSize + (p.h / 2), (float)(Globals.halfScreenSize + (p.w / 2) + (Math.Sin(dir) * 5 * Globals.scale)), (float)(Globals.halfScreenSize + (p.h / 2) + (-Math.Cos(dir) * 5 * Globals.scale)));
        }

        public void DrawAmmoText(Graphics g)
        {
            if (reloadPause > 0 && totalAmmo != 0)
            {
                g.DrawString("Reloading...", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
                g.DrawLine(new Pen(new SolidBrush(Color.Black), (5 * Globals.scale)), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (10 * Globals.scale)), new Point(Globals.halfScreenSize + (15 * Globals.scale), Globals.halfScreenSize - (10 * Globals.scale)));
                float barWidth = ((float)reloadPause / (float)reloadTime) * ((float)28 * (float)Globals.scale);
                if (reloadPause == reloadTime)
                    leftBar = (int)barWidth / 2;
                g.DrawLine(new Pen(new SolidBrush(Color.White), (3 * Globals.scale)), new Point(Globals.halfScreenSize, Globals.halfScreenSize - (10 * Globals.scale)), new Point(Globals.halfScreenSize + (int)barWidth / 2, Globals.halfScreenSize - (10 * Globals.scale)));
            }
            g.DrawString(name +"\n" + currentAmmo + " / " + totalAmmo, new Font(FontFamily.GenericSansSerif, 20), new SolidBrush(Color.Black), new PointF(20, 20));
            for (int i = 0; i < p.guns.Count; i++ )
            {
                if (p.guns[i] != this)
                {
                    int index = i;
                    if (p.guns.IndexOf(this) < i)
                        index--;
                    g.DrawString(p.guns[i].name + " (" + p.guns[i].currentAmmo + " / " + p.guns[i].totalAmmo + ")", new Font(FontFamily.GenericSansSerif, 15), new SolidBrush(Color.Black), new PointF(20, 75 + index * 20));
                }
            }
        }


        public bool ReloadDone()
        {
            if (reloadPause > 0)
                return false;
            //curRate = 0;
            return true;
        }
        public bool PauseDone()
        {
            if (pause > 0)
                return false;
            //curRate = 0;
            return true;
        }
    }
}
