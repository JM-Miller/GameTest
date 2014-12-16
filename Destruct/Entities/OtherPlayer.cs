using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities
{
    public class OtherPlayer : Entity
    {
        public int playerId;

        public override void Init()
        {
            playerId = 2;
        }

        public override void Update()
        {
            string d = "";
            playerId = 2;
            for (int i = 0; i < Data.data.Count; i++)
            {
                if (int.Parse(Data.data[i][0].ToString()) == playerId)
                {
                    d = Data.data[i];
                    break;
                }
            }
            if (int.Parse(Data.data[Data.data.Count - 1][0].ToString()) == playerId)
                d = Data.data[Data.data.Count - 1];
            if (Data.data.Count < 1)
                return;
            x = int.Parse(d.Split(',')[1]);
            y = int.Parse(d.Split(',')[2]);
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            g.FillEllipse(Brushes.brushSlateGray, new System.Drawing.Rectangle(x, y, ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2), ((Globals.defaultTileSize) * Globals.scale) - (Globals.scale * 2)));
        }
    }
}
