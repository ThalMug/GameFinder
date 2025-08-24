using System;
using Src.Game;
using Src.UI;
using UnityEngine;

namespace Src.GameSates
{
    public class ResultState : IGameState
    {
        private readonly UIController _uiController;
        private readonly GamePhaseData _data;
        private Action _onComplete;
        
        public ResultState(GamePhaseData data, UIController uiController)
        {
            _data = data;
            _uiController = uiController;
        }
        
        public void Enter(Action onComplete)
        {
            Sprite mapSprite = Resources.Load<Sprite>("background"); 
            _uiController.ShowResults(mapSprite, new Vector2(0,0), GamePhaseData.p2);
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}