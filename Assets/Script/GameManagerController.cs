using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int _killScore = 50;
    [SerializeField] GameObject _scoreTextObject;
    int _score = 0;
    Text _scoreText;

    private void Start()
    {
        _scoreText = _scoreTextObject.GetComponent<Text>();
    }

    private void Update()
    {
        _scoreText.text = _score.ToString("000");
    }

    /// <summary>Kill‚µ‚½‚Æ‚«‚Ìƒ|ƒCƒ“ƒg‰ÁŽZ</summary>
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
