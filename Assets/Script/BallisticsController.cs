using Cinemachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>�e�������̃R���|�[�l���g</summary>
public class BallisticsController : MonoBehaviour
{
    [SerializeField] int _maxBullets = 30;
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    [SerializeField] float _shootInterval = 0.1f;
    [SerializeField] float _recoilSize = 3f;
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] TextMeshProUGUI _maxBulletsText; // �}�K�W���T�C�Y�e�L�X�g
    [SerializeField] TextMeshProUGUI _remainingBulletsText; // �c�e�\���i���j
    int _remainingBullets;
    float _maxShootRange = 100;
    bool _canShoot = true;
    CinemachinePOV _cinemachinePOV;
    Animator _animator;

    private void Start()
    {
        _remainingBullets = _maxBullets;
        _maxBulletsText.text = "/ " + _maxBullets.ToString();
        _cinemachinePOV = _cam.GetCinemachineComponent<CinemachinePOV>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && _canShoot)
        {
            if (_remainingBullets > 0)
            {
                Shoot();
            }
            else // �c�e0�̂Ƃ��̓����[�h�ɓ���
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets && _canShoot)
        {
            StartCoroutine(nameof(Reload)); // �����[�h���Ă�
        }


        // �c�e�\���i���j
        _remainingBulletsText.text = _remainingBullets.ToString();
    }

    /// <summary>�e������</summary>
    void Shoot()
    {
        RaycastHit hit;
        //���C���΂��āA�q�b�g�����I�u�W�F�N�g�̏��𓾂�
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(_damage, hit.collider); // �q�b�g�����I�u�W�F�N�g��OnHit���Ă�
            }
        }
        //_cam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value -= _recoilSize;
        StartCoroutine(nameof(Recoil));
        _remainingBullets--;
        StartCoroutine(nameof(RapidFire));
    }

    /// <summary>�����[�h����</summary>
    IEnumerator Reload()
    {
        Debug.Log("�����[�h");
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _remainingBullets = _maxBullets;
    }

    /// <summary>�A�ˑ��x����</summary>
    IEnumerator RapidFire()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true; // _shootInterval�b�����҂���true��Ԃ�
    }

    /// <summary>���R�C������</summary>��
    IEnumerator Recoil()
    {
        float timer = 0;
        float horizontalRecoil = Random.Range(-0.05f, 0.05f);
        _animator.Play("AssaultRifleRecoilAnimator");
        while (true)
        {
            timer += Time.deltaTime;
            _cinemachinePOV.m_VerticalAxis.Value -= _recoilSize * Time.deltaTime / 0.09f;
            _cinemachinePOV.m_HorizontalAxis.Value += horizontalRecoil * Time.deltaTime / 0.09f;
            yield return new WaitForEndOfFrame();
            if (timer > 0.09) { yield break; }
        }
    }
}
