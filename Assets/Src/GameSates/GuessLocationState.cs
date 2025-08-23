using System;
using System.Linq;
using Src.Game;
using Src.UI;
using UnityEngine;

namespace Src.GameSates
{
    public class GuessLocationState : IGameState
    {
        private readonly UIController _uiController;
        private readonly GamePhaseData _data;
        private Action _onComplete;
        
        public GuessLocationState(GamePhaseData data, UIController uiController)
        {
            _data = data;
            _uiController = uiController;
        }
        
        public void Enter(Action onComplete)
        {
            _uiController.ShowMap();
            _onComplete = onComplete;
            MiniMapView.OnPositionSelected += OnPositionSelected;
        }

        public void Exit()
        {
            _uiController.HideMap();
            MiniMapView.OnPositionSelected -= OnPositionSelected;
            
            Debug.LogError("exiting");
        }

        private void OnWordSubmitted(string inputWord)
        {
            if (IsCorrect(inputWord))
            {
                _onComplete?.Invoke();
            }
            else
            {
                _uiController.EmptyTextBox();
                Debug.LogError("Wrong word");
            }
        }
        
        private bool IsCorrect(string input)
        {
            return GamePhaseData.expectedAnswers.Any(answer =>
                string.Equals(answer.Trim(), input.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private void OnPositionSelected()
        {
            
        }
    }
}