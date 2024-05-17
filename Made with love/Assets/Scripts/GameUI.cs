using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text turnsText;
    [SerializeField] private Text matchesText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject gameWinPanel;
    [SerializeField] private GameObject leftContainer;

    private void OnEnable()
    {
        GlobalEventManager.OnUpdateGameScore += OnUpdateGameScore;
        GlobalEventManager.OnUpdateTimer += OnUpdateTimer;
        GlobalEventManager.OnGameOver += OnGameOver;
    }

    private void OnGameOver(int score)
    {
        scoreText.text = score.ToString("D4");
        StartCoroutine(OnCoroutineGameOver());
    }

    IEnumerator OnCoroutineGameOver()
    {
        yield return new WaitForSeconds(1);
        GameData.Instance.PlayEffect(SoundEffectType.GameWin);
        gameWinPanel.transform.localScale = Vector3.zero;
        gameWinPanel.SetActive(true);
        leftContainer.SetActive(false);
        gameWinPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutQuad);
    }

    private void OnUpdateTimer(string time)
    {
        timerText.text = time;
    }

    private void OnUpdateGameScore(int turnCount, int matcheCount)
    {
        turnsText.text = turnCount.ToString("D3");
        matchesText.text = matcheCount.ToString("D3");
    }

    public void LoadMAinMenu()
    {
        GlobalEventManager.OnSaveData?.Invoke();
        SceneManager.LoadSceneAsync(0);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnUpdateGameScore -= OnUpdateGameScore;
        GlobalEventManager.OnUpdateTimer -= OnUpdateTimer;
        GlobalEventManager.OnGameOver -= OnGameOver;
    }
}
