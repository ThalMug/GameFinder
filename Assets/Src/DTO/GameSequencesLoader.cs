using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Src.Game;
using Src.GameSates;
using Src.UI;

public static class GameSequencesLoader
{
    private static readonly string GAME_DATA_PATH =
        Path.Combine(Application.dataPath, "..", "GameData");

    public static Dictionary<string, List<GameSequence>> LoadAll(UIController uiController)
    {
        Dictionary<string, List<GameSequence>> gameSequencesDict = new ();

        if (!Directory.Exists(GAME_DATA_PATH))
        {
            Debug.LogError($"GameData folder not found: {GAME_DATA_PATH}");
            return gameSequencesDict;
        }

        string[] jsonFiles = Directory.GetFiles(
            GAME_DATA_PATH,
            "*.json",
            SearchOption.AllDirectories
        );

        foreach (string jsonPath in jsonFiles)
        {
            try
            {
                List<GameSequence> gameSequences = new();
                
                string json = File.ReadAllText(jsonPath);
                PackDTO pack = JsonUtility.FromJson<PackDTO>(json);
                string fileName = Path.GetFileName(jsonPath);

                if (pack?.sequences == null)
                    continue;

                string packFolder = Path.GetDirectoryName(jsonPath);

                foreach (SequenceDTO seq in pack.sequences)
                {
                    gameSequences.Add(BuildSequence(seq, packFolder, uiController));
                }
                
                gameSequencesDict.Add(fileName, gameSequences);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error loading {jsonPath}: {e}");
            }
        }

        return gameSequencesDict;
    }

    private static GameSequence BuildSequence(
        SequenceDTO dto,
        string packFolder,
        UIController uiController
    )
    {
        Texture2D bgTex = LoadTexture(dto.backgroundSprite, packFolder);
        Sprite mapSprite = LoadSprite(dto.mapSprite, packFolder);

        GameSequenceData data = new GameSequenceData
        {
            expectedAnswers = dto.expectedAnswers,
            BackgroundTexture2D = bgTex,
            mapSprite = mapSprite,
            positionToMinimap = new Vector2(
                dto.positionToMinimap.x,
                dto.positionToMinimap.y
            ),
            shouldTimerActivate = dto.shouldTimerActivate
        };

        List<IGameStep> steps = new List<IGameStep>();

        foreach (string stepId in dto.steps)
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
                    Debug.LogError($"Unknown step: {stepId}");
                    break;
            }
        }

        return new GameSequence(dto.shouldTimerActivate, steps);
    }

    private static Texture2D LoadTexture(string fileName, string folder)
    {
        if (string.IsNullOrEmpty(fileName))
            return null;

        string path = Path.Combine(folder, fileName);

        if (!File.Exists(path))
        {
            Debug.LogError($"Texture not found: {path}");
            return null;
        }

        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        return tex;
    }

    private static Sprite LoadSprite(string fileName, string folder)
    {
        Texture2D tex = LoadTexture(fileName, folder);
        if (tex == null)
            return null;

        return Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            100f
        );
    }

    [Serializable]
    private class PackDTO
    {
        public List<SequenceDTO> sequences;
    }

    [Serializable]
    private class SequenceDTO
    {
        public string id;
        public bool shouldTimerActivate;
        public string[] expectedAnswers;
        public string backgroundSprite;
        public string mapSprite;
        public PositionDTO positionToMinimap;
        public List<string> steps;
    }

    [Serializable]
    private class PositionDTO
    {
        public float x;
        public float y;
    }
}
