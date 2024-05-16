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
}
