using Cinemachine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>弾道処理のコンポーネント</summary>
public class BallisticsController : MonoBehaviour
{
    [SerializeField] int _maxBullets = 30;
    [SerializeField] float _reloadTime = 2;
    [SerializeField] int _damage = 20;
    [SerializeField] float _shootInterval = 0.1f;
    [SerializeField] float _recoilSize = 3f;
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] TextMeshProUGUI _maxBulletsText; // マガジンサイズテキスト
    [SerializeField] TextMeshProUGUI _remainingBulletsText; // 残弾表示（仮）
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
            else // 残弾0のときはリロードに入る
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets && _canShoot)
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(_damage, hit.collider); // ヒットしたオブジェクトのOnHitを呼ぶ
            }
        }
        //_cam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value -= _recoilSize;
        StartCoroutine(nameof(Recoil));
        _remainingBullets--;
        StartCoroutine(nameof(RapidFire));
    }

    /// <summary>リロード処理</summary>
    IEnumerator Reload()
    {
        Debug.Log("リロード");
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _remainingBullets = _maxBullets;
    }

    /// <summary>連射速度処理</summary>
    IEnumerator RapidFire()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true; // _shootInterval秒だけ待ってtrueを返す
    }

    /// <summary>リコイル処理</summary>￥
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
