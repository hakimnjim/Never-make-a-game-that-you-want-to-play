using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<LevelData> levelDatas = new List<LevelData>();
    public List<ScoreData> scoreDatas = new List<ScoreData>(); // Add this line

    public PlayerData(List<LevelData> levels, List<ScoreData> scores) // Modify the constructor
    {
        levelDatas = levels;
        scoreDatas = scores; // Add this line
    }
}

[System.Serializable]
public struct LevelData
{
    public List<int> cardData;
    public List<int> cardMatching;
    public int col;
    public int row;
    public MatchingCardType matchingCardType;
    public int tunrnsCount;
    public int matchesCount;
    public int matchesRequire;
    public float gameTime;
}

[System.Serializable]
public struct ScoreData
{
    public MatchingCardType matchingCardType;
    public List<int> scores;
}