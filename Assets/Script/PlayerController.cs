using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𓮂����R���|�[�l���g
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _jumpPower = 10;
    Rigidbody _rb;
    /// <summary>�ڒn����̋���</summary>
    float _isGroundedLength;
    /// <summary>�󒆂ł̕����]���̃X�s�[�h</summary>
    float _turnSpeed = 3;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isGroundedLength = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
    }

    void Update()
    {
        // �����ړ�����
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, v); // �ړ�����
        dir = Camera.main.transform.TransformDirection(dir); //�J������̃x�N�g���ɒ���
        dir.y = 0;
        dir = dir.normalized;
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround())
        {
            // �󒆂ł����������]�����\
            velo = _rb.velocity;
            if (!(dir.magnitude == 0f))
            {
                // ���x�̑傫����ێ����Ȃ���������������ς���
                Vector2 airDir = Vector2.Lerp(new Vector2(_rb.velocity.x, _rb.velocity.z), new Vector2(dir.x, dir.z) * _rb.velocity.magnitude, Time.deltaTime * _turnSpeed); // Slerp�g����magnitude���ێ��ł��邯��...
                velo = new Vector3(airDir.x, _rb.velocity.y, airDir.y);
            }
            _rb.velocity = velo;
        }
        else
        {
            _rb.velocity = velo;
        }

        // Jump����
        if (Input.GetButtonDown("Jump") && IsGround())
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }

    /// <summary>�n�ʂɐڒn���Ă��邩�𔻒肷��</summary>
    /// <returns></returns>
    bool IsGround()
    {
        Vector3 start = transform.position;
        Vector3 end = start + Vector3.down * _isGroundedLength;
        Debug.DrawLine(start, end);
        bool isGround = Physics.Linecast(start, end);
        return isGround;
    }
}
