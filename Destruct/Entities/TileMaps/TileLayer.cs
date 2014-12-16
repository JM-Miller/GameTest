using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.TileMaps
{
    public class TileLayer
    {
        public int xMapOffset;
        public int yMapOffset;
        public int size;
        public int cellId;
        public int health;
        public TileMap map;
        public bool isRoof;
        public int opac = 255;
        public List<Items.Item> items;

        public Tile[][] tiles;

        public TileLayer(int[][] iTiles, int s, int xOff, int yOff, TileMap map, int cellId, List<Items.Item> items)
        {
            this.size = s;
            this.xMapOffset = xOff;
            this.yMapOffset = yOff;
            this.map = map;
            this.cellId = cellId;
            Create(iTiles);
            health = 100;
        }
        public void Create(int[][] iTiles)
        {
            this.items = new List<Items.Item>();
            tiles = new Tile[iTiles.Length][];
            for (int y = 0; y < iTiles.Length; y++)
            {
                for (int x = 0; x < iTiles.Length; x++)
                {
                    tiles[x] = new Tile[iTiles[y].Length];
                }
            }
            for(int y = 0; y < iTiles.Length; y++)
            {
                for(int x = 0; x < iTiles.Length; x++)
                {
                    if (iTiles[x][y] > 999)
                    {
                        Items.Item i = new Items.Ammo(y, x, this, map.state.p); ;
                        switch (new Random().Next(1, 3))
                        {
                            case 0:
                                i = new Items.GunPickUp(y, x, this);
                                break;
                            case 1:
                                i = new Items.Ammo(y, x, this, map.state.p);
                                break;
                            case 2:
                                i = new Items.BuildingSupplies(y, x, this, map.state.p);
                                break;
                        }
                        this.items.Add(i);
                        items.Add(i);
                        iTiles[x][y] -= 1000;
                    }
                    switch (iTiles[x][y])
                    {
                        case (int)TileType.Blank:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushBlack, false, false, 0, this);
                            break;
                        case (int)TileType.DefaultWall:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushBlack, true, true, 10, this);
                            break;
                        case (int)TileType.Door:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushOrange, true, false, 0, this);
                            break;
                        case (int)TileType.Grass:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushGreen, true, false, 0, this);
                            break;
                        case (int)TileType.WoodWall:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushBrown, true, true, 20, this);
                            break;
                        case (int)TileType.StoneWall:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushSlateGray, true, true, 30, this);
                            break;
                        case (int)TileType.Tree:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushBrown, true, true, 30, this);
                            break;
                        case (int)TileType.TreeLeaves:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushTransDarkGreen, true, false, 0, this, true);
                            break;
                        case (int)TileType.StoneRoof:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushSlateGray, true, false, 0, this, true);
                            break;
                        case (int)TileType.WoodRoof:
                            tiles[x][y] = new Tile(y, x, size, Brushes.brushBrown, true, false, 0, this, true);
                            break;
                    }
                }
            }
        }

        public void Init()
        {

        }
        public void Update(List<Items.Item> items)
        {
            if (this.isRoof && health < 1)
                this.opac = 0;
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles.Length; x++)
                {
                    tiles[x][y].Update(opac);
                }
            }
            items.AddRange(this.items);
            this.items.Clear();
        }
        public void Draw(Graphics g)
        {
            if (tiles.Any(j => j.Any(i => i.shouldHide)))
                opac = 100;
            else
                opac = 255;
            for (int y = 0; y < tiles.Length; y++)
            {
                for (int x = 0; x < tiles.Length; x++)
                {
                    tiles[x][y].Draw(g);
                }
            }
        }
        public bool IsColAtRect(Rectangle rect, int xAdd, int yAdd)
        {
            int xPos = (rect.X / (size * Globals.scale) + xMapOffset);
            int yPos = (rect.Y / (size * Globals.scale) + yMapOffset);
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    if (tiles[j][i].CheckForCol(rect, xAdd, yAdd))
                        return true;
                }
            }
            return false;
        }

        internal bool CheckForHit(Rectangle rect, int hits)
        {

            int xPos = (rect.X / (size * Globals.scale) + xMapOffset);
            int yPos = (rect.Y / (size * Globals.scale) + yMapOffset);
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int j = 0; j < tiles[i].Length; j++)
                {
                    if (tiles[j][i].CheckForHit(rect, hits))
                        return true;
                    if (health < 1)
                    {
                        tiles[j][i].visible = false;
                        tiles[j][i].roof = false;
                        tiles[j][i].solid = false;
                    }
                }
            }
            return false;
        }
    }
}
