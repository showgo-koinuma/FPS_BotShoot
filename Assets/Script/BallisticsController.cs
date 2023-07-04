using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float _shootInterval = 0.12f;
    [SerializeField] float _recoilSize = 1f;
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] TextMeshProUGUI _maxBulletsText; // マガジンサイズテキスト
    [SerializeField] TextMeshProUGUI _remainingBulletsText; // 残弾表示（仮）
    int _remainingBullets;
    float _maxShootRange = 100;
    bool _canShoot = true;
    bool _reloading;

    private void Start()
    {
        _remainingBullets = _maxBullets;
        _maxBulletsText.text = "/ " + _maxBullets.ToString();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && _canShoot)
        {
            if (_remainingBullets > 0)
            {
                Shoot();
            }
            else if (!_reloading) // 残弾0のときはリロードに入る
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets && !_reloading)
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
        _cam.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.Value -= _recoilSize;
        _remainingBullets--;
        StartCoroutine(nameof(RapidFire));
    }

    /// <summary>リロード処理</summary>
    IEnumerator Reload()
    {
        Debug.Log("リロード");
        _reloading = true;
        _canShoot = false;
        yield return new WaitForSeconds(_reloadTime);
        _canShoot = true;
        _remainingBullets = _maxBullets;
        _reloading = false;
    }

    /// <summary>連射速度処理</summary>
    IEnumerator RapidFire()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_shootInterval);
        _canShoot = true; // _shootInterval秒だけ待ってtrueを返す
    }
}
