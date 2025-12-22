using System.Collections.Generic;
using UnityEngine;

namespace DTO
{
    [System.Serializable]
    public class SequencesFileDTO
    {
        public List<SequenceDTO> sequences;
    }

    [System.Serializable]
    public class SequenceDTO
    {
        public string id;
        public bool shouldTimerActivate;
        public List<string> expectedAnswers;
        public string backgroundSprite;
        public string mapSprite;
        public PositionDTO positionToMinimap;
        public List<string> steps;
    }

    [System.Serializable]
    public class PositionDTO
    {
        public float x;
        public float y;
    }
}