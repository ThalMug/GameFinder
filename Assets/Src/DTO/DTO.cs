using System;
using System.Collections.Generic;
using System.Numerics;

namespace DTO
{
    [Serializable]
    public class GuessSequencePackDto
    {
        public string packId;
        public string displayName;
        public List<GuessSequenceDto> sequences;
    }
    
    [Serializable]
    public class GuessSequenceDto
    {
        public string[] expectedAnswers;
        public string backgroundSprite;
        public string mapSprite;
        public Vector2Dto positionToMinimap;
        public bool shouldTimerActivate;
        public string[] steps;
    }

    [Serializable]
    public class Vector2Dto
    {
        public float x;
        public float y;

        public Vector2 ToVector2() => new Vector2(x, y);
    }
}