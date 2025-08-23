using System;
using System.Collections.Generic;
using Src.GameSates;
using Src.UI;
using UnityEngine;

namespace Src.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("Deps")]
        [SerializeField] private UIController uiController;
        [SerializeField] private GamePhaseData data;

        [Header("Flow")]
        [SerializeField] private bool autoStart = true;

        public bool IsPlaying { get; private set; }
        public event Action OnGameComplete;

        private readonly List<IGameState> _states = new();
        private IGameState _current;
        private int _index = -1;

        private void Awake()
        {
            if (autoStart)
                StartGame();
        }

        public void StartGame()
        {
            if (uiController == null )//|| data == null)
            {
                Debug.LogError("[GameController] Missing dependencies (uiController/data).");
                return;
            }

            CleanupCurrent();
            _states.Clear();

            _states.Add(new GuessWordState(data, uiController));
            _states.Add(new GuessLocationState(data, uiController));

            _index = -1;
            IsPlaying = false;

            EnterNextState();
        }

        private void EnterNextState()
        {
            _current?.Exit();

            _index++;
            if (_index >= _states.Count)
            {
                _current = null;
                IsPlaying = false;
                OnGameComplete?.Invoke();
                return;
            }

            _current = _states[_index];
            IsPlaying = true;

            _current.Enter(EnterNextState);
        }

        private void OnDestroy()
        {
            CleanupCurrent();
        }

        private void CleanupCurrent()
        {
            try
            {
                _current?.Exit();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                _current = null;
            }
        }
    }
}
