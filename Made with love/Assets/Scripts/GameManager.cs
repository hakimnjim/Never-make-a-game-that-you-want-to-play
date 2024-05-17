using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private AllCardItems allCard;
    private List<Card> gameCards = new List<Card>();
    public List<CardMatching> cardMtchings;
    [SerializeField] private List<MatchingGame> matchingGames;
    private MatchingGame currentMatchingGame;
    private bool gameOver;
    public List<LevelData> levelDatas = new List<LevelData>();
    public List<ScoreData> scoreDatas = new List<ScoreData>();


    private void OnEnable()
    {
        GlobalEventManager.OnAddCard += OnAddCard;
        GlobalEventManager.OnRemoveCard += OnRemoveCard;
        InitGame();
    }

    private void InitGame()
    {
        currentMatchingGame.matchingCardType = GameData.Instance.CurrentMatchingCardType;
        //currentMatchingGame.matchingCardType = MatchingCardType.TwoByTwo;
        int indexGameType = matchingGames.FindIndex(x => x.matchingCardType == currentMatchingGame.matchingCardType);
        if (indexGameType != -1)
        {
            currentMatchingGame = matchingGames[indexGameType];
        }
        else
        {
            Debug.Log("Something wrong");
            return;
        }

        int rows = 0;
        int columns = 0;

        LoadScore();

        if (LoadGame(ref currentMatchingGame))
        {
            rows = currentMatchingGame.rows;
            columns = currentMatchingGame.columns;
        }
        else
        {
            rows = currentMatchingGame.rows;
            columns = currentMatchingGame.columns;
            Shuffle(allCard.cardItems);
            if (allCard.cardItems.Count >= rows * columns)
            {
                currentMatchingGame.currentCards = allCard.cardItems.GetRange(0, rows * columns / 2);
                currentMatchingGame.currentCards.AddRange(currentMatchingGame.currentCards);
                Shuffle(currentMatchingGame.currentCards);

                var leveldata = new LevelData
                {
                    cardData = currentMatchingGame.currentCards.Select(card => card.cardStruct.cardID).ToList(),
                    row = rows,
                    col = columns,
                    matchingCardType = currentMatchingGame.matchingCardType,
                    cardMatching = new List<int>(),
                    tunrnsCount = 0,
                    matchesCount = 0,
                    matchesRequire = 0,
                    gameTime = 0
                };
                

                levelDatas.Add(leveldata);
                SaveSystem.save(levelDatas, scoreDatas);
            }
            else
            {
                Debug.Log("SomeThing Wrong");
                return;
            }
        }
        currentMatchingGame.matchesRequire = (rows * columns) / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int indexCard = row * columns + col;
                if (indexCard < currentMatchingGame.currentCards.Count)
                {
                    Card card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
                    card.Init(currentMatchingGame.currentCards[indexCard].cardStruct);
                    gameCards.Add(card);
                }
            }
        }
    }

    private bool LoadGame(ref MatchingGame matchingGame)
    {
        var data = matchingGame;
        var saveData = SaveSystem.load();

        if (saveData != null && saveData.levelDatas.Count > 0)
        {
            levelDatas = saveData.levelDatas;
            int index = saveData.levelDatas.FindIndex(x => x.matchingCardType == data.matchingCardType);

            if (index != -1)
            {
                var cardItems = saveData.levelDatas[index].cardData
                                .Select(cardID => allCard.cardItems.FirstOrDefault(item => item.cardStruct.cardID == cardID))
                                .Where(item => item != null)
                                .ToList();
                List<CardStruct> matchingCards = saveData.levelDatas[index].cardMatching
    .Select(cardID => allCard.cardItems.FirstOrDefault(item => item.cardStruct.cardID == cardID))
    .Where(item => item != null)
    .Select(item => item.cardStruct) // Extract the CardStruct from each CardItem
    .ToList();

                if (cardItems.Count == matchingGame.columns * matchingGame.rows)
                {
                    data.currentCards = cardItems;
                    data.rows = saveData.levelDatas[index].row;
                    data.columns = saveData.levelDatas[index].col;
                    data.matchingCards = matchingCards;
                    data.matchesCount = saveData.levelDatas[index].matchesCount;
                    data.tunrnsCount = saveData.levelDatas[index].tunrnsCount;
                    data.matchesRequire = saveData.levelDatas[index].matchesRequire;
                    data.gameTime = saveData.levelDatas[index].gameTime;

                    matchingGame = data;
                    return true;
                }
                else
                {
                    Debug.Log("Some card items not found.");
                }
            }
            else
            {
                Debug.Log("Matching card type not found.");
            }
        }
        else
        {
            Debug.Log("No save data available or no level data found.");
        }

        return false;
    }

    private void LoadScore()
    {
        var saveData = SaveSystem.load();
        scoreDatas = saveData.scoreDatas;
    }


    private void SaveLevelProgressData()
    {
        var saveData = SaveSystem.load();
        if (saveData != null && saveData.levelDatas.Count > 0)
        {
            int index = saveData.levelDatas.FindIndex(x => x.matchingCardType == currentMatchingGame.matchingCardType);
            if (index != -1)
            {
                var level = saveData.levelDatas[index];
                if (currentMatchingGame.matchesRequire == currentMatchingGame.matchesCount)
                {
                    levelDatas.Remove(level);
                    saveData.levelDatas.Remove(level);
                    SaveSystem.save(levelDatas, scoreDatas);
                }
                else
                {
                    level.cardMatching = currentMatchingGame.matchingCards.Select(card => card.cardID).ToList();
                    level.tunrnsCount = currentMatchingGame.tunrnsCount;
                    level.matchesCount = currentMatchingGame.matchesCount;
                    level.matchesRequire = currentMatchingGame.matchesRequire;
                    level.gameTime = currentMatchingGame.gameTime;
                    int leveDataIndex = levelDatas.FindIndex(x => x.matchingCardType == level.matchingCardType);
                    if (leveDataIndex != -1)
                    {
                        levelDatas[leveDataIndex] = level;
                        SaveSystem.save(levelDatas, scoreDatas);
                    }
                    else
                    {
                        Debug.Log("Something wrong");
                    }
                }
                

                
            }
            else
            {
                Debug.Log("Something Wrong");
            }
        }

    }
    private void Start()
    {
        GlobalEventManager.OnScaleGridInContainer?.Invoke(currentMatchingGame.rows, currentMatchingGame.columns, gameCards);
        foreach (var item in currentMatchingGame.matchingCards)
        {
            GlobalEventManager.OnDisableCard?.Invoke(item.cardID);
        }
        GlobalEventManager.OnUpdateGameScore?.Invoke(currentMatchingGame.tunrnsCount, currentMatchingGame.matchesCount);
    }

    private void OnAddCard(CardMatching cardMatching)
    {
        cardMtchings.Add(cardMatching);
        if (cardMtchings.Count > 1)
        {
            CardStruct c1 = cardMtchings[0].cardStruct;
            CardStruct c2 = cardMtchings[1].cardStruct;
            CardMatching cardMatching1 = cardMtchings[0];
            CardMatching cardMatching2 = cardMtchings[1];
            bool isMAtching = c1.cardID == c2.cardID;
            currentMatchingGame.tunrnsCount++;
            if (isMAtching)
            {
                currentMatchingGame.matchesCount++;
                currentMatchingGame.matchingCards.Add(c1);
                if (currentMatchingGame.matchesCount == currentMatchingGame.matchesRequire)
                {
                    int score = currentMatchingGame.matchesCount * 10 * currentMatchingGame.matchesRequire / currentMatchingGame.tunrnsCount * (int)currentMatchingGame.gameTime;
                    int index = scoreDatas.FindIndex(x => x.matchingCardType == currentMatchingGame.matchingCardType);
                    if (index != -1)
                    {
                        scoreDatas[index].scores.Add(score);
                    }
                    else
                    {
                        List<int> newScores = new List<int> { score };
                        ScoreData scoreData = new ScoreData
                        {
                            matchingCardType = currentMatchingGame.matchingCardType,
                            scores = newScores
                        };
                        scoreDatas.Add(scoreData);
                        // Remove current Game
                        int levelIndex = levelDatas.FindIndex(x => x.matchingCardType == currentMatchingGame.matchingCardType);
                        if (levelIndex != -1)
                        {
                            levelDatas.RemoveAt(levelIndex);
                        }
                        SaveSystem.save(levelDatas, scoreDatas);
                    }
  
                    GlobalEventManager.OnGameOver?.Invoke(score);
                    gameOver = true;
                }
            }

            SoundEffectType soundEffectType = isMAtching ? SoundEffectType.Matching : SoundEffectType.Mismatching;
            GameData.Instance.PlayEffect(soundEffectType);
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(isMAtching, cardMatching1.card);
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(isMAtching, cardMatching2.card);
            GlobalEventManager.OnUpdateGameScore?.Invoke(currentMatchingGame.tunrnsCount, currentMatchingGame.matchesCount);
            
        }
    }

    private void OnRemoveCard(CardMatching cardMatching)
    {
        int index = cardMtchings.FindIndex(x => x.card == cardMatching.card);
        if (index != -1)
        {
            cardMtchings.RemoveAt(index);
        }
    }

    public void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void Update()
    {
        if (gameOver)
        {
            return;
        }

        currentMatchingGame.gameTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentMatchingGame.gameTime / 60f);
        int seconds = Mathf.FloorToInt(currentMatchingGame.gameTime % 60f);

        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        GlobalEventManager.OnUpdateTimer?.Invoke(timeString);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnAddCard -= OnAddCard;
        GlobalEventManager.OnRemoveCard -= OnRemoveCard;
        SaveLevelProgressData();
    }

    private void OnApplicationQuit()
    {
        SaveLevelProgressData();
    }

    private void SaveGameTime()
    {
        var saveData = SaveSystem.load();
        if (saveData != null)
        {
            var indexLevelData = saveData.levelDatas.FindIndex(x => x.matchingCardType == currentMatchingGame.matchingCardType);
            if (indexLevelData != -1)
            {
                var levelData = saveData.levelDatas[indexLevelData];
                levelData.gameTime = currentMatchingGame.gameTime;
                saveData.levelDatas[indexLevelData] = levelData;
                SaveSystem.save(saveData.levelDatas, scoreDatas);
                Debug.Log(levelData.gameTime);
            }
        }
    }
}

public enum MatchingCardType { None, TwoByTwo, TwoByThree, ThreeByTwo, OneByFour}

[System.Serializable]
public struct MatchingGame
{
    public MatchingCardType matchingCardType;
    public int rows;
    public int columns;
    [HideInInspector]
    public List<CardItem> currentCards;
    [HideInInspector]
    public List<CardStruct> matchingCards;
    [HideInInspector]
    public int tunrnsCount;
    [HideInInspector]
    public int matchesCount;
    [HideInInspector]
    public int matchesRequire;
    [HideInInspector]
    public float gameTime;
}