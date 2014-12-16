using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destruct.Utilities;
using Destruct.Entities.PlayerTools;

namespace Destruct.Entities
{
    public class Player : Entity
    {
        public TileMaps.TileMap map;
        public int speed = 3;
        public PlayerTools.Gun activeGun;
        public List<PlayerTools.Gun> guns;
        bool iskeySwitchDown = false;
        public Builder builder;

        public Player(TileMaps.TileMap map)
        {
            this.map = map;
            Init();
        }

        public override void Init()
        {
            this.w = ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2);
            this.h = ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2);
            this.x = Globals.halfScreenSize - (Globals.defaultTileSize * Globals.scale / 2);
            this.y = Globals.halfScreenSize - (Globals.defaultTileSize * Globals.scale / 2);
            this.color = Color.Black;
            this.brush = new SolidBrush(color);
            this.guns = new List<PlayerTools.Gun>();
            builder = new Builder();
        }

        public override void Update()
        {
            Move();
            if (activeGun != null)
                activeGun.Update(map);
            if (guns.Count > 0)
            {
                if (NativeKeyboard.IsKeyDown(KeyCode.E) && !iskeySwitchDown)
                    if (guns.IndexOf(activeGun) + 1 > guns.Count - 1)
                        activeGun = guns[0];
                    else
                        activeGun = guns[guns.IndexOf(activeGun) + 1];
            }
            builder.Update(this);
            iskeySwitchDown = NativeKeyboard.IsKeyDown(KeyCode.E);
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillEllipse(brush, new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, w, h));
            if(activeGun != null)
                activeGun.Draw(g);
            builder.Draw(g);
        }

        int oldX;
        int oldY;

        public void Move()
        {
            if (builder.building)
                return;
            if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, 0))
            {
                oldX = x;
                oldY = y;
            }
            bool l = false;
            bool r = false;
            bool u = false;
            bool d = false;
            //if (NativeKeyboard.IsKeyDown(KeyCode.Left) || NativeKeyboard.IsKeyDown(KeyCode.A) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), (2 * Globals.scale) + (speed * Globals.scale), 0))
            if (NativeKeyboard.IsKeyDown(KeyCode.Left) || NativeKeyboard.IsKeyDown(KeyCode.A))
                l = true;
            if (!NativeKeyboard.IsKeyDown(KeyCode.Left) && !NativeKeyboard.IsKeyDown(KeyCode.A))
                l = false;
            //if (NativeKeyboard.IsKeyDown(KeyCode.Right) || NativeKeyboard.IsKeyDown(KeyCode.D) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), -(2 * Globals.scale) + -(speed * Globals.scale), 0))
            if (NativeKeyboard.IsKeyDown(KeyCode.Right) || NativeKeyboard.IsKeyDown(KeyCode.D))
                r = true;
            if (!NativeKeyboard.IsKeyDown(KeyCode.Right) && !NativeKeyboard.IsKeyDown(KeyCode.D))
                r = false;
            //if (NativeKeyboard.IsKeyDown(KeyCode.Up) || NativeKeyboard.IsKeyDown(KeyCode.W) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y, w, h), 0, 0))
            if (NativeKeyboard.IsKeyDown(KeyCode.Up) || NativeKeyboard.IsKeyDown(KeyCode.W))
                u = true;
            if (!NativeKeyboard.IsKeyDown(KeyCode.Up) && !NativeKeyboard.IsKeyDown(KeyCode.W))
                u = false;
            //if (NativeKeyboard.IsKeyDown(KeyCode.Down) || NativeKeyboard.IsKeyDown(KeyCode.S) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, -(2 * Globals.scale) + -(speed * Globals.scale)))
            if (NativeKeyboard.IsKeyDown(KeyCode.Down) || NativeKeyboard.IsKeyDown(KeyCode.S))
                d = true;
            if (!NativeKeyboard.IsKeyDown(KeyCode.Down) && !NativeKeyboard.IsKeyDown(KeyCode.S))
                d = false;
            if(map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, 0))
            {
                x = oldX;
                y = oldY;
            }
            if(l)
            {
                if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), speed, 0))
                    x += speed;
            }
            if (r)
            {
                if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), -speed, 0))
                    x -= speed;
            }
            if (u)
            {
                if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, speed))
                    y += speed;
            }
            if (d)
            {
                if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, speed))
                    y += speed;
            }
        }
    }
}
