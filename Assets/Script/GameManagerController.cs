using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int _killScore = 50;
    [SerializeField] TextMeshProUGUI _scoreText;
    int _score = 0;

    private void Start()
    {

    }

    private void Update()
    {
        _scoreText.text = _score.ToString("000");
    }

    /// <summary>Killしたときのポイント加算</summary>
    /// <param name="lastHitHead"></param>
    public void AddKillScore(bool lastHitHead)
    {
        _score += _killScore;
        if (lastHitHead)
        {
            _score += _killScore;
        }
    }
}
