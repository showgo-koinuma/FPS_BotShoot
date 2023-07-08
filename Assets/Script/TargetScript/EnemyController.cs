using UnityEngine;

/// <summary>
/// タグがTargetのゲームオブジェクトのController
/// </summary>
public class EnemyController : TargetController
{
    [SerializeField] float _hp = 100;
    /// <summary>ヘッドショット倍率</summary>
    [SerializeField] float _headMagnification = 2.5f;
    [SerializeField] int _killScore = 50;
    /// <summary>[0]:body [1]:head となるように</summary>
    Collider[] _enemyCollider;
    /// <summary>ラストヒットが頭だったかどうか</summary>
    bool _lastHitHead = false;

    private void Start()
    {
        _enemyCollider = GetComponents<Collider>();
    }

    public override void OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _enemyCollider[1]) // 頭に当たったとき
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

        if (_hp <= 0) // hpが0になったら
        {
            IsDead();
        }
    }

    void IsDead()
    {
        if (_lastHitHead) { _killScore *= 2; }
        GameObject.FindGameObjectWithTag("GameController").GetComponent<StageGameManagerController>().Killed(_killScore);
        Destroy(gameObject);
    }
}
