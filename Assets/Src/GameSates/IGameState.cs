using System;

namespace Src.GameSates
{
    public interface IGameState
    {
        void Enter(Action onComplete);
        void Exit();
    }
}