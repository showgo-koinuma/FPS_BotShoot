using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageGameManagerController : MonoBehaviour
{
    [SerializeField] int _killScore = 50;
    [SerializeField] TextMeshProUGUI _scoreText;
    int _score = 0;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

    private void OnApplicationQuit()
    {
        Cursor.visible = true;
    }
}
