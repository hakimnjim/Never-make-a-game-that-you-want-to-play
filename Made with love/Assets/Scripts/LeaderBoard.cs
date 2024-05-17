using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private MatchingCardType matchingCardType;
    [SerializeField] private List<Text> scoreTexts;

    private void Start()
    {
        ScoreData scoreData = (ScoreData)(GlobalEventManager.OnGetLeaderboard?.Invoke(matchingCardType));
        OnGetLeaderboard(scoreData);
    }

    private void OnGetLeaderboard(ScoreData scoreData)
    {
        if (scoreData.matchingCardType == matchingCardType)
        {
            for (int i = 0; i < scoreTexts.Count && i < scoreData.scores.Count; i++)
            {
                var item = scoreTexts[i];
                item.text = scoreData.scores[i].ToString("D4");

            }
        }
    }

    private void OnDisable()
    {
       
    }
}
