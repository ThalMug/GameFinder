using System;
using System.Collections.Generic;
using Src.GameSates;
using Src.Loaders;
using Src.UI;
using UnityEditorInternal;
using UnityEngine;

namespace Src.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("Deps")]
        [SerializeField] private UIController uiController;

        [Header("Flow")]
        [SerializeField] private bool autoStart = true;

        public int _currentSequenceIndex = -1;
        private List<GameSequence> _gameSequences;
        private GameSequence _currentSequence =>
            _currentSequenceIndex >= 0 && _currentSequenceIndex < _gameSequences.Count
                ? _gameSequences[_currentSequenceIndex]
                : null;

        public static GameController Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        void Start()
        {
            var sequences = GameSequencesLoader.LoadPack(
                "pack1",
                uiController
            );

            StartGameSequences(sequences);
        }

        
        public void StartGameSequences(List<GameSequence> gameSequences)
        {
            _gameSequences = gameSequences;
            StartNextGameSequence();
        }

        private void OnGameSequenceCompleted()
        {
            _currentSequence.OnSequenceCompleted -= OnGameSequenceCompleted;
            StartNextGameSequence();
        }

        private void StartNextGameSequence()
        {
            _currentSequenceIndex++;

            if (_currentSequence != null)
            {
                _currentSequence.OnSequenceCompleted += OnGameSequenceCompleted;   
                _currentSequence.StartNextStep();
            }
            else
            {
                OnGameSequencesEnd();
            }
        }

        private void OnGameSequenceInterrupted()
        {
            
        }

        private void OnGameSequencesEnd()
        {
            Debug.LogError("yay");
        }
    }
}
