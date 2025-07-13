﻿using System;
using System.Linq;
using Src.Game;
using Src.UI;
using UnityEngine;

namespace Src.GameSates
{
    public class GuessWordState : IGameState
    {
        
        private readonly UIController _uiController;
        private readonly GamePhaseData _data;
        private Action _onComplete;
        
        public GuessWordState(GamePhaseData data, UIController uiController)
        {
            _data = data;
            _uiController = uiController;
        }
        
        public void Enter(Action onComplete)
        {
            _uiController.ShowTextBox();
            _onComplete = onComplete;

            WordInputView.OnWordSubmitted += OnWordSubmitted;
        }

        public void Exit()
        {
            _uiController.HideTextBox();
            
            WordInputView.OnWordSubmitted -= OnWordSubmitted;
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

    }
}