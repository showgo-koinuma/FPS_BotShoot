using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageGameManagerController : MonoBehaviour
{
    /// <summary>制限時間</summary>
    [SerializeField] int _timeLimit = 60;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] TextMeshProUGUI _botCountText;
    [SerializeField] GameObject _finishPanel;
    [SerializeField] GameObject _finishText;
    [SerializeField] GameObject _returnLobbyButton;
    Animator _timerTextAnimator;
    bool _inGame = true;
    int _botNum;
    int _botCount;
    int _score = 0;
    float _timer;

    private void Start()
    {
        _scoreText.text = _score.ToString("000");
        _timer = _timeLimit;
        _botNum = GameObject.FindObjectsOfType<EnemyController>().Length;
        _botCount = _botNum;
        _timerTextAnimator = _timerText.gameObject.GetComponent<Animator>();
        _botCountText.text = $"{_botCount} / {_botNum}";
        _finishPanel.SetActive(false);
        _finishText.SetActive(false);
        _returnLobbyButton.SetActive(false);
    }

    private void Update()
    {
        if (_inGame)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 10)
            {
                _timerTextAnimator.Play("TimerColorAnimation");
            }

            if (_timer <= 0 || _botCount == 0)
            {
                _inGame = false;
                StartCoroutine(nameof(Finish));
            }

            _timerText.text = _timer.ToString("0.0");
        }
    }

    /// <summary>キル時のポイント加算</summary>
    /// <param name="lastHitHead"></param>
    public void Killed(int addScore)
    {
        _score += addScore;
        _scoreText.text = _score.ToString("000");
        _botCount--;
        _botCountText.text = $"{_botCount} / {_botNum}";
    }

    /// <summary>button用ロビーに戻る</summary>
    public void ReturnLobby()
    {
        GameObject.Find("DDOLGameManager").GetComponent<ScoreManager>().SetScore(_score);
        SceneManager.LoadScene(0);
    }

    /// <summary>ゲーム終了時の処理</summary>
    IEnumerator Finish()
    {
        _finishPanel.SetActive(true);
        _finishText.SetActive(true);
        yield return new WaitForSeconds(1);

        _finishPanel.GetComponent<Animator>().Play("PanelAnimation");
        yield return new WaitForSeconds(1);

        _finishText.GetComponent<Animator>().Play("FInishTextAnimation");
        _scoreText.gameObject.GetComponent<Animator>().Play("ScoreTextAnimation");
        yield return new WaitForSeconds(1.5f);

        int addedTimeScore = (int)(_score * _timeLimit / (_timeLimit - _timer));
        float finishTime = _timer;
        Debug.Log("addScore : " + addedTimeScore);
        float timer = 0;
        while (timer < 1.5)
        {
            timer += Time.deltaTime;
            _score += (int)((addedTimeScore - _score) * timer / 1.5f);
            _timer = finishTime - finishTime * timer / 1.5f;
            _scoreText.text = _score.ToString("000");
            _timerText.text = _timer.ToString("0.0");
            yield return new WaitForEndOfFrame();
        }
        _timerText.text = "0.0";
        yield return new WaitForSeconds(1);
        _returnLobbyButton.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}