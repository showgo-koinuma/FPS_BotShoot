using UnityEngine;

public class RandomRespawnBotController : TargetController
{
    [SerializeField] float _hp = 100;
    /// <summary>ヘッドショット倍率</summary>
    [SerializeField] float _headMagnification = 2.5f;
    [SerializeField] Vector3[] _respawnRange = new Vector3[2];
    /// <summary>[0]:body [1]:head となるように</summary>
    Collider[] _enemyCollider;

    private void Start()
    {
        _enemyCollider = GetComponents<Collider>();
    }

    public override bool OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _enemyCollider[1]) // 頭に当たったとき
        {
            _hp -= damage * _headMagnification;
            if (_hp <= 0) // hpが0になったら
            {
                Respawn();
            }
            return true;
        }
        else // それ以外に当たったとき
        {
            _hp -= damage;
            if (_hp <= 0) // hpが0になったら
            {
                Respawn();
            }
            return false;
        }
    }

    void Respawn()
    {
        _hp = 100;
        transform.position = new Vector3(Random.Range(_respawnRange[0].x, _respawnRange[1].x), _respawnRange[0].y, Random.Range(_respawnRange[0].z, _respawnRange[1].z));
    }
}
