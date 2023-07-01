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
    [SerializeField] float _jumpPower = 15;
    Rigidbody _rb;
    float _isGroundedLength;
    float _airTurnSpeed = 3;

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
        dir = Camera.main.transform.TransformDirection(dir).normalized;
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround())
        {
            velo = dir * velo.magnitude;
            velo.y = _rb.velocity.y;
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
