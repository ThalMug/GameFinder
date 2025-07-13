using System;
using System.Collections.Generic;
using System.Linq;
using Src.GameSates;
using Src.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Src.Game
{
    public class GameController : MonoBehaviour
    {
        private List<IGameState> _gameStates;
        private IGameState _currentGameState;
        
        [SerializeField] private UIController uiController;
        [SerializeField] private GamePhaseData _data;
        
        public GameController(List<IGameState> gameStates)
        {
            _gameStates = gameStates;
        }

        public void Awake()
        {
            _gameStates = new() { new GuessWordState(_data, uiController) };
            _currentGameState = _gameStates.First();
            _currentGameState.Enter(OnFinished);
        }

        private void OnFinished()
        {
            _currentGameState.Exit();
        }
    }
}