using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>弾道処理のコンポーネント</summary>
public class GunController : MonoBehaviour
{
    /// <summary>最大マガジン容量</summary>
    [SerializeField] int _maxBullets = 30;
    /// <summary>リロード時間</summary>
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    /// <summary>連射間隔</summary>
    [SerializeField] float _shootInterval = 0.1f;
    [SerializeField] float _recoilSize = 3f;
    /// <summary>リコイル用</summary>
    [SerializeField] CinemachineVirtualCamera _cam;
    /// <summary>最大マガジン容量テキスト</summary>
    [SerializeField] TextMeshProUGUI _maxBulletsText;
    /// <summary>残弾表示テキスト</summary>
    [SerializeField] TextMeshProUGUI _remainingBulletsText;
    [SerializeField] ParticleSystem _muzzleFlashParticles;
    /// <summary>Target以外にhitしたときのエフェクト</summary>
    [SerializeField] GameObject[] _hitEffectPrefab;
    [SerializeField] Animator _hitUIEffect;
 
    /// <summary>残弾</summary>
    int _remainingBullets;
    /// <summary>弾の最大レンジ</summary>
    float _maxShootRange = 100;
    /// <summary>リコイル用</summary>
    CinemachinePOV _cinemachinePOV;
    Animator _animator;
    /// <summary>銃の状態</summary>
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
            else // 残弾0のときはリロードに入る
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets)
        {
            StartCoroutine(nameof(Reload)); // リロードを呼ぶ
        }


        // 残弾表示（仮）
        _remainingBulletsText.text = _remainingBullets.ToString();
    }

    /// <summary>弾道処理</summary>
    void Shoot()
    {
        RaycastHit hit;
        //レイを飛ばして、ヒットしたオブジェクトの情報を得る
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange)) // 何かに当たったとき
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Target") // Targetに当たったとき
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(_damage, hit.collider); // OnHitを呼ぶ
                _hitUIEffect.Play("HitUIAnimation");

            }
            else
            {
                foreach (GameObject effect in _hitEffectPrefab)
                {
                    Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
        _muzzleFlashParticles.Play(); // マズルフラッシュ
        _remainingBullets--; // 残弾を減らす
        StartCoroutine(nameof(RapidFire));
        StartCoroutine(nameof(Recoil));
    }

    /// <summary>リロード処理</summary>
    IEnumerator Reload()
    {
        _gunState = GunState.Reloading;
        _animator.Play("ReloadAnimator");
        yield return new WaitForSeconds(_reloadTime);
        _gunState = GunState.Normal;
        _remainingBullets = _maxBullets;
    }

    /// <summary>連射速度処理</summary>
    IEnumerator RapidFire()
    {
        _gunState = GunState.ShootIntervalTime;
        yield return new WaitForSeconds(_shootInterval);
        if (_gunState != GunState.Reloading)
        {
            _gunState = GunState.Normal;
        }
    }

    /// <summary>リコイル処理</summary>￥
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

    /// <summary>銃の状態を表す</summary>
    enum GunState
    {
        Normal,
        ShootIntervalTime,
        Reloading,
    }
}
