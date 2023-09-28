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
    /// <summary>�󒆂ł̕����]���̃X�s�[�h</summary>
    [SerializeField] float _turnSpeed = 3;
    Rigidbody _rb;
    /// <summary>�ڒn����̋���</summary>
    //float _isGroundedLength = 1.1f;
    bool _jumped;
    float _jumpedTimer;
    bool IsGround;
    Vector3 _planeNormalVector;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_isGroundedLength = GetComponent<CapsuleCollider>().height / 2 + 0.1f;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L)) { ScoreManager.Instance.SaveRanking(); }
        if (_jumped) // ��莞�Ԑڒn����ɂ����Ȃ�
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
        dir = dir.normalized; // �P�ʉ����Ă��鐅�������̓��̓x�N�g��
        Vector3 velo = dir * _moveSpeed;
        velo.y = _rb.velocity.y;
        if (!IsGround) // �󒆏���
        {
            // �󒆂ł����������]�����\
            velo = _rb.velocity;
            if (dir.magnitude != 0f)
            {
                // ���x�̑傫����ێ����Ȃ���������������ς���
                Vector2 startHoriVelo = new Vector2(_rb.velocity.x, _rb.velocity.z);
                float horiMag = startHoriVelo.magnitude;
                if (horiMag < 10f)
                {
                    horiMag = 10;
                }
                Vector2 endHoriVelo = new Vector2(dir.x * horiMag, dir.z * horiMag);
                float turnSpeed = _turnSpeed * Time.deltaTime;
                Vector2 airHoriVelo = endHoriVelo * turnSpeed + startHoriVelo * (1 - turnSpeed);
                velo = new Vector3(airHoriVelo.x, _rb.velocity.y, airHoriVelo.y);
            }
            _rb.velocity = velo;
        }
        else // �ڒn������
        {
            // �ڂ��Ă���ʂɉ������x�N�g���ɕς���
            Vector3 onPlaneVelo = Vector3.ProjectOnPlane(velo, _planeNormalVector);
            if (Input.GetButton("Jump"))
            {
                RbAddPower();
                onPlaneVelo.y = _jumpPower;
            }

            _rb.velocity = onPlaneVelo; // �ڒn����velocity������������
        }
    }

    /// <summary>�W�����v�Ȃǂ������Ƃ���莞�Ԑڒn����ɂ����Ȃ����߂̃��\�b�h</summary>
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

    private void OnCollisionStay(Collision collision)
    {
        float angle = Vector3.Angle(Vector3.up, collision.contacts[0].normal); // �ڂ��Ă���ʂ̖@���x�N�g��
        if (angle < 45)
        {
            _planeNormalVector = collision.contacts[0].normal;
        }
    }
}
