using Cinemachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

/// <summary>�e�������̃R���|�[�l���g</summary>
public class GunController : MonoBehaviour
{
    /// <summary>�ő�}�K�W���e��</summary>
    [SerializeField] int _maxBullets = 30;
    /// <summary>�����[�h����</summary>
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    /// <summary>�A�ˊԊu</summary>
    [SerializeField] float _shootInterval = 0.1f;
    [SerializeField] float _recoilSize = 3f;
    /// <summary>���R�C���p</summary>
    [SerializeField] CinemachineVirtualCamera _cam;
    /// <summary>�ő�}�K�W���e�ʃe�L�X�g</summary>
    [SerializeField] TextMeshProUGUI _maxBulletsText;
    /// <summary>�c�e�\���e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI _remainingBulletsText;
    [SerializeField] ParticleSystem _muzzleFlashParticles;
    /// <summary>Target�ȊO��hit�����Ƃ��̃G�t�F�N�g</summary>
    [SerializeField] GameObject[] _hitEffectPrefab;

    /// <summary>�c�e</summary>
    int _remainingBullets;
    /// <summary>�e�̍ő僌���W</summary>
    float _maxShootRange = 100;
    /// <summary>���R�C���p</summary>
    CinemachinePOV _cinemachinePOV;
    Animator _animator;
    /// <summary>�e�̏��</summary>
    GunState _gunState;

    private void Start()
    {
        _remainingBullets = _maxBullets;
        _maxBulletsText.text = "/ " + _maxBullets.ToString();
        _cinemachinePOV = _cam.GetCinemachineComponent<CinemachinePOV>();
        _animator = GetComponent<Animator>();
        _gunState = GunState.Normal;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && _gunState == GunState.Normal)
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
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets)
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange)) // �����ɓ��������Ƃ�
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Target") // Target�ɓ��������Ƃ�
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(_damage, hit.collider); // OnHit���Ă�
            }
            else
            {
                foreach (GameObject effect in _hitEffectPrefab)
                {
                    Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        _muzzleFlashParticles.Play(); // �}�Y���t���b�V��
        _remainingBullets--; // �c�e�����炷
        StartCoroutine(nameof(RapidFire));
        StartCoroutine(nameof(Recoil));
    }

    /// <summary>�����[�h����</summary>
    IEnumerator Reload()
    {
        _gunState = GunState.Reloading;
        _animator.Play("ReloadAnimator");
        yield return new WaitForSeconds(_reloadTime);
        _gunState = GunState.Normal;
        _remainingBullets = _maxBullets;
    }

    /// <summary>�A�ˑ��x����</summary>
    IEnumerator RapidFire()
    {
        _gunState = GunState.ShootIntervalTime;
        yield return new WaitForSeconds(_shootInterval);
        if (_gunState != GunState.Reloading)
        {
            _gunState = GunState.Normal;
        }
    }

    /// <summary>���R�C������</summary>��
    IEnumerator Recoil()
    {
        float timer = 0;
        float horizontalRecoil = Random.Range(-0.05f, 0.05f);
        _animator.Play("AssaultRifleRecoilAnimator", 0, 0);
        while (true)
        {
            timer += Time.deltaTime;
            _cinemachinePOV.m_VerticalAxis.Value -= _recoilSize * Time.deltaTime / 0.09f;
            _cinemachinePOV.m_HorizontalAxis.Value += horizontalRecoil * Time.deltaTime / 0.09f;
            yield return new WaitForEndOfFrame();
            if (timer > 0.09) { yield break; }
        }
    }

    /// <summary>�e�̏�Ԃ�\��</summary>
    enum GunState
    {
        Normal,
        ShootIntervalTime,
        Reloading,
    }
}