using System.Collections.Generic;
using DTO;
using Src.Game;
using Src.GameSates;
using Src.UI;
using UnityEngine;

namespace Src.Loaders
{
    public static class GameSequencesLoader
    {
        private const string BASE_PATH = "GuessSequences/Pack01";

        public static List<GameSequence> LoadPack(
            string packFileName,
            UIController uiController
        )
        {
            TextAsset jsonAsset = Resources.Load<TextAsset>($"{BASE_PATH}/{packFileName}");

            if (jsonAsset == null)
            {
                Debug.LogError($"Pack JSON not found at {BASE_PATH}/{packFileName}");
                return null;
            }

            GuessSequencePackDto packDto =
                JsonUtility.FromJson<GuessSequencePackDto>(jsonAsset.text);

            if (packDto == null)
            {
                Debug.LogError("Failed to parse GuessSequencePackDto");
                return null;
            }

            List<GameSequence> sequences = new();

            foreach (GuessSequenceDto seqDto in packDto.sequences)
            {
                GameSequenceData data = new GameSequenceData
                {
                    expectedAnswers = seqDto.expectedAnswers,
                    backgroundSprite = LoadSprite(seqDto.backgroundSprite),
                    mapSprite = LoadSprite(seqDto.mapSprite),
                    positionToMinimap = new Vector2(
                        seqDto.positionToMinimap.x,
                        seqDto.positionToMinimap.y
                    ),
                    shouldTimerActivate = seqDto.shouldTimerActivate
                };

                List<IGameStep> steps = BuildSteps(seqDto.steps, data, uiController);

                GameSequence sequence = new GameSequence(
                    seqDto.shouldTimerActivate,
                    steps
                );

                sequences.Add(sequence);
            }

            return sequences;
        }

        private static Sprite LoadSprite(string spriteName)
        {
            if (string.IsNullOrEmpty(spriteName))
                return null;

            Sprite sprite = Resources.Load<Sprite>($"{BASE_PATH}/{spriteName}");

            if (sprite == null)
                Debug.LogError($"Sprite not found: {BASE_PATH}/{spriteName}");

            return sprite;
        }

        private static List<IGameStep> BuildSteps(
            string[] stepIds,
            GameSequenceData data,
            UIController uiController
        )
        {
            List<IGameStep> steps = new();

            foreach (string stepId in stepIds)
            {
                switch (stepId)
                {
                    case "guess_word":
                        steps.Add(new GuessWordStep(data, uiController));
                        break;

                    case "guess_location":
                        steps.Add(new GuessLocationStep(data, uiController));
                        break;

                    case "result":
                        steps.Add(new ResultStep(data, uiController));
                        break;

                    default:
                        Debug.LogError($"Unknown step id: {stepId}");
                        break;
                }
            }

            return steps;
        }
    }
}
