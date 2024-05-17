using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (!SaveSystem.verifPath())
        {
            SaveSystem.save(new List<LevelData>(), new List<ScoreData>());
        }
        SceneManager.LoadSceneAsync(1);
        
    }

    private void OnDisable()
    {
        
    }
}
