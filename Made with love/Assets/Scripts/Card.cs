using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    private CardStruct _cardStruct;
    private bool flipInProgress;
    private bool isFacingUp;
    public bool FlipInProgress { get => flipInProgress; set => flipInProgress = value; }
    public bool IsFacingUp { get => isFacingUp; set => isFacingUp = value; }

    private Ease flipEase = Ease.OutQuad;

    [SerializeField] private SpriteRenderer currentCard;
    [SerializeField] private Sprite fontFace;
    [SerializeField] private Sprite backFace;
    [SerializeField] private float flipDuration = 0.5f;

    public void Init(CardStruct cardStruct)
    {
        _cardStruct = cardStruct;
        fontFace = cardStruct.cardSprite;
        GlobalEventManager.OnUpdateSelectedCard += OnUpdateSelectedCard;
        GlobalEventManager.OnDisableCard += OnDisableCard;
    }

    private void OnDisableCard(int id)
    {
        if (id == _cardStruct.cardID)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnUpdateSelectedCard(bool isMatch, Card card)
    {
        if (card == this)
        {
            CardMatching cardMatching = new CardMatching
            {
                card = this,
                cardStruct = _cardStruct
            };
            GlobalEventManager.OnRemoveCard?.Invoke(cardMatching);
            StartCoroutine(UpdateFlipCard(isMatch));
        }
    }

    IEnumerator UpdateFlipCard(bool isMatch)
    {
        yield return new WaitForSeconds(1);
        if (isMatch)
        {
            gameObject.SetActive(false);
        }
        else
        {
            Rotate(true);
        }
    }

    public void Rotate(bool forceRotation = false)
    {
        if ((flipInProgress || IsFacingUp) && !forceRotation)
        {
            return;
        }
        float duration = 0;
        Quaternion targetRotation = IsFacingUp ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        IsFacingUp = !IsFacingUp;
        GameData.Instance.PlayEffect(SoundEffectType.FlipFace);
        transform.DORotateQuaternion(targetRotation, flipDuration)
            .SetEase(flipEase).OnUpdate(() =>
            {
                FlipInProgress = true;
                duration += Time.deltaTime;
                if (duration >= flipDuration / 2)
                {
                    currentCard.sprite = IsFacingUp ? fontFace : backFace;
                }
            })
            .OnComplete(() =>
            {
                FlipInProgress = false;

                CardMatching cardMatching = new CardMatching
                {
                    card = this,
                    cardStruct = _cardStruct
                };

                if (IsFacingUp)
                {
                    GlobalEventManager.OnAddCard?.Invoke(cardMatching);
                }
                else
                {
                    GlobalEventManager.OnRemoveCard?.Invoke(cardMatching);
                }
                
            });
    }

    private void OnMouseUp()
    {
        Rotate();
        Debug.Log("hey we select this card " + this.gameObject.name);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnUpdateSelectedCard -= OnUpdateSelectedCard;
        GlobalEventManager.OnDisableCard -= OnDisableCard;
    }
}

[System.Serializable]
public struct CardMatching
{
    public Card card;
    public CardStruct cardStruct;
}

