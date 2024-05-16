using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour, ICard
{
    private bool flipInProgress;
    private bool isFacingUp;
    public bool FlipInProgress { get => flipInProgress; set => flipInProgress = value; }
    public bool IsFacingUp { get => isFacingUp; set => isFacingUp = value; }

    public void Init(CardStruct cardStruct)
    {
       
    }

    public void Rotate(bool forceRotation = false)
    {
        
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        Debug.Log("hey we select this card " + this.gameObject.name);
    }

    private void OnDisable()
    {
        
    }
}
