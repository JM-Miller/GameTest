using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.Entities.TileMaps
{
    public class TileCell
    {
        public List<TileLayer> layers;
        public int health = 500;

        public TileCell()
        {
            layers = new List<TileLayer>();
        }
    }
}
