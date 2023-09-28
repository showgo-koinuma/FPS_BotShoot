using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause、Sens、シーン切り替えなどゲーム全体のManager
/// </summary>
public class DDOLGameManagerController : MonoBehaviour
{
    [Header("Pause画面")]
    /// <summary>pause画面</summary>
    [SerializeField] GameObject _canvas;
    /// <summary>sens変更slider</summary>
    [SerializeField] Slider _sensSlider;
    [SerializeField] TextMeshProUGUI _sensText;
    [SerializeField] AudioClip _pauseOpenSound;
    [SerializeField] AudioClip _pauseCloseSound;
    [Space(5)]
    [Header("プレイヤー設定")]
    /// <summary>sens変更用</summary>
    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Vector3 _playerInitialPosition;
    bool _isPause = false;
    float _sens;
    /// <summary>sens変更用</summary>
    CinemachinePOV _cinemachinePOV;
    public static DDOLGameManagerController instans;
    /// <summary>カーソル消えない用のin game判定</summary>
    public bool InGame { set; private get; } = true;

    void Awake()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        _cinemachinePOV = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _sensSlider.value = _cinemachinePOV.m_VerticalAxis.m_MaxSpeed;
        _canvas.SetActive(_isPause);
        // カーソル関連
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
        if (!instans) instans = this;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Pause();
        }
        
        if (_playerTransform.position.y < -10)
        {
            _playerTransform.position = _playerInitialPosition;
        }
    }

    /// <summary>Pauseボタンを押したときに実行</summary>
    public void Pause()
    {
        _isPause = !_isPause;
        _canvas.SetActive(_isPause);
        if (_isPause)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = _isPause;
            SystemSoundManager.instance.PlayOneShotClip(_pauseOpenSound);
        }
        else
        {
            Time.timeScale = 1;
            SystemSoundManager.instance.PlayOneShotClip(_pauseCloseSound);
            if (InGame)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = _isPause;
            }
        }
    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        // PlayerのPositionをセット
        _playerTransform.position = _playerInitialPosition;
        _cinemachinePOV.m_VerticalAxis.Value = 0;
        _cinemachinePOV.m_HorizontalAxis.Value = 0;
        // ゲーム中判定をリセット
        InGame = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = _isPause;
    }

    public void SetSens()
    {
        _sens = _sensSlider.value;
        _sensText.text = _sens.ToString("0.00");
        _cinemachinePOV.m_VerticalAxis.m_MaxSpeed = _sens;
        _cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = _sens;
    }

    /// <summary>ゲーム終了時の処理</summary>
    private void OnApplicationQuit()
    {
        Cursor.visible = true; // カーソルを表示
        ScoreManager.Instance.SaveRanking();
    }
}
