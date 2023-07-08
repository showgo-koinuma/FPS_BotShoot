using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class ScoreManager : MonoBehaviour
{
    TextMeshProUGUI _scoreText;
    int[] _scoreList = new int[5];
    string _rankingText;

    private void Update()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Lobby") // ���r�[�����[�h���ꂽ��_scoreText�ɃX�R�A��\������
        {
            _scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            _rankingText = "";
            for (int i = 0; i < _scoreList.Length; i++)
            {
                _rankingText += $"{_scoreList[i]}\r\n";
            }
            _scoreText.text = _rankingText;
            Debug.Log(_rankingText);
        }
    }

    /// <summary>�V�����X�R�A���Z�b�g����</summary>
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
