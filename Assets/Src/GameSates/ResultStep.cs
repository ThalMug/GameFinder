using System;
using Src.Game;
using Src.UI;
using UnityEngine;

namespace Src.GameSates
{
    public class ResultStep : IGameStep
    {
        private readonly UIController _uiController;
        private readonly GameSequenceData _data;
        private Action _onComplete;
        
        public ResultStep(GameSequenceData data, UIController uiController)
        {
            _data = data;
            _uiController = uiController;
        }
        
        public void StartStep(Action onComplete)
        {
            _onComplete = onComplete;
            _uiController.ShowResults(_data.mapSprite, new Vector2(0,0), _data.positionToMinimap);
            _uiController.OnResultScreenClosed += CompleteStep;
        }

        public void CompleteStep()
        {
            _uiController.OnResultScreenClosed -= CompleteStep;
            _onComplete?.Invoke();
        }
    }
}