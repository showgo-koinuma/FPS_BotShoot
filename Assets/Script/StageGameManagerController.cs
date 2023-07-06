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
        Cursor.visible = false; // �J�[�\����\��
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\������ʓ��ɗ��߂�
    }

    /// <summary>�|�C���g���Z</summary>
    /// <param name="lastHitHead"></param>
    public void AddScore(int addScore)
    {
        _score += addScore;
        _scoreText.text = _score.ToString("000");
    }

    /// <summary>�Q�[���I�����̏���</summary>
    private void OnApplicationQuit()
    {
        Cursor.visible = true; // �J�[�\����\��
    }
}
