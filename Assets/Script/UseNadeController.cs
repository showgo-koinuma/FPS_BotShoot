using UnityEngine;
using UnityEngine.UI;

public class UseNadeController : MonoBehaviour
{
    [SerializeField] GameObject _nadeObject;
    [SerializeField] float _coolDown = 3f;
    [SerializeField] Image _skillCDImage;
    float coolDownTimer = 10;
    bool _isCD = false;

    void Update()
    {
        if (_isCD)
        {
            coolDownTimer += Time.deltaTime;
            _skillCDImage.fillAmount = 1 - coolDownTimer / _coolDown;
        }
        if (coolDownTimer >= _coolDown)
        {
            _isCD = false;
            if (Input.GetButtonDown("Fire2"))
            {
                Instantiate(_nadeObject).transform.position = Camera.main.transform.position + Camera.main.transform.forward;
                coolDownTimer = 0;
                _isCD = true;
            }
        }
    }
}
