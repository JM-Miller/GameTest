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
        public PlayerTools.RockThrowAbility activeGun;
        public List<PlayerTools.RockThrowAbility> guns;
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
            this.guns = new List<PlayerTools.RockThrowAbility>();
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
            if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w * Globals.scale, h * Globals.scale), 0, 0))
            {
                oldX = x;
                oldY = y;
            }
            bool l = false;
            bool r = false;
            bool u = false;
            bool d = false;
            int xAdd = 0;
            int yAdd = 0;
            //if (NativeKeyboard.IsKeyDown(KeyCode.Left) || NativeKeyboard.IsKeyDown(KeyCode.A) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), (2 * Globals.scale) + (speed * Globals.scale), 0))
            if (NativeKeyboard.IsKeyDown(KeyCode.Left) || NativeKeyboard.IsKeyDown(KeyCode.A))
                xAdd += 4;
            if (NativeKeyboard.IsKeyDown(KeyCode.Right) || NativeKeyboard.IsKeyDown(KeyCode.D))
                xAdd -= 4;
            if (NativeKeyboard.IsKeyDown(KeyCode.Up) || NativeKeyboard.IsKeyDown(KeyCode.W))
                yAdd += 4;
            if (NativeKeyboard.IsKeyDown(KeyCode.Down) || NativeKeyboard.IsKeyDown(KeyCode.S))
                yAdd -= 4;
            if (map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, 0))
            {
                x = oldX;
                y = oldY;
            }
            if (xAdd != 0 && yAdd != 0)
            {
                if (map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), xAdd, 0) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, yAdd))
                    y += yAdd;
                if (map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), 0, yAdd) && !map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), xAdd, 0))
                    x += xAdd;
            }
            if (!map.IsColAtRect(new Rectangle(-x + Globals.halfScreenSize, -y + Globals.halfScreenSize, w, h), xAdd, yAdd))
            {
                x += xAdd;
                y += yAdd;
            }
        }
    }
}
