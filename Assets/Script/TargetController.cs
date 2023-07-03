using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// タグがTargetのゲームオブジェクトのController
/// </summary>
public class TargetController : MonoBehaviour
{
    [SerializeField] float _hp = 100;
    /// <summary>[0]:body [1]:head となるように</summary>
    Collider[] _targetCollider;
    float _headMagnification = 2.5f;
    /// <summary>ラストヒットが頭だったかどうか</summary>
    bool _lastHitHead = false;

    private void Start()
    {
        _targetCollider = GetComponents<Collider>();
    }

    private void Update()
    {
        if (_hp <= 0) // hpが0を下回った(死んだ)ときの処理
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerController>().AddKillScore(_lastHitHead);
            Destroy(gameObject);
        }
    }


    public void OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _targetCollider[1]) // 頭に当たったとき
        {
            _hp -= damage * _headMagnification;
            if (_hp <= 0)
            {
                _lastHitHead = true;
            }
        }
        else // それ以外に当たったとき
        {
            _hp -= damage;
        }
    }
}
