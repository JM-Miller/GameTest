using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.GameStates
{
    public abstract class GameState
    {
        public abstract void Init();
        public abstract void Update();
        public abstract void Draw(Graphics g);
    }
}
