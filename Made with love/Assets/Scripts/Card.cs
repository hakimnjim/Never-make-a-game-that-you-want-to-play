using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
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
            });
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        Rotate();
        Debug.Log("hey we select this card " + this.gameObject.name);
    }

    private void OnDisable()
    {
        
    }
}
