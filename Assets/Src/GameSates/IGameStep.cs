using System;

namespace Src.GameSates
{
    public interface IGameStep
    {
        void StartStep(Action onComplete);
        void CompleteStep();
    }
}