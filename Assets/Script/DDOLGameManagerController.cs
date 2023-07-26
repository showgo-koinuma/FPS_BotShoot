using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Pause�ASens�A�V�[���؂�ւ��ȂǃQ�[���S�̂�Manager
/// </summary>
public class DDOLGameManagerController : MonoBehaviour
{
    [Header("Pause���")]
    /// <summary>pause���</summary>
    [SerializeField] GameObject _canvas;
    /// <summary>sens�ύXslider</summary>
    [SerializeField] Slider _sensSlider;
    [SerializeField] TextMeshProUGUI _sensText;
    [Space(5)]
    [Header("�v���C���[�ݒ�")]
    /// <summary>sens�ύX�p</summary>
    [SerializeField] CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] Transform _playerTransform;
    [SerializeField] Vector3 _playerInitialPosition;
    bool _isPause = false;
    float _sens;
    /// <summary>sens�ύX�p</summary>
    CinemachinePOV _cinemachinePOV;

    void Awake()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        _cinemachinePOV = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        _sensSlider.value = _cinemachinePOV.m_VerticalAxis.m_MaxSpeed;
        _canvas.SetActive(_isPause);
        // �J�[�\���֘A
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            _isPause = !_isPause;
            Pause();
        }
    }

    /// <summary>Pause�{�^�����������Ƃ��Ɏ��s</summary>
    void Pause()
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

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        // Player��Position���Z�b�g
        _playerTransform.position = _playerInitialPosition;
    }

    public void SetSens()
    {
        _sens = _sensSlider.value;
        _sensText.text = _sens.ToString("0.00");
        _cinemachinePOV.m_VerticalAxis.m_MaxSpeed = _sens;
        _cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = _sens;
    }

    /// <summary>�Q�[���I�����̏���</summary>
    private void OnApplicationQuit()
    {
        Cursor.visible = true; // �J�[�\����\��
    }
}
