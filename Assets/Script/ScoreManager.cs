using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ScoreManager : MonoBehaviour
{
    TextMeshProUGUI _rankingText;
    int[] _scoreList = new int[5];
    string _rankingString;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Lobby") // ロビーがロードされたら_scoreTextにスコアを表示する
        {
            _rankingText = GameObject.Find("RankingText").GetComponent<TextMeshProUGUI>();
            _rankingString = "";
            for (int i = 0; i < _scoreList.Length; i++)
            {
                _rankingString += $"{_scoreList[i]}\r\n";
            }
            _rankingText.text = _rankingString;
            Debug.Log(_rankingString);
        }
    }

    /// <summary>新しいスコアをセットする</summary>
    /// <param name="newScore"></param>
    public void SetScore(int newScore)
    {
        if (newScore > _scoreList[4])
        {
            _scoreList[4] = newScore;
            Array.Sort(_scoreList);
            Array.Reverse(_scoreList);
        }
    }
}
