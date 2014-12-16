using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destruct.GameStates
{
    public class GameStateManager
    {
        List<GameState> states;
        public GameState currentState;

        public void Init(GameForm form)
        {
            states = new List<GameState>();
            currentState = new MenuState(this);
            currentState.Init();
        }
        public void Update()
        {
            currentState.Update();
        }
        public void Draw(Graphics g)
        {
            currentState.Draw(g);
        }
    }
}
