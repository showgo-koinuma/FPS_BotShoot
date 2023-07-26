using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

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
    //float _isGroundedLength = 1.1f;
    /// <summary>�󒆂ł̕����]���̃X�s�[�h</summary>
    float _turnSpeed = 3;
    bool _jumped;
    float _jumpedTimer;
    bool IsGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_isGroundedLength = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
    }

    void Update()
    {
        if (_jumped)
        {
            _jumpedTimer += Time.deltaTime;
            if (_jumpedTimer > 0.2f)
            {
                _jumped = false;
                _jumpedTimer = 0;
            }
            IsGround = false;
        }

        // �����ړ�����
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        Vector3 dir = new Vector3(h, 0, v); // �ړ�����
        dir = Camera.main.transform.TransformDirection(dir); //�J������̃x�N�g���ɒ���
        dir.y = 0;
        dir = dir.normalized;
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround)
        {
            // �󒆂ł����������]�����\
            velo = _rb.velocity;
            if (!(dir.magnitude == 0f))
            {
                // ���x�̑傫����ێ����Ȃ���������������ς���
                Vector2 airDir = Vector2.Lerp(new Vector2(_rb.velocity.x, _rb.velocity.z), new Vector2(dir.x, dir.z) * _rb.velocity.magnitude, Time.deltaTime * _turnSpeed);
                velo = new Vector3(airDir.x, _rb.velocity.y, airDir.y);
            }
            _rb.velocity = velo;
        }
        else
        {
            if (dir.magnitude == 0f)
            {
                velo.y = -10;
            }

            if (Input.GetButton("Jump"))
            {
                _jumped = true;
                _jumpedTimer = 0;
                velo.y = _jumpPower;
            }

            _rb.velocity = velo; // �ڒn����velocity������������
        }

        // Jump����
        //if (Input.GetButtonDown("Jump") && IsGround)
        //{
        //    _jumped = true;
        //    _jumpedTimer = 0;
        //    _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        //}
    }

    /// <summary>�ڒn���Ă��邩�𔻒肷��</summary>
    /// <returns></returns>
    //bool IsGround()
    //{
    //    Vector3 start = transform.position;
    //    Vector3 end = start + Vector3.down * _isGroundedLength;
    //    Debug.DrawLine(start, end);
    //    bool isGround = Physics.Linecast(start, end);
    //    return isGround;
    //}

    private void OnTriggerStay(Collider other)
    {
        IsGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }
}
