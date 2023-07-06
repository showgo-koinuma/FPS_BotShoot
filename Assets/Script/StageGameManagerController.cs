using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageGameManagerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText;
    int _score = 0;

    private void Start()
    {
        _scoreText.text = _score.ToString("000");
        Cursor.visible = false; // カーソル非表示
        Cursor.lockState = CursorLockMode.Locked; // カーソルを画面内に留める
    }

    /// <summary>ポイント加算</summary>
    /// <param name="lastHitHead"></param>
    public void AddScore(int addScore)
    {
        _score += addScore;
        _scoreText.text = _score.ToString("000");
    }

    /// <summary>ゲーム終了時の処理</summary>
    private void OnApplicationQuit()
    {
        Cursor.visible = true; // カーソルを表示
    }
}
