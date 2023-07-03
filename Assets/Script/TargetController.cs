using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �^�O��Target�̃Q�[���I�u�W�F�N�g��Controller
/// </summary>
public class TargetController : MonoBehaviour
{
    [SerializeField] float _hp = 100;
    /// <summary>[0]:body [1]:head �ƂȂ�悤��</summary>
    Collider[] _targetCollider;
    float _headMagnification = 2.5f;
    /// <summary>���X�g�q�b�g�������������ǂ���</summary>
    bool _lastHitHead = false;

    private void Start()
    {
        _targetCollider = GetComponents<Collider>();
    }

    private void Update()
    {
        if (_hp <= 0) // hp��0���������(����)�Ƃ��̏���
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerController>().AddKillScore(_lastHitHead);
            Destroy(gameObject);
        }
    }


    public void OnHit(float damage, Collider hitCollider)
    {
        if (hitCollider == _targetCollider[1]) // ���ɓ��������Ƃ�
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
    }
}
