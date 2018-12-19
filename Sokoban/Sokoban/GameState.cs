using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban
{
    class GameState
    {
        public int CurrentLevel { get; set; }
        public bool WinLevel { get; set; }
        public State StateGame { get; set; }

        public GameState()
        {

        }

        public GameState(State stateGame, bool winLevel, int currentLevel)
        {
            CurrentLevel = currentLevel;
            WinLevel = winLevel;
            StateGame = stateGame;
        }
    }
}
