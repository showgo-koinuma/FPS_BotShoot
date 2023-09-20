using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>�e�������̃R���|�[�l���g</summary>
public class GunController : MonoBehaviour
{
    [Header("�e�̐��\")]
    /// <summary>�ő�}�K�W���e��</summary>
    [SerializeField] int _maxBullets = 30;
    /// <summary>�����[�h����</summary>
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    /// <summary>�A�ˊԊu</summary>
    [SerializeField] float _shootInterval = 0.1f;
    [SerializeField] float _recoilSize = 1f;
    [Space(5)]
    [Header("���R�C���p")]
    /// <summary>���R�C���p</summary>
    [SerializeField] CinemachineVirtualCamera _cam;
    [Space(5)]
    [Header("�e�L�X�g")]
    /// <summary>�ő�}�K�W���e�ʃe�L�X�g</summary>
    [SerializeField] TextMeshProUGUI _maxBulletsText;
    /// <summary>�c�e�\���e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI _remainingBulletsText;
    [Header("�G�t�F�N�g�֘A")]
    [SerializeField] AudioClip _shootSound;
    [SerializeField] AudioClip[] _reloadSounds;
    [SerializeField] ParticleSystem _muzzleFlashParticles;
    /// <summary>Target�ȊO��hit�����Ƃ��̃G�t�F�N�g</summary>
    [SerializeField] GameObject[] _hitEffectPrefab;
    [SerializeField] GameObject _hitUIEffects;
 
    /// <summary>�c�e</summary>
    int _remainingBullets;
    /// <summary>�e�̍ő僌���W</summary>
    float _maxShootRange = 100;
    AudioSource _audioSource;
    /// <summary>�N���X�w�A�̃q�b�g����Animator</summary>
    Animator _crosshairAnimator;
    /// <summary>�q�b�g�G�t�F�N�g�̐F�ύX�p</summary>
    RawImage[] _hitUIEffectImages;
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
        _audioSource = GetComponent<AudioSource>();
        _crosshairAnimator = _hitUIEffects.GetComponent<Animator>();
        _hitUIEffectImages = _hitUIEffects.GetComponentsInChildren<RawImage>();
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
        //���C���΂��āA�q�b�g�����I�u�W�F�N�g�̏��𓾂�
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, _maxShootRange, ~(1<<6))) // �����ɓ��������Ƃ�
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.TryGetComponent(out TargetController component))
            {
                Color color = new Color(1, 1, 1, 1);
                if (component.OnHit(_damage, hit.collider))// OnHit���Ă�
                {
                    color = new Color(0.9f, 0, 0, 1);
                }

                foreach (RawImage child in _hitUIEffectImages)
                {
                    child.color = color;
                }

                _crosshairAnimator.Play("HitUIAnimation");
            }
            else
            {
                foreach (GameObject effect in _hitEffectPrefab)
                {
                    Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        _audioSource.PlayOneShot(_shootSound); // ���ˉ��Đ�
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
        DOVirtual.DelayedCall(0.3f, () => { _audioSource.PlayOneShot(_reloadSounds[0]); });
        DOVirtual.DelayedCall(1.3f, () => { _audioSource.PlayOneShot(_reloadSounds[1]); });
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
