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
            Sprite mapSprite = Resources.Load<Sprite>("background"); 
            //_uiController.ShowResults(mapSprite, new Vector2(0,0), GamePhaseData.p2);
        }

        public void CompleteStep()
        {
            throw new NotImplementedException();
        }
    }
}