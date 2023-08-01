using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// <summary>�ŏ���Collider�͏����n�_�e�����蔲���Ȃ��p</summary>
    Collider[] _waveManageCollider;
    Animator _timerTextAnimator;
    bool _inGame = false; // �Q�[������
    /// <summary>���݂�wave��</summary>
    int _waveCount = 0;
    /// <summary>bot�̍ŏ��̐�</summary>
    int _numOfBot;
    /// <summary>bot�̌��݂̐�</summary>
    int _botCount;
    int _score = 0;
    float _timer;

    private void Start()
    {
        // ���ԁA�X�R�A�Abot�̐��̏����ݒ�
        _scoreText.text = _score.ToString("000");
        _timer = _timeLimit;
        _timerText.text = _timer.ToString("0.0");
        _timerTextAnimator = _timerText.gameObject.GetComponent<Animator>();

        // �Q�[���I������UI��\��
        _finishPanel.SetActive(false);
        _finishText.SetActive(false);
        _returnLobbyButton.SetActive(false);

        // wave�Ǘ��n
        _waveManageCollider = GetComponentsInChildren<Collider>();
        NextWave(); // 1st wave�����z�u
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
            _numOfBot = _enemyWave[_waveCount].GetComponentsInChildren<EnemyController>().Length; // bot�����J�E���g
            _waveCount++;
            _waveManageCollider[_waveCount].enabled = false; // ���X�e�[�W�ւ̓����J����
            // todo:UI�X�V
            _botCount = _numOfBot;
            _botCountText.text = $"{_botCount} / {_numOfBot}";
        }
        else // wave�����������̂ŃQ�[���I���̏���
        {
            _inGame = false;
            StartCoroutine(nameof(Finish));
        }
    }

    /// <summary>�L�����̃|�C���g���Z</summary>
    /// <param name="lastHitHead"></param>
    public void KillAddScore(int addScore)
    {
        _score += addScore;
        _addScoreText.text = $"+{addScore}";
        _addScoreText.gameObject.GetComponent<Animator>().Play("AddScoreTextAnimation", 0, 0); // �X�R�A�����Z�����G�t�F�N�g
        _botCount--;
        _scoreText.text = _score.ToString("000");
        _botCountText.text = $"{_botCount} / {_numOfBot}";
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

    private void OnTriggerEnter(Collider other) // �X�^�[�g�����trigger
    {
        _inGame = true;
        _waveManageCollider[0].enabled = false;
        _waveManageCollider[1].enabled = false;
    }
}