using System;
using System.Linq;
using Src.Game;
using Src.UI;
using UnityEngine;

namespace Src.GameSates
{
    public class GuessLocationStep : IGameStep
    {
        private readonly UIController _uiController;
        private readonly GameSequenceData _data;
        private Action _onComplete;
        
        public GuessLocationStep(GameSequenceData data, UIController uiController)
        {
            _data = data;
            _uiController = uiController;
        }
        
        public void StartStep(Action onComplete)
        {
            _uiController.ShowMap();
            _onComplete = onComplete;
            MiniMapView.OnPositionSelected += OnPositionSelected;
        }

        public void CompleteStep()
        {
            _uiController.HideMap();
            MiniMapView.OnPositionSelected -= OnPositionSelected;
            
            Debug.LogError("exiting");
        }


        private void OnPositionSelected()
        {
            _onComplete?.Invoke();
        }
    }
}