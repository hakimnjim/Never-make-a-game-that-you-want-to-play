using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card Item")]
public class CardItem : ScriptableObject
{
    public CardStruct cardStruct;
   
}

[System.Serializable]
public struct CardStruct
{
    public Sprite cardSprite;
    public int cardID;
}
