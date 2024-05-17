using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEventManager
{
    public delegate void AddCard(CardMatching cardMatching);
    public static AddCard OnAddCard;

    public delegate void RemoveCard(CardMatching cardMatching);
    public static RemoveCard OnRemoveCard;

    public delegate void UpdateSelectedCard(bool isMatch, Card card);
    public static UpdateSelectedCard OnUpdateSelectedCard;

    public delegate void ScaleGridInContainer(int rows, int cols, List<Card> currentCards);
    public static ScaleGridInContainer OnScaleGridInContainer;

    public delegate void UpdateGameScore(int turnCount, int matcheCount);
    public static UpdateGameScore OnUpdateGameScore;

    public delegate void UpdateTimer(string time);
    public static UpdateTimer OnUpdateTimer;

    public delegate void GameOver(int Score);
    public static GameOver OnGameOver;

    public delegate void DisableCard(int id);
    public static DisableCard OnDisableCard;

    public delegate ScoreData GetLeaderboard(MatchingCardType matchingCardType);
    public static GetLeaderboard OnGetLeaderboard;
}
