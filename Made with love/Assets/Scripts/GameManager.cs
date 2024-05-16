using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private AllCardItems allCard;
    private List<CardItem> currentCards;
    private List<Card> gameCards = new List<Card>();
    public List<CardMatching> cardMtchings;
    [SerializeField] private List<MatchingGame> matchingGames;
    [SerializeField] private MatchingCardType currentMatchingCardType;
    private MatchingGame currentMatchingGame;
    private float gameTime;
    private int tunrnsCount;
    private int matchesCount;
    private int matchesRequire;
    private bool gameOver;


    private void OnEnable()
    {
        GlobalEventManager.OnAddCard += OnAddCard;
        GlobalEventManager.OnRemoveCard += OnRemoveCard;
        InitGame();
    }

    private void InitGame()
    {
        int indexGameType = matchingGames.FindIndex(x => x.matchingCardType == currentMatchingCardType);
        if (indexGameType != -1)
        {
            currentMatchingGame = matchingGames[indexGameType];
        }
        else
        {
            Debug.Log("Something wrong");
            return;
        }
        int rows = currentMatchingGame.rows;
        int columns = currentMatchingGame.columns;
        matchesRequire = (rows * columns) / 2;
        Shuffle(allCard.cardItems);
        if (allCard.cardItems.Count >= rows * columns)
        {
            currentCards = allCard.cardItems.GetRange(0, rows * columns / 2);
            currentCards.AddRange(currentCards);
            Shuffle(currentCards);
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int indexCard = row * columns + col;
                if (indexCard < currentCards.Count)
                {
                    Card card = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
                    card.Init(currentCards[indexCard].cardStruct);
                    gameCards.Add(card);
                }
            }
        }
    }

    private void Start()
    {
        GlobalEventManager.OnScaleGridInContainer?.Invoke(currentMatchingGame.rows, currentMatchingGame.columns, gameCards);
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
            tunrnsCount++;
            if (isMAtching)
            {
                matchesCount++;
                if (matchesCount == matchesRequire)
                {
                    GlobalEventManager.OnGameOver?.Invoke(matchesCount * 1000 * matchesRequire / tunrnsCount * (int)gameTime);
                    gameOver = true;
                }
            }
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(isMAtching, cardMatching1.card);
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(isMAtching, cardMatching2.card);
            GlobalEventManager.OnUpdateGameScore?.Invoke(tunrnsCount, matchesCount);
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

        gameTime += Time.deltaTime;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);

        // Format the time as "00:00"
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        GlobalEventManager.OnUpdateTimer?.Invoke(timeString);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnAddCard -= OnAddCard;
        GlobalEventManager.OnRemoveCard -= OnRemoveCard;
    }
}

public enum MatchingCardType { None, TwoByTwo, TwoByThree, ThreeByTwo, OneByFour}

[System.Serializable]
public struct MatchingGame
{
    public MatchingCardType matchingCardType;
    public int rows;
    public int columns;
}