using UnityEngine;

/// <summary>
/// �^�O��Target�̃Q�[���I�u�W�F�N�g��Controller
/// </summary>
public class EnemyController : TargetController
{
    [SerializeField] float _hp = 100;
    /// <summary>�w�b�h�V���b�g�{��</summary>
    [SerializeField] float _headMagnification = 2.5f;
    [SerializeField] int _killScore = 50;
    /// <summary>[0]:body [1]:head �ƂȂ�悤��</summary>
    Collider[] _enemyCollider;
    /// <summary>���X�g�q�b�g�������������ǂ���</summary>
    bool _lastHitHead = false;

    private void Start()
    {
        _enemyCollider = GetComponents<Collider>();
    }

    public override void OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _enemyCollider[1]) // ���ɓ��������Ƃ�
        {
            _hp -= damage * _headMagnification;
            if (_hp <= 0)
            {
                _lastHitHead = true;
            }
        }
        else // ����ȊO�ɓ��������Ƃ�
        {
            _hp -= damage;
        }

        if (_hp <= 0) // hp��0�ɂȂ�����
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
