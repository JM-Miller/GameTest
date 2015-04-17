using Destruct.Entities.TileMaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Utilities
{
    public static class PathFinder
    {
        public static int[][] GetPath(int startX, int startY, int endX, int endY, TileMap map)
        {
            Rectangle endRect = new Rectangle(endX, endY, Globals.defaultTileSize * Globals.scale, Globals.defaultTileSize * Globals.scale);
            Rectangle startRect = new Rectangle(startX, startY, Globals.defaultTileSize * Globals.scale, Globals.defaultTileSize * Globals.scale);
            Rectangle rect = new Rectangle(startX, startY, Globals.defaultTileSize * Globals.scale, Globals.defaultTileSize * Globals.scale);
            if(map.IsColAtRect(endRect, 0, 0))
                return new int[][] { new int[] { } };
            List<int[]> finalPath = new List<int[]>();
            int ignoreIndex = -1;
            int curX = 0;
            int curY = 0;
            List<int> ignoresX = new List<int>();
            List<int> ignoresY = new List<int>();
            while(!rect.IntersectsWith(endRect))
            {
                List<int[]> points = new List<int[]>();
                List<int> distances = new List<int>();
                for(int y = -1; y <= 1; y++)
                {
                    for(int x = -1; x <= 1; x++)
                    {
                        if (ignoresX.Count > 0 && x == ignoresX.Last() && y == ignoresY.Last())
                        {
                            ignoreIndex--;
                            continue;
                        }
                        if (!map.IsColAtRectRel(new Rectangle(rect.X, rect.Y, Globals.defaultTileSize * Globals.scale - 2, Globals.defaultTileSize * Globals.scale - 2), x * Globals.defaultTileSize * Globals.scale, y * Globals.defaultTileSize * Globals.scale))
                        {
                            if (curX == -x && curY == -y)
                            {
                                ignoresX.Add(curX);
                                ignoresY.Add(curY);
                            }
                            curX = x;
                            curY = y;
                            points.Add(new int[] { rect.X + x * Globals.defaultTileSize * Globals.scale / 2, rect.Y + y * Globals.defaultTileSize * Globals.scale / 2 });
                            distances.Add((endX - points.Last()[0]) * (endX - points.Last()[0]) + (endY - points.Last()[1]) * (endY - points.Last()[1]));
                        }
                    }
                }
                if(points.Count < 1)
                {
                    finalPath.Remove(finalPath.Last());
                    if (ignoresX[ignoreIndex] == curX && ignoresY[ignoreIndex] == curY && finalPath.Count < 1)
                    {
                        return finalPath.ToArray();
                    }
                    ignoresX.Add(curX);
                    ignoresY.Add(curY);
                        ignoreIndex++;
                    continue;
                }
                int lowest = distances[0];
                int index = 0;
                for(int i = 0; i < points.Count; i++)
                {
                    if(distances[i] < lowest)
                    {
                        index = i;
                    }
                }
                rect = new Rectangle(points[index][0], points[index][1], Globals.defaultTileSize * Globals.scale, Globals.defaultTileSize * Globals.scale);
                finalPath.Add(points[index]);
                if (finalPath.Count > 100)
                    break;
            }
            return finalPath.ToArray();
        }
    }
}
