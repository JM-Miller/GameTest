using Destruct.Entities.TileMaps;
using Destruct.GameStates;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.PlayerTools
{
    public class Builder
    {
        int xPos;
        int yPos;
        int x;
        int y;
        int xFinal;
        int yFinal;
        bool isOldKeyDown;
        double dir;
        public TileMaps.TileType type;
        bool active;
        public bool building;
        bool oldSwitch;
        int pause;
        int pauseTime;
        int leftBar;
        public Dictionary<TileType, int> numOfBlocks;
        int activeType;
        int yCoord;
        int xCoord;

        public Builder()
        {
            active = true;
            numOfBlocks = new Dictionary<TileType, int>();
            activeType = 0;
        }

        public void SwitchType()
        {
            if (activeType + 1 > numOfBlocks.Count - 1)
                activeType = 0;
            else
                activeType++;
            type = numOfBlocks.Skip(activeType).First().Key;
        }

        public void Draw(Graphics g)
        {
        }
        public void DrawText(Graphics g)
        {
            if (active)
                g.DrawRectangle(new Pen(Brushes.brushBlack, Globals.scale), new Rectangle(xPos, yPos, (Globals.defaultTileSize * Globals.scale), (Globals.defaultTileSize * Globals.scale)));
        
            if (!PauseDone())
            {
                g.DrawString("Building...", new Font(FontFamily.GenericSansSerif, 5 * Globals.scale), new SolidBrush(Color.Black), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (20 * Globals.scale)));
                g.DrawLine(new Pen(new SolidBrush(Color.Black), (5 * Globals.scale)), new Point(Globals.halfScreenSize - (1 * Globals.scale), Globals.halfScreenSize - (10 * Globals.scale)), new Point(Globals.halfScreenSize + (15 * Globals.scale), Globals.halfScreenSize - (10 * Globals.scale)));
                float barWidth = ((float)pause / (float)pauseTime) * ((float)28 * (float)Globals.scale);
                if (pause == pauseTime)
                    leftBar = (int)barWidth / 2;
                if (pause == pauseTime)
                    leftBar = (int)barWidth / 2;
                g.DrawLine(new Pen(new SolidBrush(Color.White), (3 * Globals.scale)), new Point(Globals.halfScreenSize, Globals.halfScreenSize - (10 * Globals.scale)), new Point(Globals.halfScreenSize + (int)barWidth / 2, Globals.halfScreenSize - (10 * Globals.scale)));
            }

            if(numOfBlocks.Sum(i => i.Value) > 0)
                for(int i = 0; i < numOfBlocks.Count; i++)
                {
                    g.DrawString(numOfBlocks.Skip(i).First().Value + " " + numOfBlocks.Skip(i).First().Key.ToString() + " Blocks in Inventory",  new Font(FontFamily.GenericSansSerif, 15), Brushes.brushBlack, new PointF(Globals.halfScreenSize, 20 + (i * 25)));
                }
        }

        public void Update(Player p)
        {
            float xDiff = Globals.mouseX - (p.w / 2) - (Globals.halfScreenSize);
            float yDiff = Globals.mouseY - (p.h / 2) - (Globals.halfScreenSize);
            dir = Math.Atan2(xDiff, -yDiff);
            x = (int)(Globals.halfScreenSize + (p.w / 2) + (Math.Sin(dir) * p.w * Globals.scale * 1.5));
            y = (int)(Globals.halfScreenSize + (p.h / 2) + (-Math.Cos(dir) * p.h * Globals.scale * 1.5));
            yPos = (int)((int)Math.Round((double)(y / (Globals.defaultTileSize * Globals.scale))) * (Globals.defaultTileSize * Globals.scale) + (p.map.yOffset % (Globals.defaultTileSize * Globals.scale)));
            xPos = (int)((int)Math.Round((double)(x / (Globals.defaultTileSize * Globals.scale))) * (Globals.defaultTileSize * Globals.scale) + (p.map.xOffset % (Globals.defaultTileSize * Globals.scale)));
            if (oldSwitch && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Shift) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.E) && !building)
                SwitchType();
            if (numOfBlocks.Count > 0 && numOfBlocks[type] > 0 && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Shift) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.LeftMouse) && !isOldKeyDown)
                Build(p);

            if (PauseDone() && !active && building)
            {
                foreach (TileLayer tl2 in p.map.layers)
                {
                    Rectangle rect = new Rectangle(tl2.xMapOffset * (Globals.defaultTileSize * Globals.scale) + p.map.xOffset - Globals.halfScreenSize, tl2.yMapOffset * (Globals.defaultTileSize * Globals.scale) + p.map.yOffset - Globals.halfScreenSize, tl2.tiles.Length * (Globals.defaultTileSize * Globals.scale), tl2.tiles.Length * (Globals.defaultTileSize * Globals.scale));
                    Rectangle interRect = new Rectangle(xPos - Globals.halfScreenSize, yPos - Globals.halfScreenSize, (Globals.defaultTileSize * Globals.scale), (Globals.defaultTileSize * Globals.scale));
                    if (rect.IntersectsWith(interRect))
                    {
                        Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, rect.Width, rect.Height);
                        int[][][] iTiles = TileCreator.GetTestTiles();
                        if (xCoord < 0 || xCoord > iTiles[0].Length - 1 || yCoord < 0 || yCoord > iTiles[0].Length - 1)
                            continue;
                        iTiles[0][xCoord][yCoord] = (int)type;
                        p.map.layers.Add(new TileLayer(iTiles[0], tl2.size, tl2.xMapOffset, tl2.yMapOffset, p.map, 0, new List<Items.Item>()));
                        active = true;
                        building = false;
                        numOfBlocks[type]--;
                        return;
                    }
                }

            }
            if (PauseDone())
                building = false;
            pause--;
            oldSwitch = Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.Shift) && Utilities.NativeKeyboard.IsKeyDown(Utilities.KeyCode.E);
        }

        public void Build(Player p)
        {
            if (PauseDone() && active)
            {
                switch(type)
                {
                        case TileType.Blank:
                            pauseTime = 0;
                            break;
                        case TileType.DefaultWall:
                            pauseTime = 100;
                            break;
                        case TileType.Door:
                            pauseTime = 80;
                            break;
                        case TileType.Grass:
                            pauseTime = 0;
                            break;
                        case TileType.WoodWall:
                            pauseTime = 150;
                            break;
                        case TileType.StoneWall:
                            pauseTime = 200;
                            break;
                        case TileType.Tree:
                            pauseTime = 50;
                            break;
                        case TileType.TreeLeaves:
                            pauseTime = 0;
                            break;
                        case TileType.StoneRoof:
                            pauseTime = 100;
                            break;
                        case TileType.WoodRoof:
                            pauseTime = 80;
                            break;
                }
                foreach (TileLayer tl2 in p.map.layers)
                {
                    Rectangle rect = new Rectangle(tl2.xMapOffset * (Globals.defaultTileSize * Globals.scale) + p.map.xOffset - Globals.halfScreenSize, tl2.yMapOffset * (Globals.defaultTileSize * Globals.scale) + p.map.yOffset - Globals.halfScreenSize, tl2.tiles.Length * (Globals.defaultTileSize * Globals.scale), tl2.tiles.Length * (Globals.defaultTileSize * Globals.scale));
                    Rectangle interRect = new Rectangle(xPos - Globals.halfScreenSize, yPos - Globals.halfScreenSize, (Globals.defaultTileSize * Globals.scale), (Globals.defaultTileSize * Globals.scale));
                    if (rect.IntersectsWith(interRect))
                    {
                        Rectangle screenRect = new Rectangle(Globals.halfScreenSize, Globals.halfScreenSize, (Globals.defaultTileSize * Globals.scale), (Globals.defaultTileSize * Globals.scale));

                        if (screenRect.IntersectsWith(new Rectangle(xPos, yPos, (Globals.defaultTileSize * Globals.scale), (Globals.defaultTileSize * Globals.scale))))
                            return;
                        yCoord = ((int)Math.Round((double)((xPos) / (Globals.defaultTileSize * Globals.scale))) - tl2.xMapOffset - (int)Math.Round((double)(p.map.xOffset / (Globals.defaultTileSize * Globals.scale))));
                        xCoord = ((int)Math.Round((double)((yPos) / (Globals.defaultTileSize * Globals.scale))) - tl2.yMapOffset - (int)Math.Round((double)(p.map.yOffset / (Globals.defaultTileSize * Globals.scale))));
                        int[][][] iTiles = TileCreator.GetTestTiles();
                        if (xCoord < 0 || xCoord > iTiles[0].Length - 1 || yCoord < 0 || yCoord > iTiles[0].Length - 1)
                            continue;
                    }
                }
                pause = pauseTime;
                active = false;
                building = true;
            }
        }
        public bool PauseDone()
        {
            if (pause > 0)
                return false;
            return true;
        }
    }
}
