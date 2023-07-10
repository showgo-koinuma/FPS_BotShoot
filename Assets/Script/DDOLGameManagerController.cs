using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DDOLGameManagerController : MonoBehaviour
{
    /// <summary>pause画面</summary>
    [SerializeField] GameObject _canvas;
    /// <summary>sens変更slider</summary>
    [SerializeField] Slider _sensSlider;
    [SerializeField] TextMeshProUGUI _sensText;
    /// <summary>sens変更用</summary>
    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    bool _isPause = false;
    float _sens;
    /// <summary>sens変更用</summary>
    CinemachinePOV _cinemachinePOV;

    void Awake()
    {
        _cinemachinePOV = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _sensSlider.value = _cinemachinePOV.m_VerticalAxis.m_MaxSpeed;
        _canvas.SetActive(_isPause);
        Cursor.visible = false; // カーソル非表示
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _isPause = !_isPause;
            Controll();
        }
    }

    void Controll()
    {
        _canvas.SetActive(_isPause);
        Cursor.visible = _isPause;
        if (_isPause)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
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
    }
}
