using System;
using System.Collections.Generic;
using Src.GameSates;
using Src.UI;
using UnityEditorInternal;
using UnityEngine;

namespace Src.Game
{
    public class GameController : MonoBehaviour
    {
        [Header("Deps")]
        [SerializeField] private UIController uiController;
        [SerializeField] private GameSequenceData data;

        [Header("Flow")]
        [SerializeField] private bool autoStart = true;
    }
}
