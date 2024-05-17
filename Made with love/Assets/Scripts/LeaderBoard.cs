using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private MatchingCardType matchingCardType;
    [SerializeField] private List<Text> scoreTexts;


    private void OnEnable()
    {
        GlobalEventManager.OnClearData += OnInitData;
    }

    private void Start()
    {
        OnInitData();
    }

    private void OnGetLeaderboard(ScoreData scoreData)
    {
        if (scoreData.matchingCardType == MatchingCardType.None)
        {
            foreach (var item in scoreTexts)
            {
                item.text = "0000"; 
            }
            return;
        }
        if (scoreData.matchingCardType == matchingCardType)
        {
            for (int i = 0; i < scoreTexts.Count; i++)
            {
                var item = scoreTexts[i];
                if (i < scoreData.scores.Count)
                {
                    item.text = scoreData.scores[i].ToString("D4");
                }
                else
                {
                    item.text = "0000";
                }
            }
        }
    }

    private void OnInitData()
    {
        ScoreData scoreData = (ScoreData)(GlobalEventManager.OnGetLeaderboard?.Invoke(matchingCardType));
        OnGetLeaderboard(scoreData);
    }

    private void OnDisable()
    {
        GlobalEventManager.OnClearData -= OnInitData;
    }
}
