using Destruct.Entities;
using Destruct.Entities.TileMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.GameStates
{
    public class MainState : GameState
    {
        SolidBrush clearBrush;
        Color clearColor;
        Rectangle screen;
        List<Entities.Entity> entities;
        List<Entities.Items.Item> items;
        TileMap map;
        public Player p;
        bool isOldDownEsc;
        public GameStateManager gsm;

        public MainState(GameStateManager gsm)
        {
            this.gsm = gsm;
        }

        public override void Init()
        {
            clearColor = Color.Green;
            clearBrush = new SolidBrush(clearColor);
            screen = new Rectangle(0, 0, Globals.screenSize, Globals.screenSize);
            entities = new List<Entities.Entity>();
            map = new TileMap(TileCreator.GetTestTiles(), this);
            p = new Entities.Player(map);
            entities.Add(p);
            items = new List<Entities.Items.Item>();
        }

        public override void Update()
        {
            map.Update(p.x, p.y, items);
            foreach (Entities.Entity e in entities)
                e.Update();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].Update();
                if (items[i].ShouldRemove)
                    items.Remove(items[i]);
            }
            if (Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.ESC) && !isOldDownEsc)
                gsm.currentState = new InMenuState(this.gsm);
            if (Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.v0))
                Globals.speed = 1;
            if (Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.v1))
                Globals.speed = 5;
            if (Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.v2))
                Globals.speed = 10;
            isOldDownEsc = Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.ESC);
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(clearBrush, screen);
            map.Draw(g);
            foreach (Entities.Entity e in entities)
                e.Draw(g);
            foreach (Entities.Items.Item item in items)
                item.Draw(g);
            map.DrawRoof(g);
            if (p.activeGun != null)
                p.activeGun.DrawAmmoText(g);
            foreach (Entities.Items.Item item in items)
                item.DrawOverLayer(g);
            p.builder.DrawText(g);
        }
    }

    public class TileCreator
    {
        public static Random rand = new Random();
        public static int[][][] GetTestTiles()
        {
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                    new int[]{ 0,0,0,0,0,0,0,0,0,0 },
                },
            };
        }
        public static int[][][] GetRandomTiles()
        {
            switch (rand.Next(0, 9))
            { 
            case 0:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 5, 5, 5, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 5, 3, 5, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                },
                new int[][]
                {
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                }
            };
            case 1:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 5, 5, 5, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 1, 1, 1, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 5, 5, 5, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                },
                new int[][]
                {
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 8, 8, 8, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                }
            };
            case 3:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                }
            };
            case 4:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 5, 5, 5, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 5, 5, 5, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                }
            };
            case 5:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 5, 5, 5, 5, 5, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                }
            };
            case 6:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 6, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 6, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 6, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 6, 1, 1, 1, 1, 1, 1, 1, 1 },
                },
                new int[][]
                {
                    new int[]{ 0, 0, 0, 0, 0, 0, 7, 7, 7, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 7, 7, 7, 0 },
                    new int[]{ 0, 0, 7, 0, 0, 0, 7, 7, 7, 0 },
                    new int[]{ 0, 7, 7, 7, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 7, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 7, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 7, 7, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 7, 7, 7, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 7, 0, 0, 0, 0, 0, 0, 0, 0 },
                },
            };
            case 7:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 5, 4, 4, 4, 4, 4, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 1, 1, 1, 1, 4, 5 },
                    new int[]{ 1, 1, 5, 4, 4, 3, 4, 4, 4, 5 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                },
                new int[][]
                {
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 9, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 9, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 9, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 1009, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 9, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 9, 9, 9, 9, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                }
            };
            case 8:
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 4, 4, 4, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 4, 1, 4, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 4, 3, 4, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                },
                new int[][]
                {
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 9, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                }
            };
            }
            return new int[][][]
            {
                new int[][]
                {
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                    new int[]{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                }
            };
        }
    }
}
