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
    bool _inGame = true;
    int _botCount;
    int _score = 0;
    float _timer;
    /// <summary>残り時間をスコアに変換するレート</summary>
    int _timeToScoreRate = 10;

    private void Start()
    {
        _scoreText.text = _score.ToString("000");
        _timer = _timeLimit;
        _botCount = GameObject.FindObjectsOfType<EnemyController>().Length;
        _botCountText.text = _botCount.ToString();
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
                _timerText.gameObject.GetComponent<Animator>().Play("TimerColorAnimation");
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
        _botCountText.text = _botCount.ToString();
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
        float timer = 0;
        RectTransform transform = _finishPanel.GetComponent<RectTransform>();
        while (timer <= 1)
        {
            timer += Time.deltaTime;
            float scaleY = 0.15f + 0.85f * timer / 1;
            float positionY = 70 - 70 * timer / 1;
            transform.localScale = new Vector3(1, scaleY, 1);
            transform.localPosition = new Vector3(0, positionY, 0);
            yield return new WaitForEndOfFrame();
        }
        timer = 1;
        RectTransform scoreTextTransform = _scoreText.gameObject.GetComponent<RectTransform>();
        RectTransform finishTextTransform = _finishText.GetComponent<RectTransform>();
        float scorePosX = scoreTextTransform.position.x;
        float scorePosY = scoreTextTransform.position.y;
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            scorePosX = 320 * timer / 1;
            scorePosY = 50 + 145 * timer / 1;
            float finishPosY = 140 - 70 * timer / 1;
            finishTextTransform.localPosition = new Vector3(0, finishPosY, 0);
            scoreTextTransform.localPosition = new Vector3(scorePosX, scorePosY, 0);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        timer = 0;
        int addedTimeScore = _score + (int)_timer * _timeToScoreRate;
        float finishTime = _timer;
        Debug.Log("addScore : " + addedTimeScore);
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