using System;
using System.Collections.Generic;
using Src.GameSates;

namespace Src.Game
{
    public class GameSequence
    {
        private readonly bool _shouldTimerActivate;
        private readonly List<IGameStep> _gameSteps;

        private int _currentStepIndex = -1;

        public event Action OnSequenceCompleted;

        private IGameStep currentStep =>
            _currentStepIndex >= 0 && _currentStepIndex < _gameSteps.Count
                ? _gameSteps[_currentStepIndex]
                : null;

        public GameSequence(bool shouldTimerActivate, List<IGameStep> gameSteps)
        {
            _shouldTimerActivate = shouldTimerActivate;
            _gameSteps = gameSteps;
        }

        public void StartNextStep()
        {
            _currentStepIndex++;

            if (_currentStepIndex >= _gameSteps.Count)
            {
                OnSequenceCompleted?.Invoke();
                return;
            }

            currentStep.StartStep(StartNextStep);
        }
    }
}