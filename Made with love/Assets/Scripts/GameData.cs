using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance;
    public static GameData Instance
    {
        get { return instance; }
    }
    private MatchingCardType currentMatchingCardType;
    [SerializeField] private List<SoundEffect> soundEffects;
    [SerializeField] private List<AudioSource> effectAudioSource;
    [SerializeField] private AudioSource soundEffectAudioSourcePrefab;

    public MatchingCardType CurrentMatchingCardType
    {
        get { return currentMatchingCardType; }
        set { currentMatchingCardType = value; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (!SaveSystem.VerifPath())
        {
            SaveSystem.Save(new List<LevelData>(), new List<ScoreData>());
        }
    }

    private void OnEnable()
    {
        GlobalEventManager.OnGetLeaderboard += GetScoreData;
    }

    public ScoreData GetScoreData(MatchingCardType matchingCardType)
    {
        var scoreDatas = SaveSystem.Load().scoreDatas;
        var scoreData = new ScoreData();

        if (scoreDatas.Count > 0)
        {
            int index = scoreDatas.FindIndex(x => x.matchingCardType == matchingCardType);
            if (index != -1)
            {
                scoreData = scoreDatas[index];
                scoreData.scores.Sort((a, b) => b.CompareTo(a));
            }
        }

        return scoreData;

    }

    public void PlayEffect(SoundEffectType soundEffectType)
    {
        int index = soundEffects.FindIndex(x => x.soundEffectType == soundEffectType);
        if (index != -1)
        {
            AudioClip audioClip = soundEffects[index].audio;
            foreach (var item in effectAudioSource)
            {
                if (!item.isPlaying)
                {
                    item.PlayOneShot(audioClip);
                    return;
                }
            }
            AudioSource newAudioSource = Instantiate(soundEffectAudioSourcePrefab, transform);
            newAudioSource.PlayOneShot(audioClip);
            effectAudioSource.Add(newAudioSource);
        }

    }

    public void ClearData()
    {
        SaveSystem.Save(new List<LevelData>(), new List<ScoreData>());
        GlobalEventManager.OnClearData?.Invoke();
    }

    private void OnDisable()
    {
        GlobalEventManager.OnGetLeaderboard -= GetScoreData;
    }
}

public enum SoundEffectType { FlipFace, GameWin, Matching, Mismatching }

[System.Serializable]
public struct SoundEffect
{
    public SoundEffectType soundEffectType;
    public AudioClip audio;
}
