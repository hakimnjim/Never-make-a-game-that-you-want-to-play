using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGridInSquareContainer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float spacing = 0.1f;
    [SerializeField] private Transform container;
    [SerializeField] private float targetDepth = 10f;
    [SerializeField] private SpriteRenderer icon;

    private void OnEnable()
    {
        ScaleContainerWithScreen();
        GlobalEventManager.OnScaleGridInContainer += OnScaleGridInContainer;
    }

    private void OnDisable()
    {
        GlobalEventManager.OnScaleGridInContainer -= OnScaleGridInContainer;
    }

    private void OnScaleGridInContainer(int rows, int columns, List<Card> currentCards)
    {
        float cellSize = (container.localScale.x - (columns - 1) * spacing) / columns;
        float scaleFactor = cellSize / (icon.sprite.rect.width * 1.1f / icon.sprite.pixelsPerUnit);

        float startX = container.position.x - container.localScale.x / 2 + cellSize / 2;
        float startY = container.position.y + container.localScale.y / 2 - cellSize / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                int index = row * columns + col;
                if (index < currentCards.Count)
                {
                    float posX = startX + col * (cellSize + spacing);
                    float posY = startY - row * (cellSize + spacing);
                    var card = currentCards[index];
                    card.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
                    card.transform.localPosition = new Vector3(posX, posY, targetDepth);
                }
            }
        }
    }
    private void ScaleContainerWithScreen()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float containerScaleFactor = mainCamera.orthographicSize * screenAspect;
        container.localScale = new Vector3(containerScaleFactor, containerScaleFactor, 1f);
    }

}
