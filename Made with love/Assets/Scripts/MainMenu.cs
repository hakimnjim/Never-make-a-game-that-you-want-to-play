using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button easyButton;
    [SerializeField] private Button meduimButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button clearDataButton;

    private void OnEnable()
    {
        easyButton.onClick.AddListener(() => ChooseGameAction(MatchingCardType.TwoByTwo));
        meduimButton.onClick.AddListener(() => ChooseGameAction(MatchingCardType.TwoByThree));
        hardButton.onClick.AddListener(() => ChooseGameAction(MatchingCardType.FiveBySix));
        clearDataButton.onClick.AddListener(() => GameData.Instance.ClearData());
    }


    private void ChooseGameAction(MatchingCardType matchingCardType)
    {
        GameData.Instance.CurrentMatchingCardType = matchingCardType;
        SceneManager.LoadSceneAsync(1);
    }

    private void OnDisable()
    {
        easyButton.onClick.RemoveAllListeners();
        meduimButton.onClick.RemoveAllListeners();
        hardButton.onClick.RemoveAllListeners();
        clearDataButton.onClick.RemoveAllListeners();
    }
}
