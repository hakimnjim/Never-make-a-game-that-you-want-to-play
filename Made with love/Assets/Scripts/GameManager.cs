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
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;

    private void OnEnable()
    {
        GlobalEventManager.OnAddCard += OnAddCard;
        GlobalEventManager.OnRemoveCard += OnRemoveCard;
        InitGame();
    }

    private void Start()
    {
        GlobalEventManager.OnScaleGridInContainer?.Invoke(rows, columns, gameCards);
    }

    private void InitGame()
    {
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

    private void OnAddCard(CardMatching cardMatching)
    {
        cardMtchings.Add(cardMatching);
        if (cardMtchings.Count > 1)
        {
            CardStruct c1 = cardMtchings[0].cardStruct;
            CardStruct c2 = cardMtchings[1].cardStruct;
            CardMatching cardMatching1 = cardMtchings[0];
            CardMatching cardMatching2 = cardMtchings[1];
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(c1.cardID == c2.cardID, cardMatching1.card);
            GlobalEventManager.OnUpdateSelectedCard?.Invoke(c1.cardID == c2.cardID, cardMatching2.card);
        }
    }

    private void OnRemoveCard(CardMatching cardMatching)
    {
        int index = cardMtchings.FindIndex(x => x.card == cardMatching.card);
        if (index != -1)
        {
            cardMtchings.RemoveAt(index);
        }
        else
        {
            Debug.Log("Something wrong");
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

    private void OnDisable()
    {
        GlobalEventManager.OnAddCard -= OnAddCard;
        GlobalEventManager.OnRemoveCard -= OnRemoveCard;
    }
}
