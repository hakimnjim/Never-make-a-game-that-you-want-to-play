using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    void Init(CardStruct cardStruct);

    void Rotate(bool forceRotation = false);

    bool FlipInProgress { get; set; }

    bool IsFacingUp { get; set; }
}
