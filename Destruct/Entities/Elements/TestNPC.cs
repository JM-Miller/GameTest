using Destruct.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.Elements
{
    public class TestNPC : Entity
    {
        public int[][] path;
        public override void Init()
        {
        }

        public override void Update()
        {
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillEllipse(Brushes.brushSlateGray, new System.Drawing.Rectangle(x, y, ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2), ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2)));
        }
    }
}
