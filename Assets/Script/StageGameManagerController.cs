using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageGameManagerController : MonoBehaviour
{
    [Header("WAVE�Ǘ�")]
    [SerializeField] GameObject[] _enemyWave;
    [SerializeField] TextMeshProUGUI _waveUI;
    [Header("���Ԋ֌W")]
    /// <summary>��������</summary>
    [SerializeField] int _timeLimit = 60;
    [SerializeField] TextMeshProUGUI _timerText;
    [Header("�X�R�A�Abot�e�L�X�g")]
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _addScoreText;
    [SerializeField] TextMeshProUGUI _botCountText;
    [Header("�Q�[���I����UI")]
    [SerializeField] GameObject _finishPanel;
    [SerializeField] GameObject _finishText;
    [SerializeField] GameObject _returnLobbyButton;
    [Space(10)]
    [SerializeField] AudioClip _killSound;
    [SerializeField] AudioClip _startSound;
    [SerializeField] AudioClip _nextWaveSound;
    /// <summary>�ŏ���Collider�͏����n�_�e�����蔲���Ȃ��p</summary>
    Collider[] _waveManageCollider;
    Animator _timerTextAnimator;
    bool _inGame = false; // �Q�[������
    /// <summary>���݂�wave��</summary>
    int _waveCount = 0;
    /// <summary>bot�̍ŏ��̐�</summary>
    //int _numOfBot;
    /// <summary>bot�̌��݂̐�</summary>
    int _botCount;
    int _score = 0;
    float _timer;

    private void Start()
    {
        // ���ԁA�X�R�A�Abot�̐��̏����ݒ�
        _scoreText.text = _score.ToString("00000");
        _timer = _timeLimit;
        _timerText.text = _timer.ToString("0.0");
        _timerTextAnimator = _timerText.gameObject.GetComponent<Animator>();

        // �Q�[���I������UI��\��
        _finishPanel.SetActive(false);
        _finishText.SetActive(false);
        _returnLobbyButton.SetActive(false);

        // wave�Ǘ��n
        _waveManageCollider = GetComponentsInChildren<Collider>();
        Debug.Log(_waveManageCollider.Length);
    }

    private void Update()
    {
        if (_inGame)
        {
            _timer -= Time.deltaTime;


            if (_timer <= 10) // �c�莞�Ԃ�10�b��؂�Ǝ���UI���A�j���[�V��������
            {
                _timerTextAnimator.Play("TimerColorAnimation");
                if (_timer <= 0) // �c�莞��0�ŃQ�[���I��
                {
                    _inGame = false;
                    StartCoroutine(nameof(Finish));
                }
            }

            if (_botCount == 0) // bot���S�ł����玟wave��
            {
                NextWave();
            }

            _timerText.text = _timer.ToString("0.0"); // ���ԕ\��
        }
    }

    /// <summary>����wave�ɍs��</summary>
    void NextWave()
    {
        if (_waveCount < _enemyWave.Length) // ����wave��
        {
            Instantiate(_enemyWave[_waveCount]); // wave�o��
            _botCount = _enemyWave[_waveCount].GetComponentsInChildren<EnemyController>().Length; // bot�����J�E���g
            _waveCount++;
            _waveManageCollider[_waveCount].gameObject.SetActive(false); // ���X�e�[�W�ւ̓����J����
            _botCountText.text = $"{_botCount}"; // bot���̃e�L�X�g�X�V
            _waveUI.text = $"{_waveCount}"; // wave���̃e�L�X�g�X�V
            if (_waveCount != 1) SystemSoundManager.instance.PlayOneShotClip(_nextWaveSound); // �ŏ��ȊO�ɖ炷
        }
        else // wave�����������̂ŃQ�[���I���̏���
        {
            _inGame = false;
            StartCoroutine(nameof(Finish));
        }
    }

    /// <summary>�L�����̃|�C���g���Z</summary>
    public void KillAddScore(int addScore)
    {
        if (_inGame)
        {
            _score += addScore;
            _addScoreText.text = $"+{addScore}";
            _addScoreText.gameObject.GetComponent<Animator>().Play("AddScoreTextAnimation", 0, 0); // �X�R�A�����Z�����G�t�F�N�g
            _scoreText.text = _score.ToString("00000");
        }

        _botCount--;
        _botCountText.text = $"{_botCount}";
        SystemSoundManager.instance.PlayOneShotClip(_killSound);
    }

    /// <summary>button�p���r�[�ɖ߂�</summary>
    public void ReturnLobby()
    {
        GameObject.Find("DDOLGameManager").GetComponent<ScoreManager>().SetScore(_score);
        SceneManager.LoadScene(0);
    }

    /// <summary>�Q�[���I������UI����</summary>
    IEnumerator Finish()
    {
        //�\���F�p�l���AFinish�e�L�X�g
        _finishPanel.SetActive(true);
        _finishText.SetActive(true);
        DDOLGameManagerController.instans.InGame = false;
        SystemSoundManager.instance.PlayOneShotClip(_startSound);
        yield return new WaitForSeconds(1);

        //�p�l���g��A�j���[�V����
        _finishPanel.GetComponent<Animator>().Play("PanelAnimation");
        yield return new WaitForSeconds(1);

        //2�̃e�L�X�g�̈ړ��A�j���[�V����
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

    private void OnTriggerEnter(Collider other) // �X�^�[�g�����trigger
    {
        Debug.Log("suta-to");
        _inGame = true;
        _waveManageCollider[0].enabled = false;
        NextWave(); // 1st wave�����z�u
        SystemSoundManager.instance.PlayOneShotClip(_startSound);
    }
}