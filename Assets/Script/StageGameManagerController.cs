using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageGameManagerController : MonoBehaviour
{
    [Header("WAVE管理")]
    [SerializeField] GameObject[] _enemyWave;
    [SerializeField] TextMeshProUGUI _waveUI;
    [Header("時間関係")]
    /// <summary>制限時間</summary>
    [SerializeField] int _timeLimit = 60;
    [SerializeField] TextMeshProUGUI _timerText;
    [Header("スコア、botテキスト")]
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _addScoreText;
    [SerializeField] TextMeshProUGUI _botCountText;
    [Header("ゲーム終了時UI")]
    [SerializeField] GameObject _finishPanel;
    [SerializeField] GameObject _finishText;
    [SerializeField] GameObject _returnLobbyButton;
    [Space(10)]
    [SerializeField] AudioClip _killSound;
    [SerializeField] AudioClip _startSound;
    [SerializeField] AudioClip _nextWaveSound;
    /// <summary>最初のColliderは初期地点弾がすり抜けない用</summary>
    Collider[] _waveManageCollider;
    Animator _timerTextAnimator;
    bool _inGame = false; // ゲーム中か
    /// <summary>現在のwave数</summary>
    int _waveCount = 0;
    /// <summary>botの最初の数</summary>
    //int _numOfBot;
    /// <summary>botの現在の数</summary>
    int _botCount;
    int _score = 0;
    float _timer;

    private void Start()
    {
        // 時間、スコア、botの数の初期設定
        _scoreText.text = _score.ToString("00000");
        _timer = _timeLimit;
        _timerText.text = _timer.ToString("0.0");
        _timerTextAnimator = _timerText.gameObject.GetComponent<Animator>();

        // ゲーム終了時のUI非表示
        _finishPanel.SetActive(false);
        _finishText.SetActive(false);
        _returnLobbyButton.SetActive(false);

        // wave管理系
        _waveManageCollider = GetComponentsInChildren<Collider>();
        Debug.Log(_waveManageCollider.Length);
    }

    private void Update()
    {
        if (_inGame)
        {
            _timer -= Time.deltaTime;


            if (_timer <= 10) // 残り時間が10秒を切ると時間UIがアニメーションする
            {
                _timerTextAnimator.Play("TimerColorAnimation");
                if (_timer <= 0) // 残り時間0でゲーム終了
                {
                    _inGame = false;
                    StartCoroutine(nameof(Finish));
                }
            }

            if (_botCount == 0) // botが全滅したら次waveへ
            {
                NextWave();
            }

            _timerText.text = _timer.ToString("0.0"); // 時間表示
        }
    }

    /// <summary>次のwaveに行く</summary>
    void NextWave()
    {
        if (_waveCount < _enemyWave.Length) // 次のwaveへ
        {
            Instantiate(_enemyWave[_waveCount]); // wave出現
            _botCount = _enemyWave[_waveCount].GetComponentsInChildren<EnemyController>().Length; // bot数をカウント
            _waveCount++;
            _waveManageCollider[_waveCount].gameObject.SetActive(false); // 次ステージへの道を開ける
            _botCountText.text = $"{_botCount}"; // bot数のテキスト更新
            _waveUI.text = $"{_waveCount}"; // wave数のテキスト更新
            if (_waveCount != 1) SystemSoundManager.instance.PlayOneShotClip(_nextWaveSound); // 最初以外に鳴らす
        }
        else // waveがもう無いのでゲーム終了の処理
        {
            _inGame = false;
            StartCoroutine(nameof(Finish));
        }
    }

    /// <summary>キル時のポイント加算</summary>
    public void KillAddScore(int addScore)
    {
        if (_inGame)
        {
            _score += addScore;
            _addScoreText.text = $"+{addScore}";
            _addScoreText.gameObject.GetComponent<Animator>().Play("AddScoreTextAnimation", 0, 0); // スコアが加算されるエフェクト
            _scoreText.text = _score.ToString("00000");
        }

        _botCount--;
        _botCountText.text = $"{_botCount}";
        SystemSoundManager.instance.PlayOneShotClip(_killSound);
    }

    /// <summary>button用ロビーに戻る</summary>
    public void ReturnLobby()
    {
        GameObject.Find("DDOLGameManager").GetComponent<ScoreManager>().SetScore(_score);
        SceneManager.LoadScene(0);
    }

    /// <summary>ゲーム終了時のUI処理</summary>
    IEnumerator Finish()
    {
        //表示：パネル、Finishテキスト
        _finishPanel.SetActive(true);
        _finishText.SetActive(true);
        DDOLGameManagerController.instans.InGame = false;
        SystemSoundManager.instance.PlayOneShotClip(_startSound);
        yield return new WaitForSeconds(1);

        //パネル拡大アニメーション
        _finishPanel.GetComponent<Animator>().Play("PanelAnimation");
        yield return new WaitForSeconds(1);

        //2つのテキストの移動アニメーション
        _finishText.GetComponent<Animator>().Play("FInishTextAnimation");
        _scoreText.gameObject.GetComponent<Animator>().Play("ScoreTextAnimation");
        yield return new WaitForSeconds(1.5f);

        int addedTimeScore = (int)(_score * _timeLimit / (_timeLimit - _timer));
        Debug.Log("Score : " + addedTimeScore);
        DOTween.To(() => _score, i => _scoreText.text = i.ToString("00000"), addedTimeScore, 1.5f).SetEase(Ease.OutCirc).OnComplete(() => _score = addedTimeScore);
        DOTween.To(() => _timer, i => _timerText.text = i.ToString("0.0"), 0, 1.5f);
        yield return new WaitForSeconds(2.5f);

        _returnLobbyButton.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other) // スタート判定はtrigger
    {
        Debug.Log("suta-to");
        _inGame = true;
        _waveManageCollider[0].enabled = false;
        NextWave(); // 1st wave初期配置
        SystemSoundManager.instance.PlayOneShotClip(_startSound);
    }
}