using System.Collections.Generic;
using NUnit.Framework;
using Src.GameSates;

namespace Src.Game
{
    public class GameSequence
    {
        private readonly bool _shouldTimerActivate;
        private readonly List<IGameStep> _gameSteps;
        
        public GameSequence(bool shouldTimerActivate, List<IGameStep> gameSteps)
        {
            
        }
    }
}