using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

/// <summary>弾道処理のコンポーネント</summary>
public class BallisticsController : MonoBehaviour
{
    [SerializeField] int _maxBullets = 30;
    [SerializeField] float _reloadTime = 2;
    [SerializeField] GameObject _text;
    int _remainingBullets;
    float _maxShootRange = 100;
    bool _canShoot = true;
    bool _reloading;
    float _shootInterval = 0.12f;

    private void Start()
    {
        _remainingBullets = _maxBullets;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && _canShoot)
        {
            if (_remainingBullets > 0)
            {
                Shoot();
                StartCoroutine(nameof(RapidFire));
                _remainingBullets--;
            }
            else if (!_reloading)
            {
                StartCoroutine(nameof(Reload));
            }
        }
        if (Input.GetButtonDown("Reload") && _remainingBullets < _maxBullets && !_reloading)
        {
            StartCoroutine(nameof(Reload));
        }

        _text.GetComponent<UnityEngine.UI.Text>().text = _remainingBullets.ToString();
    }

    /// <summary>弾道処理</summary>
    void Shoot()
    {
        RaycastHit hit;
        //レイを飛ばして、ヒットしたオブジェクトの情報を得る
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _maxShootRange))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Target")
            {
                TargetController target = hit.collider.gameObject.GetComponent<TargetController>();
                target.OnHit(); // ヒットしたオブジェクトのOnHitを呼ぶ
            }
        }
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
