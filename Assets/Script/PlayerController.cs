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
    [SerializeField] float _airMaxSpeed = 30;
    /// <summary>�󒆂ł̕����]���̃X�s�[�h</summary>
    [SerializeField] float _turnSpeed = 3;
    Rigidbody _rb;
    /// <summary>�ڒn����̋���</summary>
    //float _isGroundedLength = 1.1f;
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
            if (_jumpedTimer > 0.3f)
            {
                _jumped = false;
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
                Vector2 startHoriVelo = new Vector2(_rb.velocity.x, _rb.velocity.z);
                Vector2 endHoriVelo = new Vector2(dir.x, dir.z) * startHoriVelo.magnitude;
                Vector2 airHoriVelo = Vector2.Lerp(startHoriVelo, endHoriVelo, _turnSpeed * Time.deltaTime).normalized * startHoriVelo.magnitude;
                velo = new Vector3(airHoriVelo.x, _rb.velocity.y, airHoriVelo.y);
            }

            //if (nowAirHoriVelo.magnitude > _airMaxSpeed) // Max Speed�ȏ�Ȃ琧������
            //{
            //    nowAirHoriVelo = nowAirHoriVelo.normalized * _airMaxSpeed;
            //    _rb.velocity = new Vector3(nowAirHoriVelo.x, _rb.velocity.y, nowAirHoriVelo.y);
            //}
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
                RbAddPower();
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

    public void RbAddPower()
    {
        _jumpedTimer = 0;
        _jumped = true;
    }

    private void OnTriggerStay(Collider other)
    {
        IsGround = true;
    }
    private void OnTriggerExit(Collider other)
    {
        IsGround = false;
    }
}
